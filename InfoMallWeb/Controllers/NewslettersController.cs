using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoMallWeb.Data;
using InfoMallWeb.Models;
using InfoMallWeb.Repository;
using InfoMallWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InfoMallWeb.Controllers
{
    public class NewslettersController : Controller
    {
        private readonly ApplicationDbContext _repo;
        private readonly IEmailSender _email;
        public NewslettersController(ApplicationDbContext ctx,  
            IEmailSender email)
        {
            _repo = ctx;
            _email = email;
        }
        public async ValueTask<IActionResult> Index()
        {
            var model = await _repo.Newsletters.ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody]NewsletterViewModel model)
        {
            var combinedUsers = await _repo.Newsletters.Where(x => x.IsSubsribed).Select(x => x.Email).ToListAsync();
            try
            {
                await _email.SendEmailToAllAsync(combinedUsers, model.Subject, model.Message);
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
        public IActionResult UnsubscribeUser()
        {
            return View();
        }

        public async ValueTask<IActionResult> Unsubscribe(string email)
        {
            bool result = false;
            var user = await _repo.Newsletters.Where(x => x.Email == email).FirstOrDefaultAsync();
            if (user != null)
            {
                user.IsSubsribed = false;
                _repo.Newsletters.Update(user);
                await _repo.SaveChangesAsync();
            }
            if (result) return NoContent();
            return BadRequest();
        }

        [HttpPost]
        public async ValueTask<IActionResult> Subscribe([FromBody]SubscribeViewModel viewModel)
        {
            if (viewModel.Email != null)
            {
                bool result;
                var checker = await _repo.Newsletters.FirstOrDefaultAsync(x => x.Email == viewModel.Email);
                if (checker != null && !checker.IsSubsribed)
                {
                    checker.IsSubsribed = true;
                    _repo.Newsletters.Update(checker);
                    await _repo.SaveChangesAsync();
                    result = true;
                }
                else
                {
                    var model = new Newsletter { Email = viewModel.Email, IsSubsribed = true };
                    _repo.Newsletters.Add(model);
                    await _repo.SaveChangesAsync();
                    result = true;
                }
                if (result) return NoContent();
            }
            return BadRequest();
        }

        

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async ValueTask<IActionResult> Create(Newsletter model)
        {
            if (ModelState.IsValid)
            {
                _repo.Newsletters.Add(model);
                await _repo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async ValueTask<IActionResult> Edit(string id)
        {
            var model = await _repo.Newsletters.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async ValueTask<IActionResult> Edit(Newsletter model)
        {
            if (ModelState.IsValid)
            {
                _repo.Newsletters.Update(model);
                await _repo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async ValueTask<IActionResult> Delete(string id)
        {
            var model = await _repo.Newsletters.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async ValueTask<IActionResult> DeleteConfirmed(string id)
        {
            var model = await _repo.Newsletters.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();
            _repo.Newsletters.Remove(model);
            await _repo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

    }
}

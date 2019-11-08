using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoMallWeb.Data;
using InfoMallWeb.Dtos;
using InfoMallWeb.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InfoMallWeb.Controllers
{
    public class ContentForMallsController : Controller
    {
        private readonly IContentForMallRepository _con;
        private readonly ApplicationDbContext _ctx;
        public ContentForMallsController(IContentForMallRepository content, ApplicationDbContext context)
        {
            _con = content;
            _ctx = context;
        }
        // GET: ContentsForMalls
        public async Task<IActionResult> Index()
        {
            var model = await _con.GetAllContents();
            return View(model);
        }

        // GET: ContentsForMalls/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _con.GetContentByID(id, true);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        
        // GET: ContentsForMalls/Create
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(_ctx.CategoriesForInformation, "CategoryId", "CategoryName");
            ViewData["Authors"] = new SelectList(_ctx.Author, "AuthorId", "Name");
            return View();
        }
        public async Task<IActionResult> Search(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var model = await _con.GetAllContents();
                var real = model.Where(x => x.Title.ToLower().Contains(message.ToLower()) || x.ShortDescription.ToLower().Contains(message.ToLower()) || x.LongDescription.ToLower().Contains(message.ToLower()))
                                .Select(x => new {x.Title, Id = x.ContentId }).Take(5).ToList();
                return Ok(real);

            }
            return BadRequest(new { message = "unexpected error"});
        }
        // POST: ContentsForMalls/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContentForMallDto content)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _con.AddContent(content);
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: ContentsForMalls/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _con.GetContentByID(id, false);
            if (model == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_ctx.CategoriesForInformation, "CategoryId", "CategoryName");
            ViewData["Authors"] = new SelectList(_ctx.Author, "AuthorId", "Name");
            return View(model);
        }

        // POST: ContentsForMalls/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContentForMallDto content)
        {
            if (id != content.ContentId)
            {
                return RedirectToAction("Error", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View("Edit", new { id });
            }
            try
            {
                await _con.UpdateContentWithID(content);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: ContentsForMalls/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _con.GetContentByID(id, false);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: ContentsForMalls/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, ContentForMallDto content)
        {
            if (id != content.ContentId)
            {
                return RedirectToAction("Error", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View("Delete", new { id });
            }
            try
            {
                _con.DeleteContentWithID(content);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
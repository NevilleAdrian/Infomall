using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoMallWeb.Data;
using InfoMallWeb.Models;
using InfoMallWeb.Repository;
using Microsoft.AspNetCore.Mvc;

namespace InfoMallWeb.Controllers
{
    public class ContactInfosController : Controller
    {
        private readonly IContactInfoRepository _con;
        public ContactInfosController(IContactInfoRepository contact,
            ApplicationDbContext context)
        {
            _con = contact;
        }

        // GET: ContactInfos
        public async Task<IActionResult> Index()
        {
            return View(await _con.GetAllContactsInfo());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _con.GetContactInfo(id);
            if(model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactInformation contact)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Edit", id);
            }
            if(id != contact.ContactId)
            {
                return RedirectToAction("Edit", id);
            }
            try
            {
                await _con.UpdateContactInfoById(contact);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
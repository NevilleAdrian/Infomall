using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoMallWeb.Data;
using InfoMallWeb.Dtos;
using InfoMallWeb.Models;
using InfoMallWeb.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InfoMallWeb.Controllers
{
    public class ContentImagesController : Controller
    {
        private readonly IContentImageRepository _con;
        private readonly ApplicationDbContext _ctx;
        public ContentImagesController(IContentImageRepository contentImage,
            ApplicationDbContext context)
        {
            _con = contentImage;
            _ctx = context;
        }

        // GET: ContentImages
        public async Task<IActionResult> Index()
        {
            return View(await _con.GetContentImages());
        }

        // GET: ContentImages/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContentImages/Create
        public IActionResult Create()
        {
            ViewData["Category"] = new SelectList(_ctx.ContentsForTab, "ContentId", "Title");
            return View();
        }

        // POST: ContentImages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContentImageDto content)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _con.AddContentImage(content);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: ContentImages/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _con.GetContentImageById(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_ctx.ContentsForTab, "ContentId", "Title", id);
            return View(model);
        }

        // POST: ContentImages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ContentImageDto content)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", new { id });
            }
            if (id != content.ContentImageId)
            {
                return RedirectToAction("Edit", new { id });
            }
            try
            {
                _con.EditContentImage(content);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: ContentImages/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _con.GetContentImageById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: ContentImages/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, ContentImage content)
        {
            if(id != content.ContentImageId)
            {
                return NotFound();
            }
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Delete", new { id });
            }
            try
            {
                _con.DeleteContentImage(content);
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");

            }
        }
    }
}
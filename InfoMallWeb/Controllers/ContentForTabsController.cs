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
    public class ContentForTabsController : Controller
    {
		private readonly IContentForTabRepository _con;
		private readonly ApplicationDbContext _ctx;
		public ContentForTabsController(IContentForTabRepository con,
			ApplicationDbContext context)
		{
			_con = con;
			_ctx = context;
		}
        // GET: ContentForTabs
        public async Task<ActionResult> Index()
        {
			var model = await _con.GetAllContents(false);
            return View(model);
        }

        // GET: ContentForTabs/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await _con.GetContentByID(id, true);
            if(model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // GET: ContentForTabs/Create
        public ActionResult Create()
        {
			ViewData["Category"] = new SelectList(_ctx.CategoriesForTab, "CategoryId", "CategoryName");
			return View();
        }

        // POST: ContentForTabs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContentForTabDto content)
        {
			if (!ModelState.IsValid)
			{
				return RedirectToAction(nameof(Index));
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

        // GET: ContentForTabs/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
			var model = await _con.GetContentByID(id, true);
            if (model == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_ctx.CategoriesForTab, "CategoryId", "CategoryName", id);
			return View(model);
        }

        // POST: ContentForTabs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContentForTabDto content)
        {
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Edit", new { id });
			}
			if (id != content.ContentId)
			{
				return RedirectToAction("Edit", new { id });
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

        // GET: ContentForTabs/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
			var model = await _con.GetContentByID(id, false);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
		}

        // POST: ContentForTabs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, ContentForTabDto content)
        {
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Delete", new { id });
			}
			if (id != content.ContentId)
			{
				return RedirectToAction("Delete", new { id });
			}
            try
            {
				await _con.DeleteContentWithID(content);
				
				return RedirectToAction(nameof(Index));
				
			}
            catch
            {
				return RedirectToAction("Error", "Home");

			}
		}
    }
}
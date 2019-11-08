using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoMallWeb.Dtos;
using InfoMallWeb.Models;
using InfoMallWeb.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfoMallWeb.Controllers
{
    public class CategoryForTabsController : Controller
    {
		private readonly ICategoryForTabRepository _cat;
		private readonly IBannerRepository _ban;

		public CategoryForTabsController(ICategoryForTabRepository cat,
		IBannerRepository banner)
		{
			_cat = cat;
			_ban = banner;
		}
        // GET: CategoryForTabs
        public async Task<IActionResult> Index()
        {
			var model = await _cat.GetAllCategories(false, false);
            return View(model);
        }

        // GET: CategoryForTabs/Details/5
        public async Task<ActionResult> Details(int id)
        {
			var model = await _cat.GetCategoryById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // GET: CategoryForTabs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryForTabs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryForTab categoryForTab)
        {
			if (ModelState.IsValid)
			{
				try
				{
					await _cat.AddCategory(categoryForTab);
					return RedirectToAction(nameof(Index));
				}
				catch
				{
					return RedirectToAction("Error", "Home");
				}
			}
			return View();
            
        }

        // GET: CategoryForTabs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
			var model = await _cat.GetCategoryById(id, false, false);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: CategoryForTabs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryForTab categoryForTab)
        {
			if (id != categoryForTab.CategoryId)
			{
				return View("Edit", new {id});
			}
			if (!ModelState.IsValid)
			{
				return View("Edit", new { id });
			}
			try
            {
				await _cat.UpdateCategoryById(categoryForTab);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
			}
        }

        // GET: CategoryForTabs/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
			var model = await _cat.GetCategoryById(id, false, false);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
		}

        // POST: CategoryForTabs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
				_cat.DeleteCategoryById(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
				return RedirectToAction("Error", "Home");
			}
        }
    }
}
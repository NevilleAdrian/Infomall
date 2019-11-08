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
    public class BannersController : Controller
    {
		private readonly IBannerRepository _ban;
		private readonly ApplicationDbContext _ctx;
		public BannersController(IBannerRepository banner,
			ApplicationDbContext context)
		{
			_ban = banner;
			_ctx = context;
		}

        // GET: Banners
        public async Task<IActionResult> Index()
        {
            return View(await _ban.GetAllBanners());
        }

        // GET: Banners/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Banners/Create
        public IActionResult Create()
        {
			ViewData["Category"] = new SelectList(_ctx.CategoriesForTab, "CategoryId", "CategoryName");
			return View();
        }

        // POST: Banners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerInformationDto banner)
        {
			if (!ModelState.IsValid)
			{
				return RedirectToAction(nameof(Index));
			}
            try
            {
				await _ban.AddBanner(banner);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
				return RedirectToAction("Error", "Home");
			}
        }

        // GET: Banners/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
			var model = await _ban.GetBannerByBannerId(id);
            if (model == null)
            {
                return NotFound();
            }
			ViewData["Category"] = new SelectList(_ctx.CategoriesForTab, "CategoryId", "CategoryName", id);
			return View(model);
        }

        // POST: Banners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BannerInformationDto banner)
        {
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Edit", new { id });
			}
			if (id != banner.BannerId)
			{
				return RedirectToAction("Edit", new { id });
			}
			try
            {
				await _ban.UpdateBannerWithId(banner);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
				return RedirectToAction("Error", "Home");
			}
		}

        // GET: Banners/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _ban.GetBannerByBannerId(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Banners/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
				_ban.DeleteBanner(id);
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
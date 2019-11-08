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
    public class PromotionsController : Controller
    {
        private readonly IPromotionRepository _promo;
        private readonly ApplicationDbContext _ctx;

        public PromotionsController(IPromotionRepository promo,
            ApplicationDbContext context)
        {
            _promo = promo;
            _ctx = context;
        }
        // GET: Promotions
        public async Task<ActionResult> Index()
        {
            var model = await _promo.GetAllPromotions(true, true);
            return View(model);
        }

        // GET: Promotions/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await _promo.GetPromotionById(id, true, true);
            if(model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // GET: Promotions/Create
        public ActionResult Create()
        {
            ViewData["Category"] = new SelectList(_ctx.CategoriesForTab, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Promotions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(bool sendToAll, PromotionDto promotion)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }
            try
            {
                var result = await _promo.AddPromotion(promotion, sendToAll);
                if(result.Item1)
                {
                    if(result.Item2)
                    {
                        ViewData["Mail"] = "Success";
                    }
                    else
                    {
                        ViewData["Mail"] = "Failure";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Create");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Promotions/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _promo.GetPromotionById(id, true, false);
            if (model == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_ctx.CategoriesForTab, "CategoryId", "CategoryName", id);
            return View(model);
        }

        // POST: Promotions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PromotionDto promotion)
        {
            if(id != promotion.PromotionId)
            {
                return RedirectToAction("Edit", new { id });
            }
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Edit", new { id });
            }
            try
            {
                bool result = await _promo.UpdatePromotionWithId(promotion);
                if(result)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction("Edit", new { id });

            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Promotions/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _promo.GetPromotionById(id, true, false);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Promotions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                await _promo.DeletePromotionWithId(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
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
    public class PromotionInformationsController : Controller
    {
        private readonly IPromotionInfoRepository _promo;
        private readonly ApplicationDbContext _ctx;

        public PromotionInformationsController(IPromotionInfoRepository promo,
            ApplicationDbContext context)
        {
            _promo = promo;
            _ctx = context;
        }
        // GET: Promotions
        public async Task<ActionResult> Index()
        {
            var model = await _promo.PromotionInformation();
            return View(model);
        }

        // GET: Promotions/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await _promo.GetPromotionInformationById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // GET: Promotions/Create
        public ActionResult Create()
        {
            ViewData["Category"] = new SelectList(_ctx.Promotions, "PromotionId", "PromotionName");
            return View();
        }

        // POST: Promotions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PromotionInformationDto promotionDto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create");
            }
            try
            {
                await _promo.AddPromotionInformation(promotionDto);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Promotions/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _promo.GetPromotionInformationById(id);
            if (model == null)
            {
                return NotFound();
            }
            ViewData["Category"] = new SelectList(_ctx.Promotions, "PromotionId", "PromotionName", id);

            return View(model);
        }

        // POST: Promotions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PromotionInformationDto promotionDto)
        {
            if (id != promotionDto.PromotionInformationId)
            {
                return RedirectToAction("Edit", new { id });
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", new { id });
            }
            try
            {
                await _promo.UpdatPromotionInformationWithId(promotionDto);
                
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Promotions/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _promo.GetPromotionInformationById(id);
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
                await _promo.DeletePromotionInformationById(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
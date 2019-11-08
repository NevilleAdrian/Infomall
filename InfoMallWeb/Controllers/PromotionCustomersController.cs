using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoMallWeb.Dtos;
using InfoMallWeb.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfoMallWeb.Controllers
{
    public class PromotionCustomersController : Controller
    {
        private readonly IPromotionCustomerRepository _promo;
        public PromotionCustomersController(IPromotionCustomerRepository promotion)
        {
            _promo = promotion;
        }
        // GET: PromotionCustomers
        public async Task<IActionResult> Index()
        {
            var model = await _promo.GetAllCustomerProduct();
            return View(model);
        }

        // GET: PromotionCustomers/Details/5
        public async Task<IActionResult> Details()
        {
            var model = await _promo.GetAllCustomerProductsByUserId();
            
            return View(model);
        }

        // GET: PromotionCustomers/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: PromotionCustomers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromotionCustomerDto promotion)
        {
            if(!ModelState.IsValid)
            {
                return View("Error", "Home");
            }
            try
            {
                var result = await _promo.AddCustomerProduct(promotion);

                if(result.Item1)
                {
                    if(result.Item2)
                    {
                        ViewData["Success"] = "Mail Sent";
                    }
                    else
                    {
                        ViewData["Success"] = "Not Sent";
                    }
                    return RedirectToAction(nameof(Details));
                }

                return View("Error", "Home");
            }
            catch
            {
                return View("Error", "Home");
            }
        }

        // GET: PromotionCustomers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _promo.GetCustomerProductById(id);
            if(model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: PromotionCustomers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PromotionCustomerDto promotionDto, bool payment = false)
        {
            if(id != promotionDto.PromotionCustomerId)
            {
                return RedirectToAction("Edit", new { id });
            }
            try
            {
                await _promo.UpdatePromotionCustomerWithId(promotionDto);
                // TODO: Add update logic here

                if(payment)
                {
                    return RedirectToAction("Details");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", "Home");
            }
        }

        // GET: PromotionCustomers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _promo.GetCustomerProductById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: PromotionCustomers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _promo.DeletePromotionCustomerWithId(id);
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", "Home");
            }
        }
    }
}
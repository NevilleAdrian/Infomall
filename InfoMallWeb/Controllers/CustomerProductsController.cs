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
using Microsoft.EntityFrameworkCore;

namespace InfoMallWeb.Controllers
{
    public class CustomerProductsController : Controller
    {
        private readonly ICustomerProductRepository _cust;
        private readonly ApplicationDbContext _ctx;
        public CustomerProductsController(ICustomerProductRepository customer, ApplicationDbContext context)
        {
            _cust = customer;
            _ctx = context;
        }
        // GET: PromotionCustomers
        public async Task<IActionResult> Index()
        {
            var model = await _cust.GetAllCustomerProducts();
            return View(model);
        }

        // GET: PromotionCustomers/Details/5
        public async Task<IActionResult> Details()
        {
            var model = await _cust.GetAllCustomerProducts();

            return View(model);
        }

        // GET: PromotionCustomers/Create
        public ActionResult Create()
        {
            ViewData["Category"] = new SelectList(_ctx.ContentsForTab.Include(c => c.Category).Where(c => c.Category.CategoryName.ToLower() == "services"), "Title", "Title", "Software Development");
            return View();
        }

        // POST: PromotionCustomers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustumerProductDto customerProduct)
        {
            if (!ModelState.IsValid)
            {
                return View("Error", "Home");
            }
            try
            {
                var result = await _cust.AddCustomerProduct(customerProduct);

                if (result.Item1)
                {
                    if (result.Item2)
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
            var model = await _cust.GetCustomerProductById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: PromotionCustomers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerProduct customerProduct, bool payment = false)
        {
            if (id != customerProduct.CustomerProductId)
            {
                return RedirectToAction("Edit", new { id });
            }
            try
            {
                await _cust.UpdateCustomerProductWithId(customerProduct);
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
            var model = await _cust.GetCustomerProductById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: PromotionCustomers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _cust.DeleteCustomerProductWithId(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", "Home");
            }
        }
    }
}
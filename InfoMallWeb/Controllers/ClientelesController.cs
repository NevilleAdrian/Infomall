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

    public class ClientelesController : Controller
    {
        private readonly IClienteleRepository _client;
        public ClientelesController(IClienteleRepository client)
        {
            _client = client;
        }
        // GET: Clienteles
        public async Task<IActionResult> Index()
        {
            var model = await _client.GetAllClientele();
            return View(model);
        }

        // GET: Clienteles/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _client.GetClienteleById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // GET: Clienteles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clienteles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteleDto clientele)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                await _client.AddClientele(clientele);
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Clienteles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _client.GetClienteleById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Clienteles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteleDto clientele)
        {
            if (id != clientele.ClienteleId)
            {
                return RedirectToAction("Error", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View("Edit", new { id });
            }
            try
            {
                await _client.UpdateClienteleWithId(clientele);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Clienteles/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _client.GetClienteleById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Clienteles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _client.DeleteClienteleWithId(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
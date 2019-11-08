using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoMallWeb.Models;
using InfoMallWeb.Repository;
using InfoMallWeb.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace InfoMallWeb.Controllers
{
	public class HomeController : Controller
	{
        private readonly IClienteleRepository _client;
        private readonly ICategoryForInfoRepository _catInfo;
        private readonly ICategoryForTabRepository _catTab;
        private readonly IContactInfoRepository _con;

        public HomeController(IClienteleRepository clientele,
            ICategoryForTabRepository categoryForTab,
            ICategoryForInfoRepository infoRepository,
            IContactInfoRepository contact)
        {
            _client = clientele;
            _catInfo = infoRepository;
             _catTab = categoryForTab;
            _con = contact;
        }
		public async Task<IActionResult> Index()
		{
            var clients = await _client.GetAllClientele();
            var infos = await _catInfo.GetAllCategories(true);
            var tabs = await _catTab.GetAllCategories();
            var model = new HomePageDto {
                Clienteles = clients,
                InfoCategories = infos,
                TabCategories = tabs
            };
			return View(model);
		}

		public async Task<IActionResult> Privacy()
		{
            var modelToGet = await _catTab.GetAllCategories(true, false);
            var model = modelToGet.Where(c => c.CategoryName.ToLower() == "privacy").Select(c => c.ContentForTabs.SingleOrDefault()).SingleOrDefault();
            return View(model);
		}

        public IActionResult Contact()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactConfirmed(ContactInformation contact)
        {
            var result = await _con.AddContact(contact);
            if(!result.Item1)
            {
                return BadRequest(new { Message = "an error occured" });
            }
            else
            {
                if(result.Item2 == "sent")
                {
                    return Ok(new { Added = true, MailSent = true });
                }
                else
                {
                    return Ok(new { Added = true, MailSent = false });

                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

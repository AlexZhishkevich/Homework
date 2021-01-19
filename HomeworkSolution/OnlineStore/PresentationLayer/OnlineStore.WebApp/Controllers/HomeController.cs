using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlineStore.Contracts.Interfaces;
using OnlineStore.Contracts.Models;
using OnlineStore.WebApp.Models;

namespace OnlineStore.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string UsernameKey = "username";

        private readonly ICatalogRepository _catalogRepository;
        private readonly IGoodRepository _goodRepository;

        public HomeController(ILogger<HomeController> logger, ICatalogRepository catalogRepository, IGoodRepository goodRepository)
        {
            _logger = logger;
            _catalogRepository = catalogRepository;
            _goodRepository = goodRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allCatalogs = _catalogRepository.GetAll();

            return View(allCatalogs);
        }

        [HttpGet]
        public IActionResult GetGoods(Catalog model)
        {
            var allCatalogs = _catalogRepository.GetAll();
            var goods = _goodRepository.GetGoodsOfCatalog(model);

            return View("Index", allCatalogs);
        }

        [HttpPost]
        public IActionResult Index(string username)
        {
            IActionResult result = null;
            if (!string.IsNullOrEmpty(username))
            {
                HttpContext.Session.SetString(UsernameKey, username);
                result = RedirectToAction(nameof(this.Index));
            }
            else
            {
                result = View();
            }

            return result;
        }

        public IActionResult Contacts()
        {
            var allCatalogs = _catalogRepository.GetAll();

            return View(allCatalogs);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

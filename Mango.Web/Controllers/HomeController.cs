using Mango.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

		public HomeController(ILogger<HomeController> logger, IProductService productService)
		{
			_logger = logger;
			_productService = productService;
		}

		public async Task<IActionResult> Index()
        {
			List<ProductDTO> objList = new();
			ResponseDTO response = await _productService.GetAllProductsAsync();
			if (response is not null && response.IsSuccess)
			{
				objList = JsonConvert.DeserializeObject<List<ProductDTO>>(
					Convert.ToString(response.Result));
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View(objList);
		}
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            ProductDTO obj = new();
            ResponseDTO response = await _productService.GetProductByIdAsync(id);
            if (response is not null && response.IsSuccess)
            {
                obj = JsonConvert.DeserializeObject<ProductDTO>(
                    Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(obj);
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
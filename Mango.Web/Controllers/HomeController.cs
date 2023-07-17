using Mango.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
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
            ProductDTO? obj = new();
            ResponseDTO? response = await _productService.GetProductByIdAsync(id);
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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Details(ProductDTO product)
        {
            CartDTO cart = new()
            {
                CartHeader = new()
                {
                    UserId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value
                }
            };
            CartDetailsDTO cartDetails = new()
            {
                Count = product.Count,
                ProductId = product.Id
            };
            List<CartDetailsDTO> cartDetailsList = new() { cartDetails };
            cart.CartDetails = cartDetailsList;

            var response = await _cartService.UpsertCartAsync(cart);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Item has been added to the Shopping Cart";
                return RedirectToAction("Index");
            }
            TempData["error"] = response?.Message;
            return View(product);
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
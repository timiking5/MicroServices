using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
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
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO response = await _productService.CreateProductAsync(model);
                if (response is not null && response.IsSuccess)
                {
                    TempData["success"] = "Product successfully craeted";
                    return RedirectToAction("Index");
                }
                TempData["error"] = response?.Message;
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response is not null && response.IsSuccess)
            {
                var product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(product);
            }
            TempData["error"] = response.Message;
            return NotFound();
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int id)
        {
            var response = await _productService.DeleteProductAsync(id);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Product successfully deleted";
                return RedirectToAction("Index");
            }
            TempData["error"] = response?.Message;
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response is not null && response.IsSuccess)
            {
                var product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(product);
            }
            TempData["error"] = response?.Message;
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDTO product)
        {
            var response = await _productService.UpdateProductAsync(product);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = response?.Message;
            return View(product);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(ICartService cartService, ICouponService couponService)
        {
            _cartService = cartService;
            _couponService = couponService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await LoadCartDTO());
        }
        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            ResponseDTO? response = await _cartService.RemoveCartAsync(cartDetailsId);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Item has been successfully removed";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTO cart)
        {
            cart.CartDetails = new List<CartDetailsDTO>();
            var response = await _cartService.ApplyCouponAsync(cart);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Coupon applied successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Unable to apply coupon";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDTO cart)
        {
            cart.CartDetails = new List<CartDetailsDTO>();
            cart.CartHeader.CouponCode = "";
            var response = await _cartService.ApplyCouponAsync(cart);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Coupon applied successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Unable to apply coupon";
            return RedirectToAction("Index");
        }
        private async Task<CartDTO> LoadCartDTO()
        {
            var userId = User.Claims?.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;
            var response = await _cartService.GetCartByUserIdAsync(userId);
            if (response is not null && response.IsSuccess)
            {
                CartDTO? cart = JsonConvert.DeserializeObject<CartDTO>(
                    Convert.ToString(response.Result));
                return cart;
            }
            return new();
        }

    }
}

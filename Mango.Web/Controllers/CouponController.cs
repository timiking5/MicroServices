using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> Index()
        {
            List<CouponDTO> objList = new();
            ResponseDTO response = await _couponService.GetAllCouponsAsync();
            if (response is not null && response.IsSuccess)
            {
                objList = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result));
            }
            return View(objList);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CouponDTO coupon)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO response = await _couponService.CreateCouponAsync(coupon);
                if (response is not null && response.IsSuccess)
                {
                    TempData["success"] = "Coupon successfully created";
                    return RedirectToAction("Index");
                }

            }
            return View(coupon);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _couponService.GetCouponByIdAsync(id);
            if (response is not null && response.IsSuccess)
            {
                var coupon = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
                return View(coupon);
            }
            TempData["error"] = response.Message;
            return NotFound();
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int id)
        {
            var response = await _couponService.DeleteCouponAsync(id);
            if (response is not null && response.IsSuccess)
            {
                TempData["success"] = "Coupon successfully deleted";
                return RedirectToAction("Index");
            }
            TempData["error"] = response.Message;
            return NotFound();
        }
    }
}

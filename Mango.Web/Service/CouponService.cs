using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using System.Threading.Tasks;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService service)
        {
            _baseService = service;
        }
        public async Task<ResponseDTO?> CreateCouponAsync(CouponDTO coupon)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.CouponAPIBase + $"/api/coupon",
                Data = coupon
            });
        }

        public async Task<ResponseDTO?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponAPIBase + $"/api/coupon/{id}"
            });
        }

        public async Task<ResponseDTO?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDTO?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/coupon/GetByCode/{couponCode}"
            });
        }

        public async Task<ResponseDTO?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/coupon/{id}"
            });
        }

        public async Task<ResponseDTO?> UpdateCouponAsync(CouponDTO coupon)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = SD.ApiType.PUT,
                Url = SD.CouponAPIBase + $"/api/coupon",
                Data = coupon
            });
        }
    }
}

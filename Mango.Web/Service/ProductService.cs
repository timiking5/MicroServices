global using Mango.Web.Models.DTO;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> CreateProductAsync(ProductDTO coupon)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.ProductAPIBase + $"/api/product",
                Data = coupon
            });
        }

        public async Task<ResponseDTO?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductAPIBase + $"/api/product/{id}"
            });
        }

        public async Task<ResponseDTO?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDTO?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + $"/api/product/{id}"
            });
        }

        public async Task<ResponseDTO?> UpdateProductAsync(ProductDTO coupon)
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

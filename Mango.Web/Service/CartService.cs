namespace Mango.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTO?> ApplyCouponAsync(CartDTO cart)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = cart,
                Url = SD.CartAPIBase + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDTO?> GetCartByUserIdAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CartAPIBase + $"/api/cart/GetCart/{userId}"
            });
        }

        public async Task<ResponseDTO?> RemoveCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.DELETE,
                Data = cartDetailsId,
                Url = SD.CartAPIBase + "/api/cart/RemoveCart"
            });
        }

        public async Task<ResponseDTO?> RemoveCouponAsync(CartDTO cart)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = cart,
                Url = SD.CartAPIBase + "/api/cart/RemoveCoupon"
            });
        }

        public async Task<ResponseDTO?> UpsertCartAsync(CartDTO cart)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = cart,
                Url = SD.CartAPIBase + "/api/cart/Upsert"
            });
        }
    }
}

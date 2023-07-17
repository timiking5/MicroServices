namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDTO?> GetCartByUserIdAsync(string userId);
        Task<ResponseDTO?> UpsertCartAsync(CartDTO cart);
        Task<ResponseDTO?> RemoveCartAsync(int cartDetailsId);
        Task<ResponseDTO?> ApplyCouponAsync(CartDTO cart);
        Task<ResponseDTO?> RemoveCouponAsync(CartDTO cart);

    }
}

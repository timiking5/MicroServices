namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDTO?> GetAllProductsAsync();
        Task<ResponseDTO?> GetProductByIdAsync(int id);
        Task<ResponseDTO?> CreateProductAsync(ProductDTO coupon);
        Task<ResponseDTO?> UpdateProductAsync(ProductDTO coupon);
        Task<ResponseDTO?> DeleteProductAsync(int id);
    }
}

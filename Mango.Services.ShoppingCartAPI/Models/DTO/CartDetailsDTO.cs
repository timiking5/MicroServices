namespace Mango.Services.ShoppingCartAPI.Models.DTO
{
    public class CartDetailsDTO
    {
        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeader? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDTO? Product { get; set; }
        public int Count { get; set; }
    }
}

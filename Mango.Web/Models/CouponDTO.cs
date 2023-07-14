namespace Mango.Web.Models
{ 
    public class CouponDTO
    {
        public int Id { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinimalAmount { get; set; }
    }
}

using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using Mango.Services.ShoppingCartAPI.Service.IService;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly AppDbContext _db;
        private ResponseDTO _response;

        public CartAPIController(IMapper mapper, AppDbContext db, IProductService productService, ICouponService couponService)
        {
            _productService = productService;
            _couponService = couponService;
            _mapper = mapper;
            _db = db;
            _response = new();
        }

        [HttpPost("Upsert")]
        public async Task<ResponseDTO> Upsert(CartDTO cart)
        {
            try
            {
                var headerFromDb = await _db.Headers.FirstOrDefaultAsync(x => x.Id == cart.CartHeader.Id);
                if (headerFromDb is null)
                {
                    // create header and details;
                    CartHeader header = _mapper.Map<CartHeader>(cart.CartHeader);
                    _db.Headers.Add(header);
                    await _db.SaveChangesAsync();
                    cart.CartDetails.First().CartHeaderId = header.Id;
                    var obj = _mapper.Map<CartDetails>(cart.CartDetails.First());
                    obj.CartHeader = header;
                    _db.Details.Add(obj);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var detailsFromDb = await _db.Details.FirstOrDefaultAsync(
                        x => x.ProductId == cart.CartDetails.First().ProductId && x.CartHeaderId == headerFromDb.Id);
                    if (detailsFromDb is null)
                    {
                        cart.CartDetails.First().CartHeaderId = headerFromDb.Id;
                        var obj = _mapper.Map<CartDetails>(cart.CartDetails.First());
                        obj.CartHeader = headerFromDb;
                        _db.Details.Add(obj);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        // update count in cart details;
                        detailsFromDb.Count += cart.CartDetails.First().Count;
                        _db.Details.Update(detailsFromDb);
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpDelete("RemoveCart")]
        public async Task<ResponseDTO> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                var cartDetails = _db.Details.First(x => x.Id == cartDetailsId);
                int totalDetailsCount = _db.Details.Where(x => x.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.Details.Remove(cartDetails);
                if (totalDetailsCount == 1)
                {
                    var headerFromDb = await _db.Headers.FirstOrDefaultAsync(x => x.Id == cartDetails.CartHeaderId);
                    _db.Headers.Remove(headerFromDb);
                }
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDTO> GetCart(string userId)
        {
            try
            {
                CartDTO cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDTO>(_db.Headers.First(x => x.UserId == userId))
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDTO>>(_db.Details
                    .Where(x => x.CartHeaderId == cart.CartHeader.Id));

                IEnumerable<ProductDTO> productDTOs = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDTOs.FirstOrDefault(x => x.Id == item.ProductId);
                    cart.CartHeader.CartTotal += item.Count * item.Product.Price;
                }

                if (cart.CartHeader.CouponId is not null && cart.CartHeader.CouponId != 0)
                {
                    CouponDTO coupon = await _couponService.GetCoupon((int)cart.CartHeader.CouponId);
                    if (coupon is not null && coupon.MinimalAmount <= cart.CartHeader.CartTotal)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDTO cart)
        {
            try
            {
                var cartFromDb = await _db.Headers.FirstAsync(x => x.UserId == cart.CartHeader.UserId);
                cartFromDb.CouponId = cart.CartHeader.CouponId;
                _db.Headers.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] CartDTO cart)
        {
            try
            {
                var cartFromDb = await _db.Headers.FirstAsync(x => x.UserId == cart.CartHeader.UserId);
                cartFromDb.CouponId = 0;
                _db.Headers.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }
    }
}

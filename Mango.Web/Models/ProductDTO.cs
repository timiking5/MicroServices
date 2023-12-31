﻿namespace Mango.Web.Models.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        /// <summary>
        /// Number of product to add to the cart
        /// </summary>
        [Range(1, 100)]
        public int Count { get; set; } = 1;
    }
}

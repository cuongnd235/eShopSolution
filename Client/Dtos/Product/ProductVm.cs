using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Client.Dtos.Product
{
    public class ProductVm
    {
        public string Id { set; get; }
        [Display(Name = "Mã sản phẩm")]
        public string Code { set; get; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { set; get; }
        [Display(Name = "Giá nhập")]
        public decimal? ImportPrice { set; get; }
        [Display(Name = "Giá bán")]
        public decimal? Price { set; get; }
        [Display(Name = "số lượng tồn")]
        public int? QuantityInStock { set; get; }
    }
}
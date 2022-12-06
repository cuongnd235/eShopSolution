﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Client.Dtos.Product
{
    public class ProductVm
    {
        public int Id { set; get; }

        [Display(Name = "Giá")]
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; }
        public int Stock { set; get; }
        public int ViewCount { set; get; }
        public DateTime DateCreated { set; get; }

        [Display(Name = "Tên")]
        public string Name { set; get; }
        [Display(Name = "Mô tả")]
        public string Description { set; get; }
        [Display(Name = "Chi tiết")]
        public string Details { set; get; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }
        public string LanguageId { set; get; }

        public bool? IsFeatured { get; set; }

        public string ThumbnailImage { get; set; }

        public List<string> Categories { get; set; } = new List<string>();
    }
}
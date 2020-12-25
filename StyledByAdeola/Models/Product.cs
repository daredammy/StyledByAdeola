using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace StyledByAdeola.Models
{
    public class Product: ProductBase
    {
        public int ProductID { get; set; }

        public string ProducPage { get; set; }

        public string ProductURL { get; set; }

        [Required(ErrorMessage = "Please enter a product name")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        public string SKU { get; set; }

        public string OptionName1 { get; set; }

        public string OptionValue1 { get; set; }

        public string OptionName2 { get; set; }

        public string OptionValue2 { get; set; }

        public string OptionName3 { get; set; }

        public string OptionValue3 { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public int SalePrice { get; set; }

        public bool OnSale { get; set; }

        public int Stock { get; set; } = 1000;

        public string Categories { get; set; }

        public int Weight { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool Visible { get; set; }

        public string ImageUrls { get; set; }
    }
}

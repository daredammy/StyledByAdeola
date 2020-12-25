using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace StyledByAdeola.Models
{
    public class ProductDocDb : ProductBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

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

        public Dictionary<string, string[]> OptionValuesPricePairs { get; set; }

        public Dictionary<string, string[]> OptionNamesValuePairs { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal SalePrice { get; set; }

        public string OnSale { get; set; }

        public string Stock { get; set; } = "1000";

        public string Categories { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public int Weight { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Visible { get; set; }

        public List<Rating> Ratings { get; set; }

        public string ImageUrls { get; set; }
    }
}

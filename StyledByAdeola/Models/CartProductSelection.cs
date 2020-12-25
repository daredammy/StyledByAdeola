namespace StyledByAdeola.Models {

    public class ProductSelection {
        public string productId { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string imageUrl { get; set; }
        public string[] selectedOptionsArray { get; set; }
        public decimal quantity { get; set; }
    }
}

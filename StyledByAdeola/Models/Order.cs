using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace StyledByAdeola.Models
{

    public class Order
    {
        //[BindNever]
        public string id { get; set; } = Guid.NewGuid().ToString();

        //[Required(ErrorMessage = "Please enter an email address")]
        public string email { get; set; }

        //[Required(ErrorMessage = "Please enter a first name")]
        public string firstName { get; set; }        
        
        //[Required(ErrorMessage = "Please enter a last name")]
        public string lastName { get; set; }

        //[Required(ErrorMessage = "Please enter address line 1")]
        public string address1 { get; set; }        
        
        //[Required(ErrorMessage = "Please enter address line 2")]
        public string address2 { get; set; }

        //[Required(ErrorMessage = "Please enter a country name")]
        public string country { get; set; }
        public string zip { get; set; }

       // [Required(ErrorMessage = "Please enter a city name")]
        public string city { get; set; }

       // [Required(ErrorMessage = "Please enter a state name")]
        public string state { get; set; }        
        
        public string phoneNumber { get; set; }

        //[BindNever]
        public bool shipped { get; set; }

        //[Required]
        public Payment payment { get; set; }

        //[BindNever]
        public ICollection<CartLine> products { get; set; }
    }

    public class Payment
    {
        //[BindNever]
        public long PaymentId { get; set; }
        //[Required]
        public string CardNumber { get; set; }
        //[Required]
        public string CardExpiry { get; set; }
        //[Required]
        public string CardSecurityCode { get; set; }
        [BindNever]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Total { get; set; }
        [BindNever]
        public string AuthCode { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace StyledByAdeola.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public virtual void AddItem(ProductDocDb product, int quantity)
        {
            CartLine line = lineCollection.Where(p => p.productId == product.Id).FirstOrDefault();

            if(line  == null)
            {
                lineCollection.Add(new CartLine
                {
                    productId = product.Id,
                    quantity = quantity
                });
            }

            else
            {
                line.quantity += quantity;
            }
        }
        public virtual void RemoveLine(ProductDocDb product) => 
            lineCollection.RemoveAll(l => 
            l.productId == product.Id);

        public virtual decimal ComputeTotalValue() =>                         
            lineCollection.Sum(e => e.price* e.quantity);

        public virtual void Clear() => lineCollection.Clear();
        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }

    public class CartLine {
        public string productId { get; set; }
        
        public string name { get; set; }
        
        public decimal price { get; set; }    

        public int quantity { get; set; }        
        
        public string[] selectedOptionsArray { get; set; }        
        
        public string imageUrl { get; set; }
    }
}

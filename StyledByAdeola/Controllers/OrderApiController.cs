using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StyledByAdeola.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;

namespace StyledByAdeola.Controllers {

    [Route("/api/orders")]
    [ApiController]
    //[Authorize(Roles = "Administrator")]    
    [AutoValidateAntiforgeryToken]
    public class OrderApiController : Controller {
        private IOrderRepository repository;
        private IProductRepository<ProductDocDb> productrepository;
        public OrderApiController(IOrderRepository repoService, IProductRepository<ProductDocDb> productRepo) {
            repository = repoService;
            productrepository = productRepo;
        }

        [HttpGet]
        public IQueryable<Order> GetOrders() {
            return repository.Orders;
        }

        [HttpPost("{id}")]
        public void MarkShipped(string orderID) {
            Order order = repository.Orders.FirstOrDefault(o => o.id == orderID);
            if (order != null) {
                order.shipped = true;
                repository.SaveOrder(order, newOrder: false);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrder([FromBody] Order order) {
            //if (ModelState.IsValid) {
            order.id = Guid.NewGuid().ToString();
            order.shipped = false;
            order.payment.Total = await GetTotalPrice(order.products).ConfigureAwait(false);

            ProcessPayment(order.payment);
            //if (order.Payment.AuthCode != null) {
            repository.SaveOrder(order, newOrder: true);
            return Ok(new {
                order.id,
                authCode = order.payment.AuthCode,
                amount = order.payment.Total
            });
            //}
            //else
            //{
            //    return BadRequest("Payment rejected");
            //}
            //}
            //return BadRequest(ModelState);
        }

        [HttpGet("GetTotalPrice")]
        private async Task<decimal> GetTotalPrice(IEnumerable<CartLine> lines) {
            //IEnumerable<string> ids = lines.Select(l => l.productId);
            //IEnumerable<ProductDocDb> prods = allMainProducts.Where(p => ids.Contains(p.Id));
            
            IEnumerable<ProductDocDb> allMainProducts = await productrepository.MainProducts().ConfigureAwait(false);
            decimal totalPrice = 0;
            foreach(CartLine cartLine in lines)
            {
                string optionValuesPricePair = string.Join(string.Empty, cartLine.selectedOptionsArray);
                ProductDocDb product = allMainProducts.Where(p => p.Id==cartLine.productId).FirstOrDefault();
                int price = int.Parse(product.OptionValuesPricePairs[optionValuesPricePair][0]);
                totalPrice += price;
            }
            return totalPrice;

            //return prods.Select(p => lines
            //            .First(l => l.productId == p.Id).quantity * p.OptionValuesPricePairs[lines.] * Price)
            //            .Sum();
        }

        private void ProcessPayment(Payment payment) {
            // integrate your payment system here
            payment.AuthCode = "12345";
        }
    }
}

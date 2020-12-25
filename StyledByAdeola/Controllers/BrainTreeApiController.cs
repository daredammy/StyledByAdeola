using System.Web.Http;
using Braintree;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using StyledByAdeola.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace StyledByAdeola.ApiControllers
{
    [Route("api/braintree")]
    [ApiController]
    [AutoValidateAntiforgeryToken]
    public class BrainTreeApiController : Controller
    {
        private IProductRepository<ProductDocDb> productRepository;
        private IConfiguration configurations;
        public BrainTreeApiController( IProductRepository<ProductDocDb> productRepo, IConfiguration config)
        {
            productRepository = productRepo;
            configurations = config;
        }

        public class ClientToken
        {
            public string token { get; set; }

            public ClientToken(string token)
            {
                this.token = token;
            }
        }

        public class Nonce
        {
            public string nonce { get; set; }
            public decimal chargeAmount { get; set; }

            public Nonce(string nonce)
            {
                this.nonce = nonce;
                this.chargeAmount = chargeAmount;
            }
        }

        [HttpGet]
        public IActionResult GetClientToken()
        {
            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "4pcx3v27hg6qmtqg",
                PublicKey = "8q77c835jvrcszvc",
                PrivateKey = configurations.GetValue<string>("BraintreeGateway:PrivateKey")
            };

            var clientToken = gateway.ClientToken.Generate();
            ClientToken ct = new ClientToken(clientToken);
            return Ok(ct);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Post([FromBody] Nonce nonce)
        {
            var gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "4pcx3v27hg6qmtqg",
                PublicKey = "8q77c835jvrcszvc",
                PrivateKey = configurations.GetValue<string>("BraintreeGateway:PrivateKey")
            };

            var cart = HttpContext.Session.GetString("cart");
            decimal shippingCost = new decimal (10.75);
            CartLine[] cartJson = JsonConvert.DeserializeObject<CartLine[]>(cart);
            decimal totalPrice = await GetTotalPrice(cartJson).ConfigureAwait(false);

            var request = new TransactionRequest
            {
                Amount = totalPrice + shippingCost,
                PaymentMethodNonce = nonce.nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = await gateway.Transaction.SaleAsync(request).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("GetTotalPrice")]
        private async Task<decimal> GetTotalPrice(IEnumerable<CartLine> lines)
        {
            IEnumerable<ProductDocDb> allMainProducts = await productRepository.MainProducts().ConfigureAwait(false);
            decimal totalPrice = 0;
            foreach (CartLine cartLine in lines)
            {
                string optionValuesPricePair = string.Join(string.Empty, cartLine.selectedOptionsArray);
                ProductDocDb product = allMainProducts.Where(p => p.Id == cartLine.productId).FirstOrDefault();
                int price = int.Parse(product.OptionValuesPricePairs[optionValuesPricePair][0]);
                totalPrice += price;
            }
            return totalPrice;
        }
    }
}

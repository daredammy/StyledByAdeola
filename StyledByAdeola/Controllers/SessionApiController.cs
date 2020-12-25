using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StyledByAdeola.Models;
using StyledByAdeola.Models.ViewModels;


namespace StyledByAdeola.Controllers {
    [Route("/api/session")]
    [ApiController]
    [AutoValidateAntiforgeryToken]        
    public class SessionApiController : Controller {

        [HttpGet("cart")]
        public IActionResult GetCart() {
            return Ok(HttpContext.Session.GetString("cart"));
        }

        [HttpPost("cart")]
        [IgnoreAntiforgeryToken]
        public void StoreCart([FromBody] ProductSelection[] products) {
            if (ModelState.IsValid)
            {
                var jsonData = JsonConvert.SerializeObject(products);
                HttpContext.Session.SetString("cart", jsonData);
            }
            else
            {
                BadRequest(ModelState);
            }
        }

        [HttpGet("checkout")]
        public IActionResult GetCheckout() {
            return Ok(HttpContext.Session.GetString("checkout"));
        }

        [HttpPost("checkout")]
        [IgnoreAntiforgeryToken]
        public void StoreCheckout([FromBody] CheckoutState data) {
            HttpContext.Session.SetString("checkout", 
                JsonConvert.SerializeObject(data));
        }
    }
}

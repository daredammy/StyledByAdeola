using System;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using StyledByAdeola.Models;
using StyledByAdeola.ServiceContracts;
using StyledByAdeola.Services;
using Microsoft.Extensions.Logging;

namespace StyledByAdeola.Controllers
{
    [Route("api/products")]
    [ApiController]
    [AutoValidateAntiforgeryToken]
    public class ProductApiController : Controller
    {
        private IProductRepository<ProductDocDb> repository;
        private  IBlobStorage blobStorage;
        private readonly ILogger<ProductApiController> logger;
        public ProductApiController(IProductRepository<ProductDocDb> repo, IBlobStorage blobStorage, ILogger<ProductApiController> logger)
        {
            this.repository = repo;
            this.blobStorage = blobStorage;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts(string category, string search,
                bool related = false, bool metadata = false)
        {
            IQueryable<ProductDocDb> products = await repository.MainProducts().ConfigureAwait(false);

            try
            {
                products = products.OrderByDescending(p => p.Tags != null).ThenBy(p => p.Tags != null ? Int32.Parse(p.Tags.ElementAtOrDefault(0)) : int.MaxValue);
            }
            catch (FormatException)
            {
                logger.LogError("The product tag at [0] is not a valid integer");
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                string catLower = category?.ToLower();
                products = products.Where(p => p.Categories.ToLower().Contains(catLower));
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchLower = search.ToLower();
                products = products.Where(p => p.Title.ToLower().Contains(searchLower)
                    || p.Description.ToLower().Contains(searchLower));
            }

            if (metadata)
            {
                return await CreateMetadata(products).ConfigureAwait(false);
            }

            return Ok(products);

            //if (related && HttpContext.User.IsInRole("Administrator"))
            //{
            //}
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetProduct(string id)
        {
            ProductDocDb product = await repository.GetProduct(id).ConfigureAwait(false);
            return Ok(product);
        }

        private async Task<IActionResult> CreateMetadata<T>(IEnumerable<T> products)
        {
            IQueryable<ProductDocDb> mainProducts = await repository.MainProducts().ConfigureAwait(false);
            return Ok(new
            {
                data = products,
                categories = mainProducts.Select(p => p.Categories).Where(pc => pc != "" && pc!= null)
                    .Distinct().OrderBy(c => c)
            });
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product pdata)
        {
            if (ModelState.IsValid)
            {
                ProductDocDb docDbProduct = new ProductDocDb
                {
                    Title= pdata.Title,
                    Description =pdata.Description,
                    Price =pdata.Price,
                    Categories = pdata.Categories,
                    ImageUrls = pdata.ImageUrls
                };
                UploadImage(docDbProduct);
                repository.SaveProduct(docDbProduct);
                return Ok(docDbProduct.Id);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceProduct(string id, [FromBody] ProductDocDb pdata)
        {
            if (ModelState.IsValid)
            {
                //Guid.TryParse(id, out Guid productId);
                pdata.Id = id;
                UploadImage(pdata);
                repository.SaveProduct(pdata, newProduct:false);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct(string id,
                [FromBody]JsonPatchDocument<ProductDocDb> patch)
        {
            IQueryable<ProductDocDb> products = await repository.MainProducts().ConfigureAwait(false);
            ProductDocDb product = products.First(p => p.Id == id);

            if (ModelState.IsValid && TryValidateModel(product))
            {
                UploadImage(product);
                await repository.SaveProduct(product, newProduct:false).ConfigureAwait(false);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task DeleteProduct(string id)
        {
            await repository.DeleteProduct(id).ConfigureAwait(false);
        }

        private void UploadImage(ProductDocDb product)
        {
            byte[] byteArray = Convert.FromBase64String(product.ImageUrls);
            string name = product.Title + ".jpg";
            using MemoryStream stream = new MemoryStream(byteArray, writable: false);
            blobStorage.UploadFileToStorage(BlobContainers.products, name, stream);
        }
    }
}

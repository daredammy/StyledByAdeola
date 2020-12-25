using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StyledByAdeola.ServiceContracts;

namespace StyledByAdeola.Models{
    public class CosmosProductRepository : IProductRepository<ProductDocDb> {
        private readonly ICosmosDb cosmosDbService;

        private IQueryable<ProductDocDb> MainProductCache;

        public CosmosProductRepository(ICosmosDb cosmosDbService_)
        {
            cosmosDbService = cosmosDbService_;
        }

        public async Task<IQueryable<ProductDocDb>> MainProducts()  {
            if (MainProductCache == null || !MainProductCache.Any())
            {
                IEnumerable<ProductDocDb> MainProduct = await cosmosDbService.GetItemsAsync<ProductDocDb>(CosmosDbContainers.Products, $"SELECT * FROM c WHERE  c.Title <> null AND c.Visible <> \"No\"")
                                                        .ConfigureAwait(false);
                MainProductCache = MainProduct.AsQueryable();

            }
            return MainProductCache;
        }

        public async Task<ProductDocDb> GetProduct(string productId)
        {
            ProductDocDb product = await cosmosDbService.GetItemAsync<ProductDocDb>(CosmosDbContainers.Products, productId).ConfigureAwait(false);
            return product;
        }

        public async Task<IQueryable<ProductDocDb>> SubProducts(string productId)
        {
            IEnumerable<ProductDocDb> subProducts = await cosmosDbService.GetItemsAsync<ProductDocDb>(CosmosDbContainers.Products, 
                                                    $"SELECT * FROM c WHERE c.ProductID = \"{productId}\"").ConfigureAwait(false);
            return subProducts.AsQueryable();
        }

        public async Task SaveProduct(ProductDocDb docDbProduct, bool newProduct = true)
        {
            docDbProduct.ImageUrls = null;
            if (newProduct)
            {
                await cosmosDbService.AddItemAsync<ProductDocDb>(CosmosDbContainers.Products, docDbProduct).ConfigureAwait(false);
            }
            else
            {
                await cosmosDbService.UpdateItemAsync<ProductDocDb>(CosmosDbContainers.Products, docDbProduct).ConfigureAwait(false);
            }
        }

        public async Task<ProductDocDb> DeleteProduct(string productId)
        {
            IQueryable<ProductDocDb> mainProducts = await MainProducts().ConfigureAwait(false);
            cosmosDbService.DeleteItemAsync<ProductDocDb>(CosmosDbContainers.Products, productId).GetAwaiter().GetResult();
            return mainProducts.Where(p => p.Id.ToString() == productId).FirstOrDefault();
        }
    }
}


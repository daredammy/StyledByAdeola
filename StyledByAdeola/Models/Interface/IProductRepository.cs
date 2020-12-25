using System.Linq;
using System.Collections.Generic;
using StyledByAdeola.Models;
using System.Threading.Tasks;

namespace StyledByAdeola.Models
{
    public interface IProductRepository<T> where T : ProductBase
    {
        Task<IQueryable<T>> MainProducts();
        Task<IQueryable<T>> SubProducts(string productId);
        Task<T> GetProduct(string productId);
        Task SaveProduct(T product, bool newProduct = true);
        Task<T> DeleteProduct(string productId);
    }
}
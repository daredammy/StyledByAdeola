using Microsoft.EntityFrameworkCore;
using System.Linq;
using StyledByAdeola.ServiceContracts;

namespace StyledByAdeola.Models
{
    public class CosmosOrderRepository : IOrderRepository
    {
        private readonly ICosmosDb cosmosDbService;

        private IQueryable<Order> orderCache;
        public CosmosOrderRepository(ICosmosDb cosmosDbService_)
        {
            cosmosDbService = cosmosDbService_;
        }
        
        public IQueryable<Order> Orders {
            get
            {
                if (orderCache == null || !orderCache.Any())
                {
                    orderCache = cosmosDbService.GetItemsAsync<Order>(CosmosDbContainers.Orders, "SELECT * FROM c")
                                                         .GetAwaiter().GetResult().AsQueryable();
                }
                return orderCache;
            }
        } 

        public void SaveOrder(Order order, bool newOrder = true)
        {
            if (newOrder)
            {
                cosmosDbService.AddItemAsync<Order>(CosmosDbContainers.Orders, order).GetAwaiter().GetResult();
            }
            else
            {
                cosmosDbService.UpdateItemAsync<Order>(CosmosDbContainers.Orders, order).GetAwaiter().GetResult();
            }
        }
    }
}
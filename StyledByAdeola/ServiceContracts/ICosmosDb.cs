using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StyledByAdeola.Models;

namespace StyledByAdeola.ServiceContracts
{
    public interface ICosmosDb
    {
        Task<IEnumerable<T>> GetItemsAsync<T>(CosmosDbContainers containerName, string queryString);
        Task<T> GetItemAsync<T>(CosmosDbContainers containerName, string id);
        Task AddItemAsync<T>(CosmosDbContainers containerName, T item);
        Task AddItemsAsync<T>(CosmosDbContainers containerName, List<T> items);
        Task UpdateItemAsync<T>(CosmosDbContainers containerName, T item);
        Task DeleteItemAsync<T>(CosmosDbContainers containerName, string id);
    }
}

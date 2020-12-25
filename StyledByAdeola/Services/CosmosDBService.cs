using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StyledByAdeola.Models;
using Microsoft.Azure.Cosmos;
using StyledByAdeola.ServiceContracts;

namespace StyledByAdeola.Services
{
    public class CosmosDBService : ICosmosDb
    {
        private string databaseName;

        private CosmosClient dbClient;

        public CosmosDBService(
            CosmosClient dbClient,
            string databaseName)
        {
            this.databaseName = databaseName;
            this.dbClient = dbClient;
            Database database = dbClient.GetDatabase(databaseName);

            foreach (CosmosDbContainers cosmosDbContainer in Enum.GetValues(typeof(CosmosDbContainers)))
            {
                if (cosmosDbContainer == CosmosDbContainers.ProductsSquare)
                {
                    database.CreateContainerIfNotExistsAsync(cosmosDbContainer.ToString(), "/ProductID");
                }
                else
                {
                    database.CreateContainerIfNotExistsAsync(cosmosDbContainer.ToString(), "/id");
                }
            }
        }

        private Container SetupContainer(CosmosDbContainers containerName)
        {
            return dbClient.GetContainer(databaseName, containerName.ToString());
        }

        public async Task AddItemAsync<T>(CosmosDbContainers containerName, T item)
        {
            Container conatiner = SetupContainer(containerName);
            await conatiner.CreateItemAsync<T>(item).ConfigureAwait(false);
        }

        public async Task AddItemsAsync<T>(CosmosDbContainers containerName, List<T> items)
        {
            Container conatiner = SetupContainer(containerName);
            List<Task> TaskList = new List<Task>() { };
            foreach (T item in items)
            {
                Task<ItemResponse<T>> LastTask = conatiner.CreateItemAsync<T>(item);
                TaskList.Add(LastTask);
            }
            await Task.WhenAll(TaskList.Where(t => t != null)).ConfigureAwait(false);
        }

        public async Task DeleteItemAsync<T>(CosmosDbContainers containerName, string id)
        {
            Container conatiner = SetupContainer(containerName);
            await conatiner.DeleteItemAsync<T>(id, new PartitionKey(id)).ConfigureAwait(false);
        }

        public async Task<T> GetItemAsync<T>(CosmosDbContainers containerName, string id)
        {
            Container container = SetupContainer(containerName);
            try
            {
                ItemResponse<T> response = await container.ReadItemAsync<T>(id, new PartitionKey(id)).ConfigureAwait(false);
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(CosmosDbContainers containerName, string queryString)
        {
            Container conatiner = SetupContainer(containerName);
            var query = conatiner.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync().ConfigureAwait(false);

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync<T>(CosmosDbContainers containerName, T item)
        {
            Container conatiner = SetupContainer(containerName);
            await conatiner.UpsertItemAsync<T>(item).ConfigureAwait(false);
        }
    }
}


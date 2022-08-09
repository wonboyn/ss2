using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace SelfServiceProj
{

    public interface IDatabase
    {
        Task<IEnumerable<Skill>> GetMultipleAsync(string query);
        Task<Skill> GetAsync(string id);
        Task AddAsync(Skill skill);
        Task UpdateAsync(string id, Skill skill);
        Task DeleteAsync(string id);
    }


    public class Database : IDatabase
    {
        private readonly SelfServiceSettings _config;
        private Container _container;

        public Database(SelfServiceSettings config)
        {
            // Initialise member variables
            _config = config;

            // Get CosmosDB Client
            var _client = new CosmosClient(_config.DbUrl);

            // Get CosmosDB Client
            _container = _client.GetContainer(_config.DbName, _config.ContainerName);
        }

        public async Task AddAsync(Skill skill)
        {
            await _container.CreateItemAsync(skill, new PartitionKey(skill.Id));
        }

        public async Task DeleteAsync(string id)
        {
            await _container.DeleteItemAsync<Skill>(id, new PartitionKey(id));
        }

        public async Task<Skill> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Skill>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }

        public async Task<IEnumerable<Skill>> GetMultipleAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Skill>(new QueryDefinition(queryString));

            var results = new List<Skill>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateAsync(string id, Skill skill)
        {
            await _container.UpsertItemAsync(skill, new PartitionKey(id));
        }
    }
}

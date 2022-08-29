using Microsoft.Azure.Cosmos;


namespace SelfServiceProj
{

    public interface IDatabase
    {
        Task<Action> GetAction(string id);
        Task<IEnumerable<Action>> GetAll();
    }


    public class Database : IDatabase
    {
        private readonly SelfServiceConfig _config;
        private Container _container;


        public Database(SelfServiceConfig config)
        {
            // Initialise member variables
            _config = config;

            // Connect to the database
            var _client = new CosmosClient(_config.DbUrl);
            _container = _client.GetContainer(_config.DbName, _config.ContainerName);
        }


        public async Task<Action> GetAction(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Action>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException) //For handling item not found and other exceptions
            {
                return null;
            }
        }


        public async Task<IEnumerable<Action>> GetAll()
        {
            var queryString = "SELECT * FROM SkillsDB1";
            var query = _container.GetItemQueryIterator<Action>(new QueryDefinition(queryString));

            var results = new List<Action>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}

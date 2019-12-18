using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Scheduler.Entities;
using System.Threading.Tasks;

namespace Scheduler.Services
{
    public interface IRepository<T>
    {
        Task CreateAsync(T entity);

        Task<T> GetAsync(int id);

        Task<WorkTask> UpdateAsync(WorkTask task);

        Task DeleteAsync(int id);
    }

    internal class TaskRepository : IRepository<WorkTask>
    {
        private readonly IConfiguration _configuration;

        public TaskRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task CreateAsync(WorkTask task)
        {
            var collection = GetCollection();

            await collection.InsertOneAsync(task);
        }

        public async Task<WorkTask> UpdateAsync(WorkTask task)
        {
            var collection = GetCollection();

            var jsonModel = task.ToJson();
            
            UpdateDefinition<WorkTask> update = "{ $set: " + jsonModel + " }";
            
            var result = await collection.UpdateOneAsync(t => t.Id == task.Id, update);
            
            return await collection.Find(t => t.Id == task.Id).FirstAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var collection = GetCollection();

            await collection.DeleteOneAsync(t => t.Id == id);
        }

        public async Task<WorkTask> GetAsync(int id)
        {
            var taskCollection = GetCollection();

            return await taskCollection.Find(t => t.Id == id).FirstAsync();
        }

        private IMongoCollection<WorkTask> GetCollection()
        {
            var connectionString = _configuration.GetConnectionString("MongoDBConnection");
            var mongoClient = new MongoClient(connectionString);
            var db = mongoClient.GetDatabase("test");
            return db.GetCollection<WorkTask>("tasks");
        }
    }
}

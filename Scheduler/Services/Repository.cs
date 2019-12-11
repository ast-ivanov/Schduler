﻿using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Scheduler.Services
{
    using WorkTask = Entities.WorkTask;

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
            
            //todo Доделать конвертирование DateTime
            UpdateDefinition<WorkTask> update = "{ $set: { description: \"" + task.Description + "\", createdDate: \"" + task.CreatedDate + "\" } }";
            
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

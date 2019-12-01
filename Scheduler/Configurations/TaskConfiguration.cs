using MongoDB.Bson.Serialization;
using Scheduler.Entities;

namespace Scheduler.Configurations
{
    public class TaskConfiguration
    {
        public static void Register()
        {
            BsonClassMap.RegisterClassMap<WorkTask>(cm =>
            {
                cm.MapMember(t => t.Description).SetElementName("description");
                cm.MapMember(t => t.CreatedDate).SetElementName("createdDate");
            });
        }
    }
}

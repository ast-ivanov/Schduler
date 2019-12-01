namespace Scheduler.Entities
{
    using System;

    public class WorkTask
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}

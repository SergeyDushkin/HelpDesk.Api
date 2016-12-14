using System;

namespace servicedesk.api
{
    public class Job
    {
        public Guid Id { get; set; }
        public string JobNumber { get; set; }
        public Guid SupplierId { get; set; }
        public Guid? UserId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? CompleteDate { get; set; }

        public Client Client { get; set; }
        public User User { get; set; }
    }

    public class JobCreated
    {
        public Guid SupplierId { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
    }
}

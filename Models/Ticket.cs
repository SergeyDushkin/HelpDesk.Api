using System;

namespace servicedesk.api
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string TicketNumber { get; set; }
        public string StoreName { get; set; }
        public string Comments { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public DateTimeOffset? CompleteDate { get; set; }
    }

    public class TicketCreated
    {
        public string Description { get; set; }
    }
}

using System;

namespace servicedesk.api
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string TicketNumber { get; set; }

        public Guid ClientId { get; set; }
        public Guid AddressId { get; set; }
        public Guid UserId { get; set; }
        
        public string Description { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public DateTimeOffset? CompleteDate { get; set; }

        public Client Client { get; set; }
        public Address Address { get; set; }
        public User User { get; set; }
    }

    public class TicketCreated
    {
        public Guid ClientId { get; set; }
        public Guid AddressId { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
    }
}

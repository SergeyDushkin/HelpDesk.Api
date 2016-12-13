using System;

namespace servicedesk.api
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Contact Contact { get; set; }
    }

    public class Contact 
    {
        public string Address { get; set; }
    }

    public class AddressCreated 
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}

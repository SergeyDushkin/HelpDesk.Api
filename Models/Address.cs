using System;

namespace servicedesk.api
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class AddressCreated 
    {
        public string Name { get; set; }
    }
}

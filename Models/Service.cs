using System;

namespace servicedesk.api
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    
    public class ServiceCreated 
    {
        public string Name { get; set; }
    }
}

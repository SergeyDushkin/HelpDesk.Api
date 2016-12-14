using System;

namespace servicedesk.api
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class SupplierCreated 
    {
        public string Name { get; set; }
    }
}

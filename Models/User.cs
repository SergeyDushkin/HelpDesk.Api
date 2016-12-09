using System;

namespace servicedesk.api
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UserCreated 
    {
        public string Name { get; set; }
    }
}

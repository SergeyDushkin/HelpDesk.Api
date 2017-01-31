using System;
using servicedesk.api.Storages;

namespace servicedesk.api
{
    public class ApplicationBuilder 
    {
        private object root { get; set; }

        public ApplicationBuilder UseRoot<T>(string name) 
        {
            return this;
        }

        public ApplicationBuilder UseCollection<T>(Action<ApplicationBuilderOptions> option = null) 
        {
            return this;
        }
        public ApplicationBuilder Use<T>(string name, ApplicationBuilderOptions option = null) 
        {
            return this;
        }
        public ApplicationBuilder UseElasticSearchCache() 
        {
            return this;
        }

        public void Build() 
        {

        }
    }

    public class ApplicationBuilderOptions
    {
        public ApplicationBuilderOptions SetName(string name) 
        {
            return this;
        }
        public ApplicationBuilderOptions UseCollection<T>(string name, ApplicationBuilderOptions option = null) 
        {
            return this;
        }
        public ApplicationBuilderOptions Use<T>(string name, ApplicationBuilderOptions option = null) 
        {
            return this;
        }
    }

    public class test 
    {
        public test() 
        {
            var builder = new ApplicationBuilder();

            builder
                .UseRoot<TicketStorage>("tickets")
                .UseCollection<AddressStorage>(option => 
                    option.SetName("addresses").UseCollection<ClientStorage>("clients"))
                .Use<ClientStorage>("client")
                .UseElasticSearchCache()
                .Build();
        }
    }
}

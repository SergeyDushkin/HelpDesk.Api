using servicedesk.Common.Security;

namespace servicedesk.api
{
    public class ApplicationServiceSettings<T> : ServiceSettings
    {
        public ServiceSettings Value => new ServiceSettings {
            CacheExpiry = base.CacheExpiry,
            Name = base.Name,
            Password = base.Password,
            RetryCount = base.RetryCount,
            RetryDelayMilliseconds = base.RetryDelayMilliseconds,
            Url = base.Url,
            Username = base.Username
        };
    }
}
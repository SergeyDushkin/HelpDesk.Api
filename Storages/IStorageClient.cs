using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Queries;
using Coolector.Common.Types;

namespace servicedesk.api.Storages
{
    public interface IStorageClient
    {
        Task<Maybe<T>> GetAsync<T>(string endpoint) where T : class;

        //Task<Maybe<PagedResult<T>>> GetCollectionAsync<T>(string endpoint) where T : class;
        Task<IEnumerable<T>> GetCollectionAsync<T>(string endpoint) where T : class;

        Task<Maybe<Stream>> GetStreamAsync(string endpoint);

        //Task<Maybe<PagedResult<TResult>>> GetFilteredCollectionAsync<TResult, TQuery>(TQuery query,
        //    string endpoint) where TResult : class where TQuery : class, IPagedQuery;
        Task<IEnumerable<TResult>> GetFilteredCollectionAsync<TResult, TQuery>(TQuery query,
            string endpoint) where TResult : class where TQuery : class, IPagedQuery;
    }
}

using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Domain.Core.Interfaces
{
    public interface IEventStoreRepository : IDisposable
    {
        Task Store(StoredEvent theEvent);
        IQueryable<StoredEvent> All();
        Task<List<StoredEvent>> GetEvents(string username, PagingViewModel paging);
        Task<int> Count(string username, string search);
    }
}
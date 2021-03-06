using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Domain.Interfaces;

namespace JPProject.Admin.Application.ViewModels
{
    public class PersistedGrantSearch : IQueryPaging, IQuerySort, IPersistedGrantCustomSearch
    {
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public string Sort { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace MohamedSprint1V2.DLL.ModelVM.Pagination
{
    public interface IPagedList
    {
        int PageIndex { get; }
        int TotalPages { get; }
        int TotalCount { get; }
        int PageSize { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }

    public class PagedList<T> : IPagedList
    {
        public List<T> Items { get; set; } = new List<T>();
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PagedList()
        {
        }

        public PagedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;
            PageSize = pageSize < 1 ? 10 : pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            if (TotalPages < 1) TotalPages = 1;
            if (PageIndex > TotalPages && count > 0) PageIndex = TotalPages;
            Items = items;
        }

        public static PagedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            source ??= new List<T>();
            var count = source.Count();
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (totalPages < 1) totalPages = 1;
            if (pageIndex > totalPages && count > 0) pageIndex = totalPages;

            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}

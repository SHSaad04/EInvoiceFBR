namespace EInvoice.Common.Pagination
{
    public static class Pagination
    {
        public static PagedResult<T> Paginate<T>(this List<T> query, int? page = null, int? pageSize = null) where T : class
        {
            PagedResult<T> pagedResult = new PagedResult<T>();
            if (!page.Validate() && !pageSize.Validate())
            {
                int num = query.Count();
                pagedResult.CurrentPage = ((num != 0) ? 1 : 0);
                pagedResult.PageSize = num;
                pagedResult.RowCount = query.Count();
                pagedResult.PageCount = ((num != 0) ? 1 : 0);
                pagedResult.Results = query.ToList();
                return pagedResult;
            }

            pagedResult.CurrentPage = page.Value;
            pagedResult.PageSize = pageSize.Value;
            pagedResult.RowCount = query.Count();
            pagedResult.PageCount = (int)Math.Ceiling(((double)pagedResult.RowCount / (double?)pageSize).Value);
            pagedResult.Results = query.Skip(((page - 1) * pageSize).Value).Take(pageSize.Value).ToList();
            return pagedResult;
        }

        public static PagedResult<T> Paginate<T>(this IQueryable<T> query, int? page = null, int? pageSize = null) where T : class
        {
            PagedResult<T> pagedResult = new PagedResult<T>();
            if (!page.Validate() && !pageSize.Validate())
            {
                int num = query.Count();
                pagedResult.CurrentPage = ((num != 0) ? 1 : 0);
                pagedResult.PageSize = num;
                pagedResult.RowCount = query.Count();
                pagedResult.PageCount = ((num != 0) ? 1 : 0);
                pagedResult.Results = query.ToList();
                return pagedResult;
            }

            pagedResult.CurrentPage = page.Value;
            pagedResult.PageSize = pageSize.Value;
            pagedResult.RowCount = query.Count();
            pagedResult.PageCount = (int)Math.Ceiling(((double)pagedResult.RowCount / (double?)pageSize).Value);
            pagedResult.Results = query.Skip(((page - 1) * pageSize).Value).Take(pageSize.Value).ToList();
            return pagedResult;
        }

        public static PagedResult<T> Paginate<T>(this IEnumerable<T> query, int? page = null, int? pageSize = null) where T : class
        {
            PagedResult<T> pagedResult = new PagedResult<T>();
            if (!page.Validate() && !pageSize.Validate())
            {
                int num = query.Count();
                pagedResult.CurrentPage = ((num != 0) ? 1 : 0);
                pagedResult.PageSize = num;
                pagedResult.RowCount = query.Count();
                pagedResult.PageCount = ((num != 0) ? 1 : 0);
                pagedResult.Results = query.ToList();
                return pagedResult;
            }

            pagedResult.CurrentPage = page.Value;
            pagedResult.PageSize = pageSize.Value;
            pagedResult.RowCount = query.Count();
            pagedResult.PageCount = (int)Math.Ceiling(((double)pagedResult.RowCount / (double?)pageSize).Value);
            pagedResult.Results = query.Skip(((page - 1) * pageSize).Value).Take(pageSize.Value).ToList();
            return pagedResult;
        }

        public static async Task<PagedResult<T>> PaginateAsync<T>(this IQueryable<T> query, int? page = null, int? pageSize = null) where T : class
        {
            PagedResult<T> results = null;
            await Task.Run(delegate
            {
                results = query.Paginate(page, pageSize);
            });
            return results;
        }

        public static PagedResult<T> Paginate<T>(this IOrderedEnumerable<T> query, int? page = null, int? pageSize = null) where T : class
        {
            PagedResult<T> pagedResult = new PagedResult<T>();
            if (!page.Validate() && !pageSize.Validate())
            {
                int num = query.Count();
                pagedResult.CurrentPage = ((num != 0) ? 1 : 0);
                pagedResult.PageSize = num;
                pagedResult.RowCount = query.Count();
                pagedResult.PageCount = ((num != 0) ? 1 : 0);
                pagedResult.Results = query.ToList();
                return pagedResult;
            }

            pagedResult.CurrentPage = page.Value;
            pagedResult.PageSize = pageSize.Value;
            pagedResult.RowCount = query.Count();
            pagedResult.PageCount = (int)Math.Ceiling(((double)pagedResult.RowCount / (double?)pageSize).Value);
            pagedResult.Results = query.Skip(((page - 1) * pageSize).Value).Take(pageSize.Value).ToList();
            return pagedResult;
        }
    }
}

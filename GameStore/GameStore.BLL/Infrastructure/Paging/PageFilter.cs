using GameStore.BLL.Interfaces;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Infrastructure.Paging
{
    internal class PageFilter<TEntity> : IOperation<TEntity> where TEntity : class
    {
        private readonly int _page;
        private readonly PageEnum _pageSize;

        public PageFilter(int page, PageEnum pageSize)
        {
            if (page < 1)
            {
                throw new ValidationException("Number of page isn't valid", "Number");
            }

            _page = page;
            _pageSize = pageSize;
        }

        public IQueryBuilder<TEntity> Execute(IQueryBuilder<TEntity> query)
        {
            int? itemsPerPage = null;
            if (_pageSize != PageEnum.All)
            {
                itemsPerPage = (int) _pageSize;
            }

            var pageIndex = _page == 0 ? 1 : _page;
            if (itemsPerPage != null)
            {
                query.Skip(itemsPerPage.Value*(pageIndex - 1));
                query.Take(itemsPerPage.Value);
            }

            return query;
        }
    }
}

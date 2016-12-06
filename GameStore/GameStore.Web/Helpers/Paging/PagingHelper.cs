using System.Web.Mvc;

namespace GameStore.Web.Helpers.Paging
{
    public static class PagingHelper
    {
        public static Pager Pager(this HtmlHelper htmlHelper, int pageIndex, int pageSize, int totalItems, string routePageKey)
        {
            return new Pager(htmlHelper, pageIndex, pageSize, totalItems, routePageKey);
        }
    }
}
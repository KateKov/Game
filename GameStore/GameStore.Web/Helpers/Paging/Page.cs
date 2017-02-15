using System.Web.Routing;

namespace GameStore.Web.Helpers.Paging
{
    public class Page
    {
       public Page()
       {
            RouteValues = new RouteValueDictionary();
       }

       public RouteValueDictionary RouteValues { get; set; }
    }

    public class PageBuilder
    {
        private readonly Page _page;
        public PageBuilder(Page pagerOptions)
        {
            _page = pagerOptions;
        }

        public PageBuilder AddRouteDataValue(RouteValueDictionary routeValues)
        {
            _page.RouteValues = routeValues;
            return this;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.Web.Helpers.Paging
{
    public class Pager : IHtmlString
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly int _pageIndex;
        private readonly int _pageSize;
        private readonly int _totalItemsCount;
        private readonly Page _page;
        private readonly string _routePageKey;

        public Pager(HtmlHelper htmlHelper, int pageIndex, int pageSize, int totalItemsCount, string routePageKey)
        {
            _htmlHelper = htmlHelper;
            _pageIndex = pageIndex;
            _pageSize = pageSize;
            _totalItemsCount = totalItemsCount;
            _routePageKey = routePageKey;
            _page = new Page();
        }

        public Pager Options(Action<PageBuilder> buildOptions)
        {
            buildOptions(new PageBuilder(_page));
            return this;
        }

        public virtual string ToHtmlString()
        {
            var stringBuilder = new StringBuilder();
            var pageCount = (int)Math.Ceiling(_totalItemsCount / (double)_pageSize);
            for (var i = 1; i <= pageCount; i++)
            {
                var page = BuildPageLink(i, i.ToString(), true, i == _pageIndex);
                stringBuilder.Append(page);
            }
          
            return stringBuilder.ToString();
        }
        private string BuildPageLink(int? pageIndex, string displayText = null, bool isActive = false, bool isCurrent = false)
        {
            var liBuilder = new TagBuilder("li");
            if (isCurrent)
            {
                liBuilder.MergeAttribute("class", "active");
            }
            if (!isActive)
            {
                liBuilder.MergeAttribute("class", "disabled");
            }
            var aBuilder = new TagBuilder("a");
            if (pageIndex.HasValue && isActive)
            {
                aBuilder.MergeAttribute("href", GeneratePageUrl((int)pageIndex));
            }
            aBuilder.SetInnerText(displayText);
            liBuilder.InnerHtml = aBuilder.ToString();
            return liBuilder.ToString();
        }
        private string GeneratePageUrl(int pageNumber)
        {
            var viewContext = _htmlHelper.ViewContext;
            var routeDataValues = viewContext.RequestContext.RouteData.Values;
            var pageLinkValueDictionary = new RouteValueDictionary(_page.RouteValues)
            {
                { _routePageKey, pageNumber }
            };
            if (!pageLinkValueDictionary.ContainsKey("controller") && routeDataValues.ContainsKey("controller"))
            {
                pageLinkValueDictionary.Add("controller", routeDataValues["controller"]);
            }
            if (!pageLinkValueDictionary.ContainsKey("action") && routeDataValues.ContainsKey("action"))
            {
                pageLinkValueDictionary.Add("action", routeDataValues["action"]);
            }
            var virtualPathForArea = RouteTable.Routes.GetVirtualPathForArea(viewContext.RequestContext, pageLinkValueDictionary);
            return virtualPathForArea == null ? null : virtualPathForArea.VirtualPath;
        }
    }
}
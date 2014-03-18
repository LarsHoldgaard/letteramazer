using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LetterAmazer.Websites.Client.Resources.Views.Shared;

namespace LetterAmazer.Websites.Client.Extensions
{
    public static class PaginatedExtensions
    {
        public static string Paginator(this HtmlHelper htmlHelper, int pageIndex, int pageSize, long totalItems)
        {
            return Paginator(htmlHelper, pageIndex, pageSize, totalItems, "page", null);
        }
        public static string Paginator(this HtmlHelper htmlHelper, int pageIndex, int pageSize, long totalItems, string pageParameterName)
        {
            return Paginator(htmlHelper, pageIndex, pageSize, totalItems, pageParameterName, null);
        }
        public static string Paginator(this HtmlHelper htmlHelper, int pageIndex, int pageSize, long totalItems, RouteValueDictionary parameters)
        {
            return Paginator(htmlHelper, pageIndex, pageSize, totalItems, "page", parameters);
        }
        public static string Paginator(this HtmlHelper htmlHelper, int pageIndex, int pageSize, long totalItems, string pageParameterName, RouteValueDictionary parameters)
        {
            if (totalItems == 0)
            {
                return "";
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("pageSize", 0, "PageSize must greater than 0");
            }

            if (totalItems < pageSize) return "";

            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            int totalPages = (int)totalItems / pageSize + (totalItems % pageSize > 0 ? 1 : 0);
            int startIndex = 0;
            int endIndex = totalPages;
            if (totalPages > 10)
            {
                startIndex = pageIndex - 5;
                endIndex = pageIndex + 5;
                if (startIndex < 0)
                {
                    startIndex = 0;
                    endIndex = startIndex + 10;
                }
                if (endIndex > totalPages)
                {
                    endIndex = totalPages;
                    startIndex = totalPages - 10;
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"pagination\">");
            RenderPaginatorLink(0, ViewRes.First, pageIndex, sb, urlHelper, pageParameterName, false, parameters);
            RenderPaginatorLink(pageIndex > 0 ? pageIndex - 1 : 0, ViewRes.Prev, pageIndex, sb, urlHelper, pageParameterName, false, parameters);
            for (int i = startIndex; i < endIndex; i++)
            {
                RenderPaginatorLink(i, (i + 1).ToString(), pageIndex, sb, urlHelper, pageParameterName, true, parameters);
            }
            RenderPaginatorLink(pageIndex + 1 < totalPages - 1 ? pageIndex + 1 : totalPages - 1, ViewRes.Next, pageIndex, sb, urlHelper, pageParameterName, false, parameters);
            RenderPaginatorLink(totalPages - 1, ViewRes.Last, pageIndex, sb, urlHelper, pageParameterName, false, parameters);

            sb.Append("</ul>");
            return sb.ToString();
        }

        private static void RenderPaginatorLink(int pageIndex, string pageText, int currentPageIndex, StringBuilder sb, UrlHelper urlHelper, string pageParameterName, bool addCurrent, RouteValueDictionary parameters)
        {
            if (pageIndex == currentPageIndex)
            {
                if (addCurrent) sb.AppendFormat("<li class=\"active\"><a href=\"#\">{0} <span class=\"sr-only\">(current)</a></li>", pageText);
                else sb.AppendFormat("<li class=\"disabled\"><a href=\"#\"><span>{0}</span></a></li>", pageText);
            }
            else
            {
                RouteValueDictionary routeValues = new RouteValueDictionary();

                urlHelper.RequestContext.RouteData.Values.ToList().ForEach(x => routeValues.Add(x.Key, x.Value));
                var queryString = urlHelper.RequestContext.HttpContext.Request.QueryString;
                foreach (string key in queryString.Keys)
                {
                    if (!routeValues.ContainsKey(key))
                    {
                        routeValues.Add(key, queryString[key]);
                    }
                }
                if (parameters != null)
                {
                    parameters.ToList().ForEach(x => routeValues[x.Key] = x.Value);
                }
                if (routeValues.ContainsKey(pageParameterName))
                {
                    routeValues[pageParameterName] = pageIndex + 1;
                }
                else
                {
                    routeValues.Add(pageParameterName, pageIndex + 1);
                }
                string url = urlHelper.RouteUrl(routeValues);
                sb.AppendFormat("<li><a href=\"{0}\"><span>{1}</span></a></li>", url, pageText);
            }
        }
    }
}
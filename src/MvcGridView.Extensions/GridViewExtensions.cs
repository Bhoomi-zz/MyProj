using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Security.Permissions;
using System.Linq.Expressions;
using System.Web.Routing;


namespace MvcGridView.Code
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class GridViewExtensions
    {
        public static string ActionImage(this HtmlHelper html, string action, string controller, object routeValues, string imageRelativeUrl, string alt)
        {
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            return string.Format("<a href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" border=\"0\"/></a>",
                urlHelper.Action(action, controller, routeValues),
                urlHelper.Content(imageRelativeUrl),
                alt);
        }

        public static void GridView<T>(
             this HtmlHelper html,
             GridViewData<T> data,
             Action<GridViewData<T>> headerTemplate,
             Action<T, string> itemTemplate,
             string cssClass,
             string cssAlternatingClass,
             Action<T> editItemTemplate,
             Action<GridViewData<T>> footerTemplate)
        {
            headerTemplate(data);

            int i = 0;
            foreach (var item in data.PagedList)
            {
                if (!item.Equals(data.EditItem))
                {
                    itemTemplate(item, (i % 2 == 0 ? cssClass : cssAlternatingClass));
                }
                else
                {
                    editItemTemplate(item);
                }

                i++;
            }

            footerTemplate(data);
        }
    }
}

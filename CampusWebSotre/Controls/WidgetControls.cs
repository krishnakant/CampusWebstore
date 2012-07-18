using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;


namespace CampusWebStore.Controls
{
    public static class WidgetControls
    {

        public static MvcHtmlString DisplayCatalogWidget(this HtmlHelper html)
        {

            //var objCatalog = new CatalogsHelper();

            //var models = objCatalog.GetCatalogList();

         return html.Partial("Widgets/CatalogsList");
        }

    }
}
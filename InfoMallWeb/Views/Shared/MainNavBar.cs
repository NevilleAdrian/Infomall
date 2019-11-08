using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoMallWeb.Views.Shared
{
    public static class MainNavBar
    {
        public static string ActivePageKey => "ActivePage";

        public static string Index => "Home";

        public static string AboutUs => "AboutUs";

        public static string Services => "Services";

        public static string Promotions => "Promotions";

        public static string Blog => "Blog";

        public static string Products => "Products";

        public static string ContactUs => "ContactUs";

        public static string Account => "Account";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string AboutUsNavClass(ViewContext viewContext) => PageNavClass(viewContext, AboutUs);

        public static string ServicesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Services);

        public static string PromotionsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Promotions);

        public static string BlogNavClass(ViewContext viewContext) => PageNavClass(viewContext, Blog);

        public static string ContactUsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ContactUs);

        public static string AccountNavClass(ViewContext viewContext) => PageNavClass(viewContext, Account);

        public static string ProductsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Products);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActiveMainPage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}

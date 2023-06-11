using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

namespace WebApp.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(name: Resources.UrlRoute.UrlFinancialRecords, template: "", defaults: new { controller = "FinancialRecords", action = "Index" });
            routes.MapRoute(name: Resources.UrlRoute.UrlFinancialReport, template: "", defaults: new { controller = "Reports", action = "Index" });
        }
    }
}

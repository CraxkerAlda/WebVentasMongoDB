using System.Web;
using System.Web.Mvc;
using WebVentasMongoDB.Filters;

namespace WebVentasMongoDB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionFilterAttribute());

        }

    }
}

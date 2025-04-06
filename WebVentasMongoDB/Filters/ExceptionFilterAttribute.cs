using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebVentasMongoDB.Filters
{
    public class ExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            // Captura la excepción
            Exception ex = filterContext.Exception;
            // Loguea la excepción en NLog
            Logger.Error(ex,
                "Ocurrió una excepción en el controlador: " +
                filterContext.RouteData.Values["controller"] +
                               " Acción: " +
                               filterContext.RouteData.Values["action"]);

            // Configurar la respuesta HTTP
            filterContext.Result = new ViewResult
            {
                ViewName = "Error" // Vista personalizada de error
            };

            filterContext.ExceptionHandled = true;
        }
    }
}
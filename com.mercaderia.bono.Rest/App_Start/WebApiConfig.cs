using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace com.mercaderia.bono.Rest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            if (config != null)
            {
                // Web API configuration and services

                var enableCorsAttribute = new EnableCorsAttribute("*",
                                          "Origin, Content-Type, Accept",
                                          "GET, PUT, POST, DELETE, OPTIONS");
                config.EnableCors(enableCorsAttribute);

                // Para forzar la respuesta Json
                config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));//text/html

                // Web API routes
                config.MapHttpAttributeRoutes();

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
            }
            else
            {
                throw new ArgumentNullException("config");
            }
        }
    }
}

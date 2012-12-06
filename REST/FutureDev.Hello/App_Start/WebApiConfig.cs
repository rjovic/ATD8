using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FutureDev.Hello.Formatters;
using FutureDev.Hello.MessageHandlers;

namespace FutureDev.Hello
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("ControllerExt", "api/{controller}.{ext}");
            config.Routes.MapHttpRoute("ControllerIdExt", "api/{controller}/{id}.{ext}");

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.MessageHandlers.Add(new UriFormatExtensionMessageHandler());
            config.Formatters.Add(new CsvFormatter());

        }
    }
}

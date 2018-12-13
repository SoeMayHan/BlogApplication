using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BlogApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "GetAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "BlogPosts", action = "GetBlogPosts", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "AddAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "BlogPosts", action = "AddBlogPost", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "GetSingleAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "BlogPosts", action = "GetBlogPost", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "UpdateAPI",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "BlogPosts", action = "UpdateBlogPost", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

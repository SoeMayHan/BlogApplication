using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using BlogApplication.Converter;

namespace BlogApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.MapHttpAttributeRoutes();
            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            formatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            formatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            formatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            formatter.SerializerSettings.ContractResolver = new SnakeToCamelCasePropertyResolver();
            config.Formatters.Insert(0, formatter);//default
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;

namespace BlogApplication.Converter
{
    public class JSContentNegotiator: IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter jsonFormatter;

        public JSContentNegotiator(JsonMediaTypeFormatter formatter)
        {
            this.jsonFormatter = formatter;
            this.jsonFormatter.SerializerSettings.ContractResolver = new SnakeToCamelCasePropertyResolver();
        }
        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            return new ContentNegotiationResult(jsonFormatter, new System.Net.Http.Headers.MediaTypeHeaderValue("applicaton/json"));
        }
    }
}
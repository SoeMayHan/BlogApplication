using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlogApplication.Common
{
    public interface BaseRestApiInterface
    {
        StatusType Status { get; set; }
        String Message { get; set; }

        IList Results { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusType { SUCCESS, FAILED };
}

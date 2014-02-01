using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting.Jsonp;

namespace AngularServices
{
    public class FormatterConfig
    {
        public static void RegisterFormatters(System.Net.Http.Formatting.MediaTypeFormatterCollection formatters)
        {
            var jsonFormatter = formatters.JsonFormatter;
            jsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            //// Insert the JSONP formatter in front of the standard JSON formatter.
            //var jsonpFormatter = new JsonpMediaTypeFormatter(formatters.JsonFormatter);
            //formatters.Insert(0, jsonpFormatter);
        }
    }
}
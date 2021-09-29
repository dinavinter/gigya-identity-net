using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Hosting;


namespace Gigya.Razor.Components
{
    [ViewComponent(Name = "GigyaLogin")]
    public class GigyaLoginViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string screenSet = "Default-RegistrationLogin",
            string startScreen = "gigya-login-screen", string onSubmitUrl = null)
        {
            
            return View("Default", new GigyaScreen(screenSet, startScreen, onSubmitUrl));
        }
    }

    public static class GigyaExtensions
    {
        public static IHtmlContent SdkScript(this ViewDataDictionary viewData) => new HtmlString(
            $"<script async src='https://cdns.{viewData["gigya.domain"]}/js/gigya.js?apikey={viewData["gigya.apiKey"]}'></script>");
 
        
        public static Request Request(
            this ViewDataDictionary viewData,
            string api )
        {
            if (viewData == null)
            {
                throw new ArgumentNullException(nameof(viewData));
            }
 
            var site = viewData.Site();
            return site.Request(new Request(api));

        }
        // public static string GigyaGetRequest(
        //     this IUrlHelper helper,
        //     string api = null,
        //     string @namespace = null,
        //     object values = null,
        //     string protocol = null,
        //     string host = null,
        //     string fragment = null)
        // {
        //     if (helper == null)
        //     {
        //         throw new ArgumentNullException(nameof(helper));
        //     }
        //
        //     var httpContext = helper.ActionContext.HttpContext;
        //
        //     protocol ??= httpContext.Request.Scheme;
        //
        //     var site = helper.ActionContext.Site();
        //     return site.Request(new Request(api), protocol).Uri().ToString();
        //    
        // }
        //
        public static GigyaSite Site(this ActionContext context) => new GigyaSite(context);
        public static GigyaSite Site(this ViewDataDictionary context) => new GigyaSite(context);

 
    }
    
           public record Request(string Api = "gs/ver.htm", Dictionary<string, string> Params = null,
            string Host = "accounts.gigya.com")
        {
            public Dictionary<string, string> Params { get; init; } = Params ?? new Dictionary<string, string>();
            public string Scheme = HttpScheme.Https.ToString();

            public (string name, string value) Param
            {
                init => Params[value.name] = value.value;
            }

            public HttpGetRequest Get() => new(this);

            // public IEnumerable<(string name, string value)> SetParams
            // {
            //     init { _ = value.ForEach(param => Params[param.name] = param.value); }
            // }
        };

         

        public record HttpGetRequest(Request Request) : Request(Request)
        {
            public Uri Uri()
            {
                return new UriBuilder()
                {
                    Scheme = Scheme,
                    Host = Host,
                    Path = Request.Api,
                    Query = Query()
                }.Uri;
            }

            private string Query() =>
                Request
                    .Params
                    .Select(tuple => $"{HttpUtility.UrlEncode(tuple.Key)}={HttpUtility.UrlEncode(tuple.Value)}")
                    .Aggregate("?", (query, param) => $"{query}{param}&")
                    .TrimEnd('&');
        };

        public class GigyaSite
        {
            public string ApiKey { get; set; }
            public string Domain { get; set; } = "gigya.com";

            public GigyaSite(ActionContext context)
            {
                if (context.ModelState.TryGetValue("gigya.domain", out var stateEntry) &&
                    stateEntry.RawValue is string domain)
                {
                    Domain = domain;
                }

                if (context.ModelState.TryGetValue("gigya.apiKey", out var stateApiKey) &&
                    stateApiKey.RawValue is string apiKey)
                {
                    ApiKey = "gigya.com";
                }
            }

            public GigyaSite(ViewDataDictionary viewDataDictionary)
            {
                ApiKey = viewDataDictionary["gigya.apiKey"] as string;
                Domain =  viewDataDictionary["gigya.domain"]as string;
            }
 
            public Request Request(Request request,
                string @namespace = "accounts",
                string host = null,
                string fragment = null)
            {
                return request with { Param = ("apiKey", ApiKey), Host = domain() } ;

                string domain() => host ?? $"{@namespace}.{Domain}";
            }
            public HttpGetRequest GetRequest(Request request,
                string @namespace = "accounts",
                string host = null,
                string fragment = null)
            {
                return new HttpGetRequest(request with { Param = ("apiKey", ApiKey), Host = domain() });

                string domain() => host ?? $"{@namespace}.{Domain}";
            }
        }
}
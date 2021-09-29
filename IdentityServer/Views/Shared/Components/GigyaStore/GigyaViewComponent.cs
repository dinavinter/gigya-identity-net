using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Gigya.Razor.Components
{
    public class GigyaStoreViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string apiKey, string domain)
        {
            return View("Default", new GigyaStore(apiKey , domain ));
        }
    }

    // public class GigyaStoreTag: TagHelper
    // {
    //     public readonly string ApiKey;
    //     public readonly string Domain;
    //     public readonly IEnumerable<ViewComponent> Children;
    //
    //     public GigyaStoreTag(string apiKey, string domain = "gigya.com")
    //     {
    //         ApiKey = apiKey;
    //         Domain = domain;
    //     }
    //
    //     public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    //     {
    //         output.TagName = "script";    // Replaces <email> with <a> tag
    //         // output.PreContent =
    //         //     $"<script async src='https://cdns.{Domain}/js/gigya.js?apikey={ApiKey}></script>";
    //         //
    //         //
    //         // var address = MailTo + "@" + EmailDomain;
    //         output.Attributes.SetAttribute("src",  $"<script async src='https://cdns.{Domain}/js/gigya.js?apikey={ApiKey}></script>");
    //         // output.Content.SetContent(   );
    //
    //         return base.ProcessAsync(context, output);
    //     }
    //
    //     public IHtmlContent SdkScript()
    //     {
    //         return new HtmlString($"<script async src='https://cdns.{Domain}/js/gigya.js?apikey={ApiKey}'></script>");
    //     }
    // }
    public class GigyaStore
    {
        public readonly string Content;
        public readonly string ApiKey;
        public readonly string Domain;
        public readonly IEnumerable<ViewComponent> Children;

        public GigyaStore(string apiKey, string domain = "gigya.com", string? content = null)
        {
            Content = content;
            ApiKey = apiKey;
            Domain = domain;
        }

        public IHtmlContent SdkScript()
        {
            return new HtmlString($"<script async src='https://cdns.{Domain}/js/gigya.js?apikey={ApiKey}'></script>");
        }
    }
}
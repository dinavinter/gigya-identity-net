using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Hosting;


namespace Gigya.Razor.Components
{
    [ViewComponent(Name = "JwtDebug")]
    public class JwtDebugViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View("Default" );
        }
    }

    
}
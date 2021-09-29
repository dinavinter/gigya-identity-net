using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace  Gigya.Razor.Components
{
    public class AuthorizationPage : PageModel
    {
        public string AuthorizeUrl { get; set; }
    }
 


}
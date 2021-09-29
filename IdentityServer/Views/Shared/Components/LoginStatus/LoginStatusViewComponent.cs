using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gigya.Razor.Components 
{
    public class LoginStatusViewComponent : ViewComponent
    {
        // private readonly SignInManager<GigyaUser> _signInManager;
        // private readonly UserManager<GigyaUser> _userManager;
        //
        // public LoginStatusViewComponent(SignInManager<GigyaUser> signInManager, UserManager<GigyaUser> userManager)
        // {
        //     _signInManager = signInManager;
        //     _userManager = userManager;
        // }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var authontication = await HttpContext.AuthenticateAsync();
            if (authontication.Succeeded)
            {
                return View("LoggedIn", authontication);
            }
            else
            {
                return View("Login");
            }
        }
    }
}
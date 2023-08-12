using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace  Gigya.Razor.Components
{
    [ViewComponent(Name = "GigyaScreen")]

    public class GigyaScreenViewComponent : ViewComponent
    {    
        public async Task<IViewComponentResult> InvokeAsync(string screenSet, string startScreen = null ,string onSubmitUrl = null)
        {
            return View("Default", new GigyaScreen(screenSet, startScreen, onSubmitUrl));
        }
    }
 

    public class GigyaScreen
    {
        public string ScreenSet;
        public string StartScreen;
        public string OnSubmitUrl;

        public GigyaScreen(string screenSet, string startScreen, string onSubmitUrl)
        {
            ScreenSet = screenSet;
            StartScreen = startScreen;
            OnSubmitUrl = onSubmitUrl;
        }

        public string ReturnUrl { get; set; }
    }
}
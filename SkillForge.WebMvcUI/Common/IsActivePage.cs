using Microsoft.AspNetCore.Mvc.Rendering;

namespace SkillForge.WebMvcUI
{
    public static class IsActivePage {
        public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action){
        
        controller = controller.ToLower();
        action = action.ToLower();
        var routeData = htmlHelper.ViewContext.RouteData;
        var routeAction = routeData.Values["action"]!.ToString();
        var routeController = routeData.Values["controller"]!.ToString();

        var returnActive = (controller == routeController && action == routeAction);

        return returnActive ? "active" : "";
        }
    }
}
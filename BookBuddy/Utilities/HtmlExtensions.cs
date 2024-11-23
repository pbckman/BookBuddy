using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookBuddy.Utilities
{
    public static class HtmlExtensions
    {
        public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action)
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var routeAction = (string)routeData.Values["action"];
            var routeController = (string)routeData.Values["controller"];
            bool isActive = action.Equals(routeAction, StringComparison.OrdinalIgnoreCase) &&
                            controller.Equals(routeController, StringComparison.OrdinalIgnoreCase);

            return isActive ? "active" : string.Empty;
        }
    }
}

using System.Web.Mvc;

namespace VCMS.Lib.Common
{
    public class AjaxChildActionOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            var isAjaxRequest = controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
            var isChildAction = controllerContext.IsChildAction;
            var result = isAjaxRequest || isChildAction;
            return result;
        }
    }
}

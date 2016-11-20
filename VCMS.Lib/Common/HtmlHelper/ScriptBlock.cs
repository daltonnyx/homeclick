using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Routing;
using System.Web.WebPages;

namespace System.Web.Mvc
{
    public static partial class HtmlHelpers
    {
        /// <summary>
        ///  This section is for adding a script to the <head> tag from a partial view (or any other place)
        /// </summary>
        private const string SCRIPTBLOCK_BUILDER = "_ScriptBlockBuilder_";

        public static MvcHtmlString Script(this HtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            htmlHelper.ViewContext.HttpContext.Items[SCRIPTBLOCK_BUILDER + Guid.NewGuid()] = template;
            return MvcHtmlString.Empty;
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith(SCRIPTBLOCK_BUILDER))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null)
                    {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                }
            }
            return MvcHtmlString.Empty;
        }
    }
}
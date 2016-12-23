using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Routing;
using System.Web.WebPages;

namespace System.Web.Mvc
{
    public class CustomSelectItem : SelectListItem
    {
        public object HtmlAttributes { get; set; }
    }

    public static partial class HtmlHelpers
    {

        public static string RandomString(this HtmlHelper htmlHelper, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static MvcHtmlString ImageFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var imgUrl = expression.Compile()(htmlHelper.ViewData.Model);
            return BuildImageTag(imgUrl.ToString(), null);
        }
        public static MvcHtmlString ImageFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {
            var imgUrl = expression.Compile()(htmlHelper.ViewData.Model);
            return BuildImageTag(imgUrl.ToString(), htmlAttributes);
        }

        private static MvcHtmlString BuildImageTag(string imgUrl, object htmlAttributes)
        {
            TagBuilder tag = new TagBuilder("img");

            tag.Attributes.Add("src", imgUrl);
            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString CustomDropdownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<CustomSelectItem> list, string optionLabel = "", object htmlAttributes = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText((LambdaExpression)expression);
            var value = (metadata.Model != null) ? metadata.Model : string.Empty;
            return CustomDropdownList(htmlHelper, metadata, name, optionLabel, list, value.ToString(), HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString CustomDropdownList<TModel>(this HtmlHelper<TModel> htmlHelper, string name, IEnumerable<CustomSelectItem> list, string selectedValue = "", string optionLabel = "", object htmlAttributes = null)
        {
            return CustomDropdownList(htmlHelper, null, name, optionLabel, list, selectedValue, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        private static MvcHtmlString CustomDropdownList(this HtmlHelper htmlHelper, ModelMetadata metadata, string name, string optionLabel, IEnumerable<CustomSelectItem> list, string selectedValue, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder dropdown = new TagBuilder("select");

            if (name != string.Empty)
            {
                string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
                if (String.IsNullOrEmpty(fullName))
                {
                    throw new ArgumentException("name");
                }

                dropdown.Attributes.Add("id", fullName.Replace('[', '_').Replace(']', '_'));
                dropdown.Attributes.Add("name", fullName);
                dropdown.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));
            }
            //dropdown.MergeAttribute("data-val", "true");
            //dropdown.MergeAttribute("data-val-required", "Mandatory field.");
            //dropdown.MergeAttribute("data-val-number", "The field must be a number.");
            dropdown.MergeAttributes(htmlAttributes); //dropdown.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            //option label
            if (optionLabel != null)
            {
                var option = new TagBuilder("option");
                option.SetInnerText(optionLabel);
                option.MergeAttribute("value", string.Empty);
                dropdown.InnerHtml += option.ToString();
            }

            foreach (var item in list)
            {
                var optionHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(item.HtmlAttributes);
                var option = new TagBuilder("option");
                if (selectedValue == item.Value)
                    option.Attributes.Add("selected", "selected");
                option.Attributes.Add("value", item.Value);
                option.MergeAttributes(optionHtmlAttributes);
                option.SetInnerText(item.Text);
                dropdown.InnerHtml += option.ToString();
            }
            return MvcHtmlString.Create(dropdown.ToString(TagRenderMode.Normal));
        }

        //Controller name as string
        public static string GetControllerName(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
                return (string)routeValues["controller"];

            return string.Empty;
        }
        //Action name as string
        public static string GetActionName(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("action"))
                return (string)routeValues["action"];

            return string.Empty;
        }
    }
}
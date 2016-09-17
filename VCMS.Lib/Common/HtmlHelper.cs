using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public class CustomSelectItem : SelectListItem
    {
        public object HtmlAttributes { get; set; }
    }

    public static class HtmlHelpers
    {
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

        public static MvcHtmlString CustomDropdownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<CustomSelectItem> list, string selectedValue = "", string optionLabel = "", object htmlAttributes = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText((LambdaExpression)expression);
            return CustomDropdownList(htmlHelper, metadata, name, optionLabel, list, selectedValue, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        private static MvcHtmlString CustomDropdownList(this HtmlHelper htmlHelper, ModelMetadata metadata, string name, string optionLabel, IEnumerable<CustomSelectItem> list, string selectedValue, IDictionary<string, object> htmlAttributes)
        {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("name");
            }

            TagBuilder dropdown = new TagBuilder("select");
            dropdown.Attributes.Add("name", fullName);
            //dropdown.MergeAttribute("data-val", "true");
            //dropdown.MergeAttribute("data-val-required", "Mandatory field.");
            //dropdown.MergeAttribute("data-val-number", "The field must be a number.");
            dropdown.MergeAttributes(htmlAttributes); //dropdown.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            dropdown.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));

            foreach (var item in list)
            {
                var optionHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(item.HtmlAttributes);
                TagBuilder option = new TagBuilder("option");
                option.Attributes.Add("value", item.Value);
                option.MergeAttributes(optionHtmlAttributes);
                option.SetInnerText(item.Text);
                dropdown.InnerHtml += option.ToString();
            }
            return MvcHtmlString.Create(dropdown.ToString(TagRenderMode.Normal));
        }

    }
}
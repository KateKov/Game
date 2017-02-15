using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace GameStore.Web.Helpers
{
    public static class EnumDropDownListHelper
    {
        private static readonly SelectListItem[] SingleEmptyItem = { new SelectListItem { Text = string.Empty, Value = string.Empty } };

        public static MvcHtmlString EnumDropDownList<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes) where TEnum : struct
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = GetNonNullableModelType(metadata);
            var values = Enum.GetValues(enumType).Cast<TEnum>();
            var items = values.Select(value => new SelectListItem
            {
                Text = GetDisplayName(value),
                Value = value.ToString(),
                Selected = value.Equals(metadata.Model)
            });
            if (metadata.IsNullableValueType)
            {
                items = SingleEmptyItem.Concat(items);
            }

            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }

        public static string GetDisplayName<TEnum>(TEnum enumValue)
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = fi.GetCustomAttributes(typeof(DisplayAttribute), false);
            var attribute = (DisplayAttribute)attributes[0];
            return attribute.GetName();
        }

        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            var realModelType = modelMetadata.ModelType;
            var underlyingType = Nullable.GetUnderlyingType(realModelType);
            return underlyingType ?? realModelType;
        }       
    }
}
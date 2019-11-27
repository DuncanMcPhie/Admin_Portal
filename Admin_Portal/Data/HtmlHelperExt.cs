using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Admin_Portal.Data
{
    public static class HtmlHelperExt
    {
        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, TEnum selectedValue, object htmlAttributes)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().OrderBy(e => e.ToString());
            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem()
                                                {
                                                    Text = value.ToString(),
                                                    Value = ((int)Enum.Parse(typeof(TEnum), value.ToString())).ToString(),
                                                    Selected = (value.Equals(selectedValue))
                                                };

            return SelectExtensions.DropDownListFor(htmlHelper, expression, items, htmlAttributes);
        }
    }
}
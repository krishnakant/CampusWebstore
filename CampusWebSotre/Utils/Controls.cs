namespace CampusWebStore.Utils
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public static class Controls
    {
        #region Public Methods

        public static string RadioButtonList(this HtmlHelper helper, string name, IEnumerable<string> items)
        {
            SelectList selectList = new SelectList(items);
            return helper.RadioButtonList(name, selectList);
        }

        public static string RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {
            TagBuilder tableTag = new TagBuilder("table");
            tableTag.AddCssClass("radio-main");
            TagBuilder trTag = new TagBuilder("tr");
            foreach (SelectListItem item in items)
            {
                TagBuilder tdTag = new TagBuilder("td");

                string rbValue = item.Value ?? item.Text;

                string rbName = name + "_" + rbValue;

                // var radioTag = helper.RadioButton(rbName, rbValue, item.Selected, new { name = name });
                TagBuilder radioTag = new TagBuilder("input");
                radioTag.MergeAttribute("type", "radio");
                radioTag.MergeAttribute("id", rbName);
                radioTag.MergeAttribute("value", rbValue);
                if (item.Selected)
                {
                    radioTag.MergeAttribute("Checked", "Checked");
                }

                TagBuilder labelTag = new TagBuilder("label");
                labelTag.MergeAttribute("for", rbName);
                labelTag.MergeAttribute("id", rbName + "_label");
                labelTag.InnerHtml = rbValue;
                tdTag.InnerHtml = radioTag + labelTag.ToString();
                trTag.InnerHtml += tdTag.ToString();
            }

            tableTag.InnerHtml = trTag.ToString();

            return tableTag.ToString();
        }

        #endregion
    }
}
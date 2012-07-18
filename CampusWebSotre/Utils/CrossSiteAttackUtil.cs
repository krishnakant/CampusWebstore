namespace CampusWebStore.Utils
{
    using System.Collections.Specialized;
    using System.Text.RegularExpressions;

    public static class CrossSiteAttackUtil
    {
        #region Constants and Fields

        public const string AcceptableTags = "strong|p|em|span|ul|li|ol";
        public const string AcceptableTagsForTinyMce = "strong,p,em,span,ul,li,ol";

        private const string StripAttributes = " ?[\\w:\\-]+ ?= ?(\"[^\"]+\"|'[^']+'|\\w+)";

        private const string StripTagsPattern = "</?\\w+((\\s+\\w+(\\s*=\\s*(?:\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)/?>";

        private const string WhiteListPattern = "</?(?(?=" + AcceptableTags + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:([""']?).*?\1?)?)*\s*/?>";

        private static readonly Regex RegexBetweenTags = new Regex(@">(?! )\s+", RegexOptions.Compiled);

        private static readonly Regex RegexLineBreaks = new Regex(@"([\n\s])+?(?<= {2,})<", RegexOptions.Compiled);

        #endregion

        #region Public Methods

        public static string CleanHtml(string html)
        {
            return FilterHtml(html, WhiteListPattern);
        }

        public static string CleanHtmlForWords(string content)
        {
            if (content == null)
            {
                return null;
            }

            if ("null".Equals(content))
            {
                return null;
            }

            content = content
                .Replace("<script", "[script")
                .Replace("</script>", "[/script]")
                .Replace("<style", "[style")
                .Replace("</style>", "[/style]")
                .Replace("\\r", string.Empty)
                .Replace("\\t", string.Empty)
                ;
            if (content.EndsWith("<br>"))
            {
                content = content.Substring(0, content.Length - 4);
            }

            // content = RemoveWhitespaceFromHtml(content);
            // content = System.Web.HttpUtility.HtmlEncode(content);
            return content;
        }

        public static string RemoveTags(string html)
        {
            return Regex.Replace(html, StripTagsPattern, string.Empty, RegexOptions.Compiled);
        }

        public static string RemoveWhitespaceFromHtml(string html)
        {
            html = RegexBetweenTags.Replace(html, ">");
            html = RegexLineBreaks.Replace(html, "<");
            return html.Trim();
        }

        #endregion

        #region Methods

        private static string CleanWordHtml(string html)
        {
            StringCollection sc = new StringCollection();

            // get rid of unnecessary tag spans (comments and title)
            sc.Add(@"<!--(\w|\W)+?-->");
            sc.Add(@"<title>(\w|\W)+?</title>");

            // Get rid of classes and styles
            sc.Add(@"\s?class=\w+");
            sc.Add(@"\s+style='[^']+'");

            // Get rid of unnecessary tags
            sc.Add(
                @"<(meta|link|/?o:|/?style|/?div|/?st\d|/?head|/?html|body|/?body|/?span|!\[)[^>]*?>");

            // Get rid of empty paragraph tags
            sc.Add(@"(<[^>]+>)+&nbsp;(</\w+>)+");

            // remove bizarre v: element attached to <img> tag
            sc.Add(@"\s+v:\w+=""[^""]+""");

            // remove extra lines
            sc.Add(@"(\n\r){2,}");
            foreach (string s in sc)
            {
                html = Regex.Replace(html, s, string.Empty, RegexOptions.IgnoreCase);
            }

            return html;
        }

        /// <summary>
        /// Cleans all manner of evils from the rich text editors in IE, Firefox, Word, and Excel
        /// Only returns acceptable HTML, and converts line breaks to <br />
        /// Acceptable HTML includes HTML-encoded entities.
        /// </summary>
        private static string FilterHtml(string html, string pattern)
        {
            if(string.IsNullOrEmpty(html))
            {
                return null;
            }
            html = html.Replace("&" + "nbsp;", " ").Trim(); // concat here due to SO formatting

            // Does this have HTML tags?

            if (html.IndexOf("<") >= 0)
            {
                // Make all tags lowercase
                html = Regex.Replace(html, "<[^>]+>", m => m.ToString().ToLower());

                // Filter out anything except allowed tags
                // Problem: this strips attributes, including href from a http://stackoverflow.com/questions/307013/how-do-i-filter-all-html-tags-except-a-certain-whitelist
                html = Regex.Replace(html, pattern, string.Empty, RegexOptions.Compiled);
                // Make all BR/br tags look the same, and trim them of whitespace before/after
                html = Regex.Replace(html, @"\s*<br[^>]*>\s*", "<br />", RegexOptions.Compiled);
            }

            // html = html.Replace(StripAttributes, ">");
            html = Regex.Replace(html, StripAttributes, string.Empty);

            // No CRs
            html = html.Replace("\r", string.Empty);

            // Convert remaining LFs to line breaks
            // html = html.Replace("\n", "<br />");
            html = html.Replace("\n", string.Empty);

            // Trim BRs at the end of any string, and spaces on either side
            html = CleanWordHtml(html);
            string filterHtml = Regex.Replace(html, "(<br />)+$", string.Empty, RegexOptions.Compiled).Trim();
            return filterHtml;
        }

        #endregion
    }
}
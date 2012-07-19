namespace CampusWebSotre.Utils
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;



    public static class Captcha
    {
        #region Public Methods

        public static string CaptchaImage(this HtmlHelper helper, int height, int width, string url)
        {
            CaptchaImage image = CacheCaptchaImage(height, width, url);

            string captchaUrl = url + "?guid=" + image.UniqueId;

            StringBuilder stringBuilder = new StringBuilder(256);
            stringBuilder.Append("<input type=\"hidden\" name=\"captcha-guid\" value=\"\"");
            stringBuilder.Append(image.UniqueId);
            stringBuilder.Append("\" />");
            stringBuilder.AppendLine();
            stringBuilder.Append("<img src=\"");

            stringBuilder.Append(captchaUrl);
            stringBuilder.Append("\" alt=\"CAPTCHA\" width=\"");
            stringBuilder.Append(width);
            stringBuilder.Append("\" height=\"");
            stringBuilder.Append(height);
            stringBuilder.Append("\" />");

            return stringBuilder.ToString();
        }

        public static string CaptchaImageUrl(this UrlHelper helper, int height, int width, string url)
        {
            CaptchaImage image = CacheCaptchaImage(height, width, url);
            return url + "?guid=" + image.UniqueId;
        }

        #endregion

        #region Methods

        private static CaptchaImage CacheCaptchaImage(int height, int width, string url)
        {
            CaptchaImage image = new CaptchaImage
                {
                    Height = height, 
                    Width = width, 
                };
            HttpRuntime.Cache.Add(image.UniqueId, image, null, DateTime.Now.AddSeconds(Utils.CaptchaImage.CacheTimeOut), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            return image;
        }

        #endregion
    }
}
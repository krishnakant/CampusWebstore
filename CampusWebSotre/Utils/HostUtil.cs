namespace CampusWebSotre.Utils
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    public interface IHostUtil
    {
        #region Public Methods

        string GetHostName(HttpRequestBase request, HttpResponseBase httpResponse);

        /// <summary>
        /// Returns the Host. Example: http://redeeze.com
        /// </summary>
        string GetHostUrl(HttpRequestBase request);

        string GetSiteMailUrl(HttpRequestBase request, UrlHelper url);

        #endregion
    }

    public class HostUtil : IHostUtil
    {
        #region Public Methods

        public static string HostUrl(HttpRequestBase request)
        {
            string url = request.IsSecureConnection ? "https://" : "http://";

            // ReSharper disable PossibleNullReferenceException
            url += request.Url.Host;

            // ReSharper restore PossibleNullReferenceException
            url += ":" + request.Url.Port;
            return url;
        }

        public static string HostUrl(HttpRequest request)
        {
            string url = request.IsSecureConnection ? "https://" : "http://";

            // ReSharper disable PossibleNullReferenceException
            url += request.Url.Host;

            // ReSharper restore PossibleNullReferenceException
            url += ":" + request.Url.Port;
            return url;
        }

        #endregion

        #region Implemented Interfaces

        #region IHostUtil

        public string GetHostName(HttpRequestBase request, HttpResponseBase httpResponse)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (httpResponse == null)
            {
                throw new ArgumentNullException("httpResponse");
            }

            if (request.Url == null)
            {
                throw new ArgumentException("Request Url");
            }

            return request.Url.Host;
        }

        public string GetHostUrl(HttpRequestBase request)
        {
            if (request.Url == null)
            {
                return string.Empty;
            }

            string url = string.Empty;
            url += request.IsSecureConnection ? "https://" : "http://";

            url += request.Url.Host;

            url += ":" + request.Url.Port;

            return url;
        }

        public string GetSiteMailUrl(HttpRequestBase request, UrlHelper url)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            return GetHostUrl(request) + url.Action("Thread", "Messages", new { area = string.Empty });
        }

        #endregion

        #endregion

    }
}
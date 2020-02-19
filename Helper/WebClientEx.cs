using System;
using System.Net;

namespace HomeTheater.Helper
{
    public class WebClientEx : WebClient
    {
        public WebClientEx(CookieContainer container)
        {
            CookieContainer = container;
        }

        public CookieContainer CookieContainer { get; set; } = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            var r = base.GetWebRequest(address);
            if (r is HttpWebRequest request) request.CookieContainer = CookieContainer;
            return r;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            var response = base.GetWebResponse(request, result);
            ReadCookies(response);
            return response;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var response = base.GetWebResponse(request);
            ReadCookies(response);
            return response;
        }

        private void ReadCookies(WebResponse r)
        {
            if (r is HttpWebResponse response)
            {
                var cookies = response.Cookies;
                CookieContainer.Add(cookies);
            }
        }
    }
}
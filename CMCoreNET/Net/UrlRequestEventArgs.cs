using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace CMCoreNET.Net
{
    /// <summary>
    /// UrlRequestEventArgs with the Response from a request.
    /// </summary>
    public class UrlRequestEventArgs : EventArgs
    {
        public System.IO.Stream ResponseStream { get; internal set; }
        public WebHeaderCollection Headers { get; internal set; }
        public CookieCollection Cookies { get; internal set; }
        public int ResponseTime { get; internal set; }
        public string ContentType { get; internal set; }
        public long ContentLength { get; internal set; }
        public HttpStatusCode StatusCode { get; internal set; }
    }
}

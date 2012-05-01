using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
namespace CMCoreNET.Net
{
    public enum UrlRequestMethod
    {
        POST,
        GET,
        PUT,
        OPTION,
        HEAD
    }

    /// <summary>
    /// UrlRequest
    /// Request a stream to a url on the internet or network.
    /// </summary>
    public class UrlRequest
    {
        #region Fields

        CookieCollection cookies;
        Dictionary<string, string> headers;

        #endregion

        #region Properties

        
        public string Url { get; set; }
        public UrlRequestMethod? RequestMethod { get; set; }
        public int Timeout { get; set; }
        public byte[] Data { get; set; }
        public bool AllowAutoRedirect { get; set; }
        
        public string UserAgent 
        {
            get 
            {
                return Headers["User-Agent"];
            }

            set
            {
                Headers["User-Agent"] = value;
            }
        }

        public IDictionary<string, string> Headers
        {
            get 
            {
                if (headers == null)
                {
                    headers = new Dictionary<string, string>();
                }

                return headers;
            }
        }

        public CookieCollection Cookies 
        {
            get 
            {
                if (cookies == null)
                {
                    cookies = new CookieCollection();
                }
                return cookies;
            }
        }

        public int ContentLenght
        {
            get
            { 
                int lenght = 0;
                if (Data != null)
                {
                    lenght = Data.Length;
                }
                return lenght;
            }
        }

        #endregion

        #region Constructors

        public UrlRequest()
        { 
            
        }

        public UrlRequest(string url) 
        {
            Url = url;
            AllowAutoRedirect = true;
        }

        #endregion

        #region Public Methods

        public HttpWebResponse Load()
        {
            if (string.IsNullOrEmpty(Url))
            {
                throw new InvalidOperationException("Url is required");
            }
            return Load(Url, RequestMethod, Data);
        }

        public HttpWebResponse Load(string url) 
        {
            return Load(url, RequestMethod, Data);
        }

        public HttpWebResponse Load(string url, UrlRequestMethod? method, byte[] data) 
        {
            Url = url;
            RequestMethod = method;
            Data = data;

            HttpWebRequest request = BuildRequest();
            
            if (Data != null && Data.Length > 0)
            {
                SendRequestData(request);
            }

            return GetWebResponse(request);
        }

        public void AcceptSSLValidationDelegate(RemoteCertificateValidationCallback validationDelegate)
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(validationDelegate);
        }

        #endregion

        #region Private Methods

        HttpWebRequest BuildRequest() 
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(Url);
            var visitor = new HeaderVisitor();
            visitor.Visit(this);

            if (visitor.HasHeaders)
            {
                foreach (var header in visitor.Headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            if (!string.IsNullOrEmpty(visitor.ContentType))
            {
                request.ContentType = visitor.ContentType;
            }

            request.Method = GetRequestMethod();

            if (Timeout != 0)
            {
                request.Timeout = Timeout;
            }

            request.AllowAutoRedirect = AllowAutoRedirect;

            return request;
        }

        string GetRequestMethod()
        {
            if (!RequestMethod.HasValue)
            {
                return UrlRequestMethod.GET.ToString();
            }

            return RequestMethod.ToString();
        }

        void SendRequestData(HttpWebRequest request) 
        {
            request.ContentLength = Data.Length;

            Stream streamToWrite = request.GetRequestStream();
            streamToWrite.Write(Data, 0, Data.Length);
            streamToWrite.Close();
        }

        HttpWebResponse GetWebResponse(HttpWebRequest request)
        {
            return request.GetResponse() as HttpWebResponse;
        }

        #endregion

        #region Inner Class

        class HeaderVisitor
        {
            public Dictionary<string, string> Headers
            {
                get;
                private set;
            }

            public string ContentType { get; private set; }
            public bool HasHeaders { get; private set; }

            public void Visit(UrlRequest request)
            {
                if (request.Headers != null && request.Headers.Any())
                {
                    HasHeaders = true;
                    Headers = new Dictionary<string, string>();
                    var itemsToRemove = new List<string>();

                    foreach (var header in request.Headers)
                    {
                        if (header.Key.ToLower() == "content-type")
                        {
                            ContentType = header.Value;
                            itemsToRemove.Add(header.Key);
                        }
                        Headers.Add(header.Key, header.Value);
                    }

                    itemsToRemove.ForEach(i => Headers.Remove(i));

                    if (Headers.Count() == 0)
                    {
                        HasHeaders = false;
                    }
                }
            }
        }

        #endregion
    }
}


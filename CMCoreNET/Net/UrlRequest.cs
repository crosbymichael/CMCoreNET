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
    
    /// <summary>
    /// UrlRequest
    /// Request a stream to a url on the internet or network.
    /// </summary>
    public class UrlRequest
    {
        public enum UrlRequestMethod
        {
            POST,
            GET,
            PUT,
            OPTION,
            HEAD
        }

        #region Fields

        private UrlRequestMethod? method;
        private byte[] data;
        private int timeout = 0;
        private HttpWebRequest request;
        private HttpWebResponse response;
        private CookieCollection cookies;

        #endregion

        #region Properties

        public string ContentType { get; set; }
        public string Url { get; set; }

        public UrlRequestMethod? RequestMethod
        {
            get { return this.method; }
            set { this.method = value; }
        }

        public int Timeout 
        {
            get { return this.timeout; }
            set { this.timeout = value; }
        }

        public WebHeaderCollection Headers
        {
            get 
            {
                return this.request.Headers;
            }
        }

        public CookieCollection Cookies 
        {
            get 
            {
                if (this.cookies == null) this.cookies = new CookieCollection();
                return this.cookies;
            }
        }

        public byte[] Data 
        {
            get { return this.data; } 
            set { this.data = value; }
        }

        #endregion

        #region Constructors

        public UrlRequest()
        { 
            
        }

        public UrlRequest(string url) 
        {
            this.Url = url;
            CreateRequest();
        }

        #endregion

        #region Public Methods

        public HttpWebResponse Load()
        {
            return Load(this.Url, this.RequestMethod, this.data);
        }

        public HttpWebResponse Load(string Url) 
        {
            return Load(Url, UrlRequestMethod.GET, this.data);
        }

        public HttpWebResponse Load(
            string Url, 
            UrlRequestMethod? method,
            byte[] data) {

            this.Url = Url;
            this.RequestMethod = method;
            this.data = data;

            SetupRequest();

            if (this.data != null)
            {
                SendRequestData();
            }
            GetResponse();
            return this.response;
        }

        public void AcceptSSLValidationDelegate(
            RemoteCertificateValidationCallback validationDelegate)
        {
            ServicePointManager.ServerCertificateValidationCallback += 
                new RemoteCertificateValidationCallback(validationDelegate);
            this.request.AllowAutoRedirect = true;
        }

        #endregion

        #region Private Methods

        private void CreateRequest() 
        {
            request = (HttpWebRequest)HttpWebRequest.Create(this.Url);
        }

        private void SetupContentType() 
        {
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                request.ContentType = this.ContentType;
            }
        }

        private void SetRequestMethod() 
        {
            request.Method = (this.RequestMethod == null) ?
                UrlRequestMethod.GET.ToString() :
                this.RequestMethod.ToString();
        }

        private void SetupTimeout() 
        {
            if (this.timeout != 0)
                request.Timeout = timeout;
        }

        private void SetupRequest() 
        {
            if (request == null)
            {
                CreateRequest();
            }
            SetRequestMethod();
            SetupContentType();
            SetupTimeout();
        }

        private void SendRequestData() 
        {
            request.ContentLength = this.data.Length;
            Stream streamToWrite = request.GetRequestStream();
            streamToWrite.Write(this.data, 0, this.data.Length);
            streamToWrite.Close();
        }

        private void GetResponse()
        {
            response = this.request.GetResponse() as HttpWebResponse;
        }

        #endregion
    }
}


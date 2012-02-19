using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
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

        private UrlRequestMethod? method;
        private byte[] data;
        private int timeout = 0;
        private WebRequest request;
        private HttpWebResponse response;

        #endregion

        #region Properties

        public string ContentType { get; set; }
        public string Url { get; set; }

        public UrlRequestMethod? RequestMethod {
            get { return this.method; }
            set { this.method = value; }
        }

        public int Timeout {
            get { return this.timeout; }
            set { this.timeout = value; }
        }

        public WebHeaderCollection Headers { get; set; }
        public CookieCollection Cookies { get; set; }

        #endregion

        #region Constructors

        public UrlRequest() {
            
        }

        public UrlRequest(string url) {
            this.Url = url;
        }

        #endregion

        #region Public Methods

        public HttpWebResponse Load(string Url) {
            return Load(Url, UrlRequestMethod.GET, null);
        }

        public HttpWebResponse Load(
            string Url, 
            UrlRequestMethod? method,
            byte[] data) {

            this.Url = Url;
            this.RequestMethod = method;
            this.data = data;

            SetupRequest();

            if (data != null)
                SendRequestData();
            GetResponse();
            return this.response;
        }

        #endregion


        #region Private Methods

        private void CreateRequest() {
            request = WebRequest.Create(this.Url);
        }

        private void SetupHeaders() {
            if (this.Headers != null && this.Headers.Count > 0)
                request.Headers = this.Headers;
        }

        private void SetupContentType() {
            if (!string.IsNullOrEmpty(this.ContentType))
                request.ContentType = this.ContentType;
        }

        private void SetRequestMethod() {
            request.Method = (this.RequestMethod == null) ?
                UrlRequestMethod.GET.ToString() :
                this.RequestMethod.ToString();
        }

        private void SetupTimeout() {
            if (this.timeout != 0)
                request.Timeout = timeout;
        }

        private void SetupRequest() {
            CreateRequest();
            SetRequestMethod();
            SetupHeaders();
            SetupContentType();
            SetupTimeout();
        }

        private void SendRequestData() {
            request.ContentLength = this.data.Length;
            Stream streamToWrite = request.GetRequestStream();
            streamToWrite.Write(this.data, 0, this.data.Length);
            streamToWrite.Close();
        }

        private void GetResponse() {
            response = this.request.GetResponse() as HttpWebResponse;
        }

        #endregion
    }
}


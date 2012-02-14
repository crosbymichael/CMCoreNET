using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

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

    internal abstract class SyncUrlRequest
    {
        #region Fields

        private UrlRequestMethod? method;
        private const int bufferReadSize = 1024;
        private byte[] data;
        private int timeout = 0;
        private HttpWebRequest request;
        private HttpWebResponse response;
        private System.Timers.Timer requestTimer;
        private int currentResponseTime = 0;

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

        public string UserAgent { get; set; }

        public WebHeaderCollection Headers { get; set; }
        public CookieCollection Cookies { get; set; }

        private bool HasData
        {
            get { return (this.data != null && this.data.Length > 0); }
        }

        #endregion

        #region Public Methods

        public void AddHeader(HttpRequestHeader name, string value)
        {
            this.Headers.Add(name, value);
        }

        public void AddCookie(Cookie cookie)
        {
            this.Cookies.Add(cookie);
        }

        public void Load(string url) { this.Load(url, null); }

        public void Load(string url,
            UrlRequestMethod? method)
        {
            this.Load(url, method, (string)null);
        }

        public void Load(string url,
            UrlRequestMethod? method,
            string data)
        {
            this.Load(url, method, EncodeString(data));
        }

        public void Load(string url,
            UrlRequestMethod? method,
            byte[] data)
        {
            if (!string.IsNullOrEmpty(url))
                this.Url = url;

            if (method != null)
                this.RequestMethod = method;

            if (data != null && data.Length > 0)
                this.data = data;

            if (string.IsNullOrEmpty(this.Url))
                throw new ArgumentNullException("The url cannot be null or empty");

            InitRequest();
        }

        #endregion

        #region Private Methods

        private byte[] EncodeString(string data)
        {
            if (!string.IsNullOrEmpty(data))
                return System.Text.Encoding.UTF8.GetBytes(data);
            return null;
        }

        private void StartTimer()
        {
            this.requestTimer = new System.Timers.Timer();
            this.requestTimer.Elapsed +=
                new System.Timers.ElapsedEventHandler(requestTimer_Elapsed);
            this.requestTimer.Start();
        }

        private void StopTimerAndDispose()
        {
            this.requestTimer.Stop();
            this.requestTimer.Dispose();
            this.requestTimer = null;
        }

        private void requestTimer_Elapsed(object sender,
            System.Timers.ElapsedEventArgs e)
        {
            this.currentResponseTime++;
        }

        private void CreateRequest()
        {
            request = HttpWebRequest.Create(this.Url) as HttpWebRequest;
        }

        private void SetupUserAgent() {
            if (!string.IsNullOrEmpty(this.UserAgent))
                request.UserAgent = this.UserAgent;
        }

        private void SetupHeaders()
        {
            if (this.Headers != null && this.Headers.Count > 0)
                request.Headers = this.Headers;
        }

        private void SetupContentType()
        {
            if (!string.IsNullOrEmpty(this.ContentType))
                request.ContentType = this.ContentType;
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

        private void SendRequestData()
        {
            Stream streamToWrite = request.GetRequestStream();
            streamToWrite.Write(this.data, 0, this.data.Length);
            streamToWrite.Close();
            GetResponseFromRequest();
        }

        private void GetResponseFromRequest() {
            response =
                this.request.GetResponse() as HttpWebResponse;
            StopTimerAndDispose();
        }

        #endregion


        #region Protected Members

        protected virtual void InitRequest()
        {
            SetupRequest();
            StartTimer();

            if (this.HasData)
                SendRequestData();
            else
                GetResponseFromRequest();
        }

        protected virtual void SetupRequest()
        {
            CreateRequest();
            SetRequestMethod();
            SetupUserAgent();
            SetupHeaders();
            SetupContentType();
            SetupTimeout();
        }

        protected HttpWebResponse GetResponse() {
            return this.response;
        }

        #endregion
    }
}

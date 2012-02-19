using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
        private const int bufferReadSize = 1024;
        private byte[] data;
        private int timeout = 0;
        private WebRequest request;
        private HttpWebResponse response;
        private UrlRequestEventArgs requestEventObject;
        private System.Timers.Timer requestTimer;
        private int currentResponseTime = 0;
        private SynchronizationContext context;
        private Action<UrlRequestEventArgs> callback;

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

        private bool HasData {
            get { return (this.data != null && this.data.Length > 0); }
        }

        #endregion

        #region Constructors

        public UrlRequest() {
            this.context = SynchronizationContext.Current;
        }

        #endregion

        #region Public Methods

        #region Static Methods

        public static string ProcessStreamToString(Stream stream) {
            if (stream == null)
                throw new ArgumentNullException("The requested stream is null");

            using (var reader = new StreamReader(stream)) {
                string content = reader.ReadToEnd();
                return content;
            }
        }

        #endregion

        public void AddHeader(HttpRequestHeader name, string value) {
            this.Headers.Add(name, value);
        }

        public void AddCookie(Cookie cookie) {
            this.Cookies.Add(cookie);
        }

        public void Load(string url, Action<UrlRequestEventArgs> callback) {
            this.Url = url;
            this.callback = callback;
            Task t = new Task(InitRequest);
            t.Start();

        }

        /// <summary>
        /// Kill a running request
        /// </summary>
        public void Kill() {
            if (this.request != null) request.Abort();
        }

        #endregion


        #region Private Methods

        private byte[] EncodeString(string data) {
            if (!string.IsNullOrEmpty(data))
                return System.Text.Encoding.UTF8.GetBytes(data);
            return null;
        }

        private void StartTimer() {
            this.requestTimer = new System.Timers.Timer();
            this.requestTimer.Elapsed += 
                new System.Timers.ElapsedEventHandler(requestTimer_Elapsed);
            this.requestTimer.Start();
        }

        private void StopTimerAndDispose() {
            this.requestTimer.Stop();
            this.requestTimer.Dispose();
            this.requestTimer = null;
        }

        private void requestTimer_Elapsed(object sender, 
            System.Timers.ElapsedEventArgs e)
        {
            this.currentResponseTime++;
        }

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

        private void InitRequest() {
            SetupRequest();
            StartTimer();

            if (this.HasData)
                SendRequestData();
            else
                GetResponse();
        }

        private void SendRequestData() {
            Stream streamToWrite = request.GetRequestStream();
            streamToWrite.Write(this.data, 0, this.data.Length);
            streamToWrite.Close();
            GetResponse();
        }

        private void GetResponse() {
            response =
                this.request.GetResponse() as HttpWebResponse;

            StopTimerAndDispose();
            PopulateEventObject();
            this.context.Post((o) => {
                this.callback.Invoke(o as UrlRequestEventArgs);
            }, this.requestEventObject);
        }

        private void PopulateEventObject() {
            this.requestEventObject = new UrlRequestEventArgs();
            this.requestEventObject.ResponseTime = this.currentResponseTime;
            this.requestEventObject.ContentLength = response.ContentLength;
            this.requestEventObject.ContentType = response.ContentType;
            this.requestEventObject.StatusCode = response.StatusCode;
            this.requestEventObject.Headers = response.Headers;
            this.requestEventObject.Cookies = response.Cookies;
            this.requestEventObject.ResponseStream = response.GetResponseStream();
        }

        private void CleanupSelf() {
            this.requestEventObject = null;
            this.response.Close();
            this.response = null;
            this.request = null;
        }

        #endregion
    }
}


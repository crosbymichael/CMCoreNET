using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CMCoreNET.Net;

namespace CMCoreNET.Web
{
    public class WebServer
    {
        #region Fields

        private string Url;
        private byte[] data;
        private UrlRequestMethod? method = null;
        private UrlRequest request;
        private HttpWebResponse response;
        private SynchronizationContext context;

        private Action<WebServerResponse> fullCallback;
        private Action<string> simpleCallback;

        #endregion

        #region Properties

        public string Path {
            get { return this.Url; }
            set { this.Url = value; }
        }

        public bool IsAlive { get { return true; } }

        public UrlRequestMethod? Method {
            get {
                if (this.method == null)
                    this.method = UrlRequestMethod.GET;
                return this.method;
            }
            set {
                this.method = value;
            }
        }

        public byte[] PostData {
            set { this.data = value; }
        }

        #endregion

        public WebServer(string path) {
            this.Path = path;
            this.context = SynchronizationContext.Current;
            CreateRequest();
        }

        #region Public Methods

        public void RegisterFullCallback(Action<WebServerResponse> callback) {
            this.fullCallback = callback;
        }

        public void RegisterSimpleCallback(Action<string> callback) {
            this.simpleCallback = callback;
        }

        public void Post() {
            Post(null);
        }

        public void Post(string data) {
            VerifiyCallbacks();
            if (string.IsNullOrEmpty(data) && this.data == null)
                throw new ArgumentNullException("The post data cannot be null or empty for a POST");
            if (!string.IsNullOrEmpty(data))
                this.data = data.GetBytes();
            StartTask();
        }

        public void Get() {
            VerifiyCallbacks();
            StartTask();
        }

        public void AddCookie(Cookie cookie) {
            this.request.Cookies.Add(cookie);
        }

        public void AddHeader(HttpRequestHeader name, string value) {
            this.request.Headers.Add(name, value);
        }

        #endregion

        #region Private Methods

        private void StartTask() {
            Task t = new Task(GetResponse);
            t.Start();
        }

        private void CreateRequest() {
            this.request = new UrlRequest(this.Url);
        }

        private void GetResponse() {
            this.response = this.request.Load(this.Url, this.Method, this.data);
            DispatchCallback();
        }

        private string ResponseDataToString() {
            string content = null;
            if (this.response != null) {
                using (TextReader reader = new StreamReader(this.response.GetResponseStream())) {
                    content = reader.ReadToEnd();
                }
            }
            return content;
        }

        private WebServerResponse CreateFullResponse() {

            WebServerResponse dto = new WebServerResponse();
            dto.Cookies = this.response.Cookies;
            dto.Headers = this.response.Headers;
            dto.StatusCode = (int)this.response.StatusCode;
            dto.ContentLength = (int)this.response.ContentLength;
            dto.ContentType = this.response.ContentType;
            dto.ContentEncoding = this.response.ContentEncoding;
            dto.StatusDescription = this.response.StatusDescription;
            
            var stream = this.response.GetResponseStream();
            using (TextReader reader = new StreamReader(stream)) {
                dto.Contents = reader.ReadToEnd().GetBytes();
            }
            return dto;
        }

        private void DispatchCallback() {
            if (fullCallback != null)
                DispatchFull();
            else if (simpleCallback != null)
                DispatchSimple();
            else
                throw new Exception("No callback to dispatch on");
        }

        private void DispatchFull() {
            var content = CreateFullResponse();
            context.Post((o) => {
                fullCallback.Invoke(content);
            }, null);
        }

        private void DispatchSimple() {
            var content = ResponseDataToString();
            context.Post((o) => {
                simpleCallback.Invoke(content);
            }, null);
        }

        private void VerifiyCallbacks() {
            var e = new Exception("No valid callback to dispatch");
            if (fullCallback == null && simpleCallback == null)
                throw e;
        }

        #endregion
    }

    public class WebServerResponse {
        public CookieCollection Cookies { get; set; }
        public WebHeaderCollection Headers { get; set; }
        public byte[] Contents { get; set; }
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string ContentType { get; set; }
        public int ContentLength { get; set; }
        public string ContentEncoding { get; set; }

        public WebServerResponse() { }
    }
}

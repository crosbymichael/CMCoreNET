using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCoreNET.Web
{
    public class WebServer
    {
        #region Fields

        private string Url;

        #endregion

        #region Properties

        public bool IsAlive { get; }

        #endregion

        public WebServer() { }

        public WebServer(string url) {
            this.Url = url;
        }

        #region Public Methods

        public void Post() { 
            
        }

        public void Head() { 
        
        }

        public void Get() { 
        
        }

        #endregion

        #region Private Methods



        #endregion
    }
}

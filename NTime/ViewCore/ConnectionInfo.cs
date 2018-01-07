using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore
{
    public class ConnectionInfo
    {
        public ConnectionInfo()
        {

        }

        public ConnectionInfo(string remoteURL)
        {

        }

        private string _serverURL;

        public string ServerURL
        {
            get { return _serverURL; }
            set { _serverURL = value; }
        }
    }
}

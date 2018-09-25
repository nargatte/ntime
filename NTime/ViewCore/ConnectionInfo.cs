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
            this.ServerURL = remoteURL;
        }

        public string ServerURL { get; set; }
    }
}

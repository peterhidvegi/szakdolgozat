using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    public class NetworkConnection : IDisposable
    {
        
        public const string GetSnapsDirectorys = "\\\\10.147.0.52\\validate\\cam\\archive";

        public const string GetMotionsDirectorys = "\\\\10.147.0.52\\validate\\motion\\archive";

        private readonly string _networkName = "\\\\10.147.0.52\\validate";
        
      //  private readonly string _networkName = "V:\\";



        public NetworkConnection(string name,string pass)
        {
            var netResource = new NetResource
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = _networkName.TrimEnd('\\')
            };

            var result = WNetAddConnection2(
                netResource, pass, name, 0);

            if (result != 0)
            {
                throw new Win32Exception(result);
            }
        }

        public event EventHandler<EventArgs> Disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var handler = Disposed;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }

            WNetCancelConnection2(_networkName, 0, true);
        }


        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,
                                                     string password,
                                                     string username,
                                                     int flags);
        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags, bool force);


        ~NetworkConnection()
        {
            Dispose(false);
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }

    public enum ResourceScope
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    };

 
    public enum ResourceType
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }

    public enum ResourceDisplaytype
    {
        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    }
}

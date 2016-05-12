using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    class WNetGetConnectionClass
    {

        [DllImport("mpr.dll")]
        static extern uint WNetGetConnection(string lpLocalName, StringBuilder lpRemoteName, ref int lpnLength);

        internal static bool IsLocalDrive(String driveName)
        {
            bool isLocal = true;  // assume local until disproved

            // strip trailing backslashes from driveName
            driveName = driveName.Substring(0, 2);

            int length = 256; // to be on safe side 
            StringBuilder networkShare = new StringBuilder(length);
            uint status = WNetGetConnection(driveName, networkShare, ref length);

            // does a network share exist for this drive?
            if (networkShare.Length != 0)
            {
                // now networkShare contains a UNC path in format \\MachineName\ShareName
                // retrieve the MachineName portion
                String shareName = networkShare.ToString();
                string[] splitShares = shareName.Split('\\');
                // the 3rd array element now contains the machine name
                if (Environment.MachineName == splitShares[2])
                    isLocal = true;
                else
                    isLocal = false;
            }

            return isLocal;
        }

    }
}

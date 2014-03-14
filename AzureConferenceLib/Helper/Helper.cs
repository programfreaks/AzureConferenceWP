using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;

namespace AzureConferenceLib.Helper
{
    public class HelperMethods
    {
        public static bool IsInternetAvailable
        {
            get
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
        }

        public static string ReadFile(string filePath)
        {
            var resrouceStream = Application.GetResourceStream(new Uri(filePath, UriKind.Relative));
            if (resrouceStream != null)
            {
                var myFileStream = resrouceStream.Stream;
                if (myFileStream.CanRead)
                {
                    var myStreamReader = new StreamReader(myFileStream);

                    return myStreamReader.ReadToEnd();
                }
            }
            return "";
        }
    }
}

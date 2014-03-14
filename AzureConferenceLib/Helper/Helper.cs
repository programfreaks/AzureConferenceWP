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

        public static string GetLocationOrder(string location)
        {
            switch (location)
            {
                case "Keynote":
                    return "1. " + location;
                    break;
                case "OPEN":
                    return "2. " + location;
                    break;
                case "BROAD(Develop)":
                    return "3. " + location;
                    break;
                case "FLEXIBLE(Deploy&Manage)":
                    return "4. " + location;
                    break;
                case "Start-up Conclave":
                    return "5. " + location;
                    break;
                case "MIC Champs Connect":
                    return "6. " + location;
                    break;
                case "HOL":
                    return "7. " + location;
                    break;
                default:
                    return "10";
            }
        }
    }
}

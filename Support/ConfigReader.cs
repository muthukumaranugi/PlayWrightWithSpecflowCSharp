using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PlayWrightWithSpecflowCSharp.Support
{
    public class ConfigReader
    {
        public static string appURL;
        //if only engine, provide ex: chromium; if it is installed browser, provide ex: chromium:chrome
        public static string browser;
        //provide value for wait time in seconds
        public static string defaultPageWait;
        public static string defaultWait;
        /*for browser resolution provide "systemdefault" for fit the screen of the machine
        or provide custom value like "1024x768"*/
        public static dynamic browserResolution;
        public static dynamic userCredentials;
        public static string headlessBrowser;
        public static dynamic playwrightTrace;
        public static string recordVideo;
        public static string loginURL;
        //For current current directory, provide "default", else provide the actual paths
        public static string baseStoragePath;
        public static string addlTracingAtBrowser;


    }
}

using PlayWrightWithSpecflowCSharp.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Infrastructure;

namespace PlayWrightWithSpecflowCSharp.Support
{
    public class CommonFunctions
    {
        public static string runSuffix { get; } = Guid.NewGuid().ToString("N");
        public static void SetCurrentDirectory()
        {
            string basePath = ConfigReader.baseStoragePath;
            if (basePath.ToLower() == "default" || basePath.ToLower() == "")
            {
                basePath = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                Directory.SetCurrentDirectory(basePath);
            }
        }

        public static async Task<byte[]> TakeScreenshotMethod(IPage page)
        {
            return await page.ScreenshotAsync();
        }

        public static string GetUsernameFromConfigByIdentifier(string userIdentifier)
        {
            dynamic userData = ConfigReader.userCredentials;
            string username = null;
            foreach (var userdetail in userData.GetChildren())
            {
                if (userdetail["identifier"] == userIdentifier.ToLower())
                {
                    username = userdetail["username"];
                    break;
                }
            }
            return username;
        }

        public static string GetPasswordFromConfigByIdentifier(string userIdentifier)
        {
            dynamic userData = ConfigReader.userCredentials;
            string password = null;
            foreach (var userdetail in userData.GetChildren())
            {
                if (userdetail["identifier"] == userIdentifier.ToLower())
                {
                    password = userdetail["password"];
                    break;
                }
            }
            return password;
        }

        public static string CheckRunSuffix(string field)
        {
            if (!String.IsNullOrEmpty(field) && !field.EndsWith(runSuffix))
            {
                field = String.Format("{0}_{1}", field, runSuffix);
            }

            return field;
        }

        public static string RemoveRunSuffix(string field)
        {
            if (!String.IsNullOrEmpty(field))
            {
                if (field.EndsWith(runSuffix))
                {
                    field = field.Remove(field.Length - runSuffix.Length - 1);
                }
            }

            return field;
        }

        public static int GetUniqueInteger()
        {
            int timestamp = Convert.ToInt32(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            return timestamp;
        }

    }
}

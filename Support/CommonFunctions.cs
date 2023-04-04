﻿using PlayWrightWithSpecflowCSharp.Support;
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

    }
}

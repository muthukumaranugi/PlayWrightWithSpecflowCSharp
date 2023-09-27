using BoDi;
using CAP.SpecTests.Drivers;
using Gherkin;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using PlayWrightWithSpecflowCSharp.Drivers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.Infrastructure;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(3)]

namespace PlayWrightWithSpecflowCSharp.Support
{
    [Binding]
    public sealed class Hooks
    {

        [BeforeTestRun(Order = 1)]
        public static void RegisterDependencies(IObjectContainer _objectContainer)
        {
            IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"), optional: true, reloadOnChange: true)
               .Build();
            _objectContainer.RegisterInstanceAs(config);
            _objectContainer.RegisterTypeAs<BrowserDriver, IBrowserDriver>();
            _objectContainer.RegisterTypeAs<AzureADApiDriver, IAzureADApiDriver>();

            ConfigReader.appURL = config["applicationURL"];
            ConfigReader.browser = config["browerName"];
            ConfigReader.defaultPageWait = config["pageLoadWaitTime"];
            ConfigReader.defaultWait = config["defaultWaitTime"];
            ConfigReader.browserResolution = config.GetSection("browserResolution");
            ConfigReader.userCredentials = config.GetSection("userData");
            ConfigReader.headlessBrowser = config["headlessBrowser"];
            ConfigReader.playwrightTrace = config.GetSection("PlayWriteTrace");
            ConfigReader.recordVideo = config["RecordVideo"];
            ConfigReader.loginURL = config["loginPageURL"];
            ConfigReader.baseStoragePath = config["BaseStoragePath"];
            ConfigReader.addlTracingAtBrowser = config["AddlTracingAtBrowser"];
            ConfigReader.azureADAuthInfo = config.GetSection("AzureADAuthInfo");
            ConfigReader.azureADBaseUrl = config["azureADBaseUrl"];
            ConfigReader.azureADDomain = config["AzureADDomain"];

            CommonFunctions.SetCurrentDirectory();
            var artifactDirectory = Path.Combine(Directory.GetCurrentDirectory(), "FailedScreenshots");
            if (Directory.Exists(artifactDirectory))
            {
                Directory.Delete(artifactDirectory, recursive: true);
            }

            var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
            if (exitCode != 0)
            {
                throw new Exception($"Playwright exited with code {exitCode}");
            }

        }

        [BeforeFeature(Order = 1)]
        public static void CreateBrowserInstance(IObjectContainer _objectContainer, FeatureContext featureContext)
        {
            string featureTitle = featureContext.FeatureInfo.Title;
            IBrowserDriver _browserDr = new BrowserDriver();
            _objectContainer.RegisterInstanceAs(_browserDr);

            IBrowser _browser = _browserDr.InitializeBrowser(featureTitle).GetAwaiter().GetResult();
            IBrowserContext _browserContext = _browserDr.InitializeBrowserContext(_browser, featureTitle).GetAwaiter().GetResult();


            IPage webpage = _browserDr.InitializeBrowserPage(_browserContext).GetAwaiter().GetResult();

            _objectContainer.RegisterInstanceAs(_browser);
            _objectContainer.RegisterInstanceAs(_browserContext);
            _objectContainer.RegisterInstanceAs(webpage);

            _browserDr.StartTracerInDriver(featureContext.FeatureInfo.Title, _browserContext).GetAwaiter().GetResult();

        }

        [AfterFeature]
        public static void CloseBrowserInstance(FeatureContext featureContext, IBrowser _browser, IBrowserDriver _browserDr, IBrowserContext _browserContext, IPage webpage)
        {
            _browserDr.StopTracerInDriver(featureContext.FeatureInfo.Title, _browserContext).GetAwaiter().GetResult();
            _browserDr.CloseBrowser(webpage, _browserContext, _browser).GetAwaiter().GetResult();

        }

    }
}

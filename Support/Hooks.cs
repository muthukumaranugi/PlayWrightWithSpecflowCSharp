using BoDi;
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

        [BeforeFeature(Order = 2)]
        public static async Task InitializeToken(FeatureContext featureContext, IBrowserDriver _browserDr, IPage webpage)
        {
            await _browserDr.NaviagateToAppURL(webpage);

            async Task InputCredentials(string useremail, string userpass)
            {
                await webpage.Locator("input#email").ClearandFill(useremail);
                await webpage.Locator("input#password").ClearandFill(userpass);
                await webpage.Locator("button#next").ClickAsync();
            }

            if (featureContext.FeatureInfo.Tags.Contains("LoginAsLabDirector"))
            {
                string useremail = CommonFunctions.GetUsernameFromConfigByIdentifier("labdirector");
                string userpass = CommonFunctions.GetPasswordFromConfigByIdentifier("labdirector");
                await InputCredentials(useremail, userpass);

            }

        }

        [BeforeScenario]
        public void LogScenarioName(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            string featureTitle = featureContext.FeatureInfo.Title;
            string scenarioTitle = scenarioContext.ScenarioInfo.Title;
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now;
            Console.WriteLine("Beginning scenario execution :" + currentDateTimeOffset.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
            Console.WriteLine($"Executing feature_scenario: {featureTitle}_{scenarioTitle}");
            Console.WriteLine($"ScenarioTitle : {scenarioTitle.Replace(" ", "")}");
        }

        [AfterStep]
        public void AddScreenshotsandLogTime(ScenarioContext scenarioContext, ISpecFlowOutputHelper _specFlowOutputHelper, IPage webpage)
        {

            IPage page = webpage;
            var testStatus = scenarioContext.TestError;
            if (testStatus != null)
            {
                var screenshot = CommonFunctions.TakeScreenshotMethod(page).GetAwaiter().GetResult();
                var artifactDirectory = Path.Combine(Directory.GetCurrentDirectory(), "FailedScreenshots");
                if (!Directory.Exists(artifactDirectory))
                {
                    Directory.CreateDirectory(artifactDirectory);
                }
                string title = scenarioContext.ScenarioInfo.Title;
                string fileName = "error_" + title + DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss") + "_screenshot.png";
                var sourceFilePath = Path.Combine(artifactDirectory, fileName);
                File.WriteAllBytes(sourceFilePath, screenshot);
                var finalPath = Path.GetFullPath(sourceFilePath);
                _specFlowOutputHelper.WriteLine("Error Screenshot");
                _specFlowOutputHelper.AddAttachment(fileName);
            }
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now;
            Console.WriteLine("Step completed : " + currentDateTimeOffset.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));

        }


        [AfterFeature]
        public static void CloseBrowserInstance(FeatureContext featureContext, IBrowser _browser, IBrowserDriver _browserDr, IBrowserContext _browserContext, IPage webpage)
        {
            _browserDr.StopTracerInDriver(featureContext.FeatureInfo.Title, _browserContext).GetAwaiter().GetResult();
            _browserDr.CloseBrowser(webpage, _browserContext, _browser).GetAwaiter().GetResult();

        }

    }
}

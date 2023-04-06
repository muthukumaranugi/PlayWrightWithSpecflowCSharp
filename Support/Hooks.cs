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

        [BeforeScenario(Order = 1)]
        public void LogScenarioName(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            string featureTitle = featureContext.FeatureInfo.Title;
            string scenarioTitle = scenarioContext.ScenarioInfo.Title;
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now;
            Console.WriteLine("Beginning scenario execution :" + currentDateTimeOffset.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
            Console.WriteLine($"Executing feature_scenario: {featureTitle}_{scenarioTitle}");
            Console.WriteLine($"ScenarioTitle : {scenarioTitle.Replace(" ", "")}");

        }

        [BeforeScenario(Order = 2)]
        public async Task InitializeToken(FeatureContext featureContext, IBrowserDriver _browserDr, IPage webpage, ScenarioContext scenarioContext)
        {
            await _browserDr.NaviagateToAppURL(webpage);

            async Task InputCredentials(string useremail, string userpass)
            {
                /*
                //Implement login functionality
                await webpage.Locator("input#email").ClearandFill(useremail);
                await webpage.Locator("input#password").ClearandFill(userpass);
                await webpage.Locator("button#next").ClickAsync();*/
                Console.WriteLine("Successfully signed in before scenario");

            }

            if (!(featureContext.FeatureInfo.Tags.Contains("skipLogin") || scenarioContext.ScenarioInfo.Tags.Contains("skipLogin")))
            {
                if (featureContext.FeatureInfo.Tags.Contains("LoginAsAdmin") || scenarioContext.ScenarioInfo.Tags.Contains("LoginAsAdmin"))
                {
                    string useremail = CommonFunctions.GetUsernameFromConfigByIdentifier("labdirector");
                    string userpass = CommonFunctions.GetPasswordFromConfigByIdentifier("labdirector");
                    await InputCredentials(useremail, userpass);
                }
            }

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


        [AfterScenario]
        public async Task SignOutOfApplication(FeatureContext featureContext, ScenarioContext scenarioContext, IPage webpage)
        {
            ILocator homepageSignoutIcon = webpage.Locator("#test");
            ILocator homepageSignoutButton = webpage.Locator("//*[@type='button']//*[text()='Logout']");
            ILocator loginpageSignInButton = webpage.Locator("//button[text()='Sign in']");
            if (!featureContext.FeatureInfo.Tags.Contains("skipSignOut") || !scenarioContext.ScenarioInfo.Tags.Contains("skipSignOut"))
            {
                //bool successfulSignIn = await homepageSignoutIcon.IsVisibleAsync();
                bool successfulSignIn = true;
                if (successfulSignIn)
                {
                    /*
                    //Implement logout function
                    await homepageSignoutIcon.ClickAsync();
                    await homepageSignoutButton.ClickAsync();
                    await loginpageSignInButton.WaitForElementVisibility();*/
                    Console.WriteLine("Successfully signed out after scenario");
                }
            }

        }


        [AfterFeature]
        public static void CloseBrowserInstance(FeatureContext featureContext, IBrowser _browser, IBrowserDriver _browserDr, IBrowserContext _browserContext, IPage webpage)
        {
            _browserDr.StopTracerInDriver(featureContext.FeatureInfo.Title, _browserContext).GetAwaiter().GetResult();
            _browserDr.CloseBrowser(webpage, _browserContext, _browser).GetAwaiter().GetResult();

        }

    }
}

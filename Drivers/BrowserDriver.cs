using BoDi;
using PlayWrightWithSpecflowCSharp.Support;
using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayWrightWithSpecflowCSharp.Drivers
{
    public class BrowserDriver : IBrowserDriver
    {

        public async Task<IBrowser> InitializeBrowser(string featureName)
        {
            string? browserDetails = TestContext.Parameters["browserName"];
            browserDetails = browserDetails == null || browserDetails == "" ? Environment.GetEnvironmentVariable("browserName") : null;
            IPlaywright _playwright = await Playwright.CreateAsync();
            BrowserTypeLaunchOptions browserTypeLaunchOptions = new BrowserTypeLaunchOptions();
            if (browserDetails == null || browserDetails == "")
            {
                browserDetails = ConfigReader.browser;
            }
            string[] browserConfigDetails = browserDetails.ToLower().Split(':');
            var browserType = browserConfigDetails[0]; // Can be "chromium", "firefox", "webkit"

            if (browserConfigDetails.Length > 1)
            {
                browserTypeLaunchOptions.Channel = browserConfigDetails[1]; // Can be "chromium", "msedge", "chrome", "webkit", "firefox", "chrome-beta", "msedge-beta", "msedge-dev", etc.
            }


            string headlessRunConfig = ConfigReader.headlessBrowser;
            bool headlessRun = headlessRunConfig.ToLower() == "yes" ? true : false;
            browserTypeLaunchOptions.Headless = headlessRun;


            string additionalTracing = ConfigReader.addlTracingAtBrowser;
            if (additionalTracing.ToLower() == "yes")
            {
                if (Directory.Exists($"NewTraces/Trace_{featureName}"))
                {
                    Directory.Delete($"NewTraces/Trace_{featureName}", recursive: true);
                }
                browserTypeLaunchOptions.TracesDir = $"NewTraces/Trace_{featureName}";
            }

            //browserTypeLaunchOptions.Timeout = 60000;
            //browserTypeLaunchOptions.SlowMo = 2000;

            IBrowser _browser = await _playwright[browserType].LaunchAsync(browserTypeLaunchOptions);
            return _browser;

        }

        public async Task<IBrowserContext> InitializeBrowserContext(IBrowser _browser, string featureTitle)
        {
            BrowserNewContextOptions browserNewContextOptions = new BrowserNewContextOptions();

            //browserNewContextOptions.StorageStatePath = "./Data/auth.json";
            string recordVideo = ConfigReader.recordVideo;
            if (recordVideo.ToLower() == "yes")
            {
                if (Directory.Exists($"Videos/Feature_{featureTitle}"))
                {
                    Directory.Delete($"Videos/Feature_{featureTitle}", recursive: true);
                }
                browserNewContextOptions.RecordVideoDir = $"Videos/Feature_{featureTitle}";
            }
            IBrowserContext browserContext = await _browser.NewContextAsync(browserNewContextOptions);
            return browserContext;
        }

        public async Task<IPage> InitializeBrowserPage(IBrowserContext browserContext)
        {
            float pageWaitTime = float.Parse(ConfigReader.defaultPageWait) * 1000;
            float defwaitTime = float.Parse(ConfigReader.defaultWait) * 1000;
            IPage webpage = await browserContext.NewPageAsync();

            webpage.SetDefaultNavigationTimeout(pageWaitTime);
            webpage.SetDefaultTimeout(defwaitTime);

            dynamic brResolution = ConfigReader.browserResolution;
            string defaultBrResolution = brResolution["systemdefault"];
            if (defaultBrResolution.ToLower() != "yes")
            {
                int pageWidth = Convert.ToInt32(brResolution["pageWidth"]);
                int pageHeight = Convert.ToInt32(brResolution["pageHeight"]);
                await webpage.SetViewportSizeAsync(pageWidth, pageHeight);

            }
            return webpage;

        }

        public async Task NaviagateToAppURL(IPage webpage)
        {
            await webpage.GotoAsync(ConfigReader.appURL);
        }

        public async Task StartTracerInDriver(string featureName, IBrowserContext browserContext)
        {
            dynamic tracerConfig = ConfigReader.playwrightTrace;
            string enableTracing = tracerConfig["EnableTrace"];
            string screenshots = tracerConfig["Screenshots"];
            string snapshots = tracerConfig["Snapshots"];
            string sources = tracerConfig["Sources"];

            if (enableTracing.ToLower() == "yes")
            {
                TracingStartOptions traceStartOptions = new TracingStartOptions();
                traceStartOptions.Name = featureName;
                traceStartOptions.Title = featureName;
                traceStartOptions.Screenshots = screenshots.ToLower() == "yes" ? true : false;
                traceStartOptions.Snapshots = snapshots.ToLower() == "yes" ? true : false;
                traceStartOptions.Sources = sources.ToLower() == "yes" ? true : false;
                await browserContext.Tracing.StartAsync(traceStartOptions);
            }

        }

        public async Task StopTracerInDriver(string featureName, IBrowserContext browserContext)
        {
            dynamic tracerConfig = ConfigReader.playwrightTrace;
            string enableTracing = tracerConfig["EnableTrace"];
            if (enableTracing.ToLower() == "yes")
            {
                await browserContext.Tracing.StopAsync(new()
                {
                    Path = $"Traces/trace_{featureName}.zip"
                });
            }
        }


        public async Task CloseBrowser(IPage page, IBrowserContext browserContext, IBrowser browser)
        {
            await browserContext.CloseAsync();
            await page.CloseAsync();
            await browser.CloseAsync();
        }


    }
}

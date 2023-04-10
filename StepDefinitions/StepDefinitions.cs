using BoDi;
using PlayWrightWithSpecflowCSharp.Drivers;
using PlayWrightWithSpecflowCSharp.PageObjects;
using PlayWrightWithSpecflowCSharp.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Infrastructure;

namespace PlayWrightWithSpecflowCSharp.StepDefinitions
{
    [Binding]
    public partial class StepDefinitions
    {
        IPage page;
        //static IBrowserDriver _browserDr;
        HeroKuappHomePage herokuHomePage;
        HeroKuappCheckboxPage herokuCheckboxPage;
        HeroKuappIFramePage herokuIFramePage;
        public StepDefinitions(IPage webpage, IBrowserDriver browserDriver)
        {
            page = webpage;
            herokuHomePage = new HeroKuappHomePage(page);
            herokuCheckboxPage = new HeroKuappCheckboxPage(page);
            herokuIFramePage = new HeroKuappIFramePage(page);
            //_browserDr = browserDriver;
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
                
                //Implement login functionality
                
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
            if (!featureContext.FeatureInfo.Tags.Contains("skipSignOut") || !scenarioContext.ScenarioInfo.Tags.Contains("skipSignOut"))
            {
                //bool successfulSignIn = await homepage.GetSignOutIconVisibility();;
                bool successfulSignIn = true;
                if (successfulSignIn)
                {
                    
                    //Implement logout function
                   
                    Console.WriteLine("Successfully signed out after scenario");
                }
            }

        }


        



    }
}

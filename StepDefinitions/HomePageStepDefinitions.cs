using PlayWrightWithSpecflowCSharp.PageObjects;
using TechTalk.SpecFlow;

namespace PlayWrightWithSpecflowCSharp.StepDefinitions
{   
    public partial class StepDefinitions
    {

        [Given(@"user is navigated to the home page")]
        [When(@"user is navigated to the home page")]
        public async Task WhenUserIsNavigatedToTheHomePage()
        {
            await herokuHomePage.VerifyUserInHomePage();
        }

        [Then(@"the following links should be available")]
        public async Task ThenTheFollowingLinksShouldBeAvailable(Table table)
        {
            await herokuHomePage.VerifyLinks("Frames");
            await herokuHomePage.PrintAllTextContents();
        }

        [Given(@"Navigates to the ""([^""]*)"" link from home page")]
        [When(@"Navigates to the ""([^""]*)"" link from home page")]
        public async Task GivenNavigatesToTheLinkFromHomePage(string PageName)
        {
            await herokuHomePage.NavigateToPage(PageName);
        }
     

    }
}
using CAP.SpecTests.Support;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using NUnit.Framework;
using PlayWrightWithSpecflowCSharp.Models;
using PlayWrightWithSpecflowCSharp.PageObjects;
using PlayWrightWithSpecflowCSharp.Support;
using System.Text.Json;
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
        
        [Given(@"user is created with username ""([^""]*)"" and password ""([^""]*)""")]
        public void GivenUserIsCreatedWithUsernameAndPassword(string username, string password)
        {
            BookUser userDetail = new BookUser()
            {
                id = CommonFunctions.GetUniqueInteger(),
                userName = username,
                password = password
            };
            apiDriver.CreateOneBookUser<BookUser>(userDetail);
            apiDriver.VerifyTheAPIResponseCode(200);
            BookUser createdBookUser = CommonJsonFunctions.DeserializeJsonResponse<BookUser>(apiDriver.GetResponseMessage());
            Console.WriteLine($"The api response for creating book user is:" + apiDriver.GetResponseMessage());
            createdBookUser.Should().BeEquivalentTo(userDetail);
        }

        [When(@"get the details of user with id ""([^""]*)""")]
        public void WhenGetTheDetailsOfUserWithId(string id)
        {
            BookUser userDetail = new BookUser()
            {
                id = Convert.ToInt32(id),
                userName = $"User {id}",
                password = $"Password{id}"
            };
            apiDriver.GetOneBookUser<BookUser>(id);
            apiDriver.VerifyTheAPIResponseCode(200);
            BookUser createdBookUser = CommonJsonFunctions.DeserializeJsonResponse<BookUser>(apiDriver.GetResponseMessage());
            Console.WriteLine($"The api response for getting book user is:" + apiDriver.GetResponseMessage());
            createdBookUser.Should().BeEquivalentTo(userDetail);
        }

        [Then(@"delete the user details with id ""([^""]*)""")]
        public void ThenDeleteTheUserDetailsWithId(string id)
        {
            apiDriver.GetOneBookUser<BookUser>(id);
            apiDriver.VerifyTheAPIResponseCode(200);
        }



    }
}
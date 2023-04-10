using PlayWrightWithSpecflowCSharp.PageObjects;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace PlayWrightWithSpecflowCSharp.StepDefinitions
{   
    public partial class StepDefinitions
    {
        string jsalertButtonName = null;

        [When(@"clicks on ""([^""]*)"" button")]
        public void WhenClicksOnButton(string buttonName)
        {
            jsalertButtonName = buttonName;
            
        }

        [Then(@"the alert is displayed with the message ""([^""]*)""")]
        public async Task ThenTheAlertIsDisplayedWithTheMessage(string expectedAlertMessage)
        {
            await herokuJSAlertsPage.AlertAction("Accept", null);
            await herokuJSAlertsPage.ClickAlertButton(jsalertButtonName);
            herokuJSAlertsPage.VerifyAlertMessage(expectedAlertMessage);            
        }

        [Then(@"the alert is displayed with the message ""([^""]*)"" and ""([^""]*)"" the alert")]
        public async Task ThenTheAlertIsDisplayedWithTheMessageAndTheAlert(string expectedAlertMessage, string alertAction)
        {
            await herokuJSAlertsPage.AlertAction(alertAction, null);
            await herokuJSAlertsPage.ClickAlertButton(jsalertButtonName);
            herokuJSAlertsPage.VerifyAlertMessage(expectedAlertMessage);
        }

        [Then(@"the alert is displayed with the message ""([^""]*)"" input ""([^""]*)"" and ""([^""]*)"" the alert")]
        public async Task ThenTheAlertIsDisplayedWithTheMessageInputAndTheAlert(string expectedAlertMessage, string inputMessage, string alertAction)
        {
            await herokuJSAlertsPage.AlertAction(alertAction, inputMessage);
            await herokuJSAlertsPage.ClickAlertButton(jsalertButtonName);
            herokuJSAlertsPage.VerifyAlertMessage(expectedAlertMessage);
        }

        [Then(@"the message ""([^""]*)"" is displayed in the page")]
        public async Task ThenTheMessageIsDisplayedInThePage(string expectedResultMessage)
        {
            await herokuJSAlertsPage.VerifyResultMessage(expectedResultMessage);
        }

    }
}
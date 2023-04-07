using PlayWrightWithSpecflowCSharp.PageObjects;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace PlayWrightWithSpecflowCSharp.StepDefinitions
{   
    public partial class StepDefinitions
    {

        [Then(@"verify the status ""([^""]*)"" of the checkbox ""([^""]*)""")]
        public async Task ThenVerifyTheStatusOfTheCheckbox(string checkboxStatus, string checkBoxName)
        {
            await herokuCheckboxPage.VerifyCheckboxStatus(checkBoxName, checkboxStatus);
        }

        [When(@"set the status of the checkbox ""([^""]*)"" as ""([^""]*)""")]
        public async Task WhenSetTheStatusOfTheCheckboxAs(string checkBoxName, string checkboxStatus)
        {            
            await herokuCheckboxPage.SetCheckboxStatus(checkBoxName, checkboxStatus);
        }

    }
}
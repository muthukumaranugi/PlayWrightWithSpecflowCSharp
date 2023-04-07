using PlayWrightWithSpecflowCSharp.PageObjects;
using TechTalk.SpecFlow;

namespace PlayWrightWithSpecflowCSharp.StepDefinitions
{   
    public partial class StepDefinitions
    {
        [When(@"Selects new document from the File menu")]
        public async Task WhenSelectsNewDocumentFromTheFileMenu()
        {
            await herokuIFramePage.CreateNewDocument();
        }

        [When(@"inputs the document string in the editor")]
        public async Task WhenInputsTheDocumentStringInTheEditor(string multilineText)
        {
            await herokuIFramePage.InputDataIntheTextEditor(multilineText);
        }


    }
}
using PlayWrightWithSpecflowCSharp.PageObjects;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace PlayWrightWithSpecflowCSharp.StepDefinitions
{   
    public partial class StepDefinitions
    {

        [When(@"clicks (.*) times on ""([^""]*)"" header of the ""([^""]*)"" table")]
        public async Task WhenClicksTimesOnHeaderOfTheTable(int timesOfClick, string columnName, string tableName)
        {
            for (int i = 0; i < timesOfClick; i++)
            {
                await herokutablePage.ClickTableHeader(tableName, columnName);
            }            
        }

        [Then(@"the data in ""([^""]*)"" column of the ""([^""]*)"" table should be sorted in ""([^""]*)"" order")]
        public async Task ThenTheDataInColumnOfTheTableShouldBeSortedInOrder(string columnName, string tableName, string sortOrder)
        {
            await herokutablePage.VerifyColumnSorting(tableName, columnName, sortOrder);
        }

    }
}
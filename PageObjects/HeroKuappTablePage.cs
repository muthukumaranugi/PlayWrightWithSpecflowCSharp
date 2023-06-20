using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayWrightWithSpecflowCSharp.PageObjects
{
    public class HeroKuappTablePage
    {
        private IPage page;
        public HeroKuappTablePage(IPage _page) => page = _page;

        ILocator exampleTable => page.Locator("table#table1");


        private ILocator GetTableLocator(string tableName)
        {
            return page.Locator($"//table[@id='{tableName}']");
        }

        private ILocator GetHeaderNameLocator(string tableName, string columnName)
        {
            string columnLocatorString =  $"//thead//*[contains(@class,'header')]//*[text()='{columnName}']";
            return GetTableLocator(tableName).Locator(columnLocatorString);
        }

        private ILocator GetHeaderColumns(string tableName)
        {
            string columnsLocatorString = $"//thead//*[contains(@class,'header')]";
            return GetTableLocator(tableName).Locator(columnsLocatorString);
        }

        public async Task VerifyColumnSorting(string tableName, string columnName)
        {
            int columnIndex = await GetColumnIndexUsingName(tableName, columnName);


        }

        public async Task<int> GetColumnCount(string tableName)
        {
            return await GetHeaderColumns(tableName).CountAsync();
        }

        public async Task<int> GetColumnIndexUsingName(string tableName, string columnName)
        {
            int columnCount = await GetColumnCount(tableName);

            int columnIndex = 0;
            for (int i = 0; i < columnCount; i++)
            {
                string? currentColumnName = await GetHeaderColumns(tableName).Nth(columnCount).TextContentAsync();
                if (currentColumnName == columnName)
                {
                    columnIndex = i;
                    break;
                }
                if (i == columnCount - 1)
                {
                    Assert.That(false, $"Unable to find the column - '{columnName}'");
                }

            }
            return columnIndex;
        }


    }
}

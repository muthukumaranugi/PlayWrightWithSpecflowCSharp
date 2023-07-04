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
            //return page.Locator($"//table[@id='{tableName}']");
            return page.Locator($"//*[text()='{tableName}']//following-sibling::table").First;
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

        private ILocator GetTableRowsLocator(string tableName)
        {
            return GetTableLocator(tableName).Locator("//tbody//tr");
        }

        public async Task<string?> GetTableCellValue(string tableName, int rowIndex, int columnIndex)
        {
            string? tableCellValue = await GetTableRowsLocator(tableName).Nth(rowIndex)
                                    .Locator("//td").Nth(columnIndex)
                                    .TextContentAsync();
            return tableCellValue;
        }

        public async Task VerifyColumnSorting(string tableName, string columnName, string expectedSortOrder)
        {
            //await page.PauseAsync();
            //Get the column index using column name
            int columnIndex = await GetColumnIndexUsingName(tableName, columnName);
            Console.WriteLine("Printing column index using name" + columnIndex);

            //Get the rows count in the table
            int rowCount = await GetRowsCount(tableName);

            //Compare Grid cell Text for sorting assertion
            string? currGridCellValue = string.Empty;
            string? nextGridCellValue = string.Empty;

            for (int rowIndex = 0; rowIndex < rowCount - 1; rowIndex++)
            {
                currGridCellValue = await GetTableCellValue(tableName, rowIndex, columnIndex);
                nextGridCellValue = await GetTableCellValue(tableName, rowIndex+1, columnIndex);

                if (expectedSortOrder.ToLower() == "ascending" || expectedSortOrder.ToLower() == "asc")
                {
                    if (currGridCellValue.StartsWith("$"))
                    {
                        double currValue = Convert.ToDouble(currGridCellValue!.TrimStart('$'));
                        double nextValue = Convert.ToDouble(nextGridCellValue!.TrimStart('$'));
                        Assert.That(currValue, Is.LessThanOrEqualTo(nextValue), $"The Column - '{columnName}' is not sorted as expected in ascending order");
                    }
                    else 
                    {
                        Assert.That(string.Compare(currGridCellValue, nextGridCellValue), Is.LessThanOrEqualTo(0), $"The Column - '{columnName}' is not sorted as expected in ascending order");
                    }                   
                }

                if (expectedSortOrder.ToLower() == "descending" || expectedSortOrder.ToLower() == "desc")
                {
                    if(currGridCellValue.StartsWith("$"))
                    {
                        double currValue = Convert.ToDouble(currGridCellValue!.TrimStart('$'));
                        double nextValue = Convert.ToDouble(nextGridCellValue!.TrimStart('$'));
                        Assert.That(currValue, Is.GreaterThanOrEqualTo(nextValue), $"The Column - '{columnName}' is not sorted as expected in ascending order");
                    }
                    else 
                    {
                        Assert.That(string.Compare(currGridCellValue, nextGridCellValue), Is.GreaterThanOrEqualTo(0), $"The Column - '{columnName}' is not sorted as expected in descending order");
                    }                    
                }
            }


        }

        public async Task<int> GetColumnCount(string tableName)
        {
            return await GetHeaderColumns(tableName).CountAsync();
        }

        public async Task<int> GetRowsCount(string tableName)
        {
            return await GetTableRowsLocator(tableName).CountAsync();
        }

        public async Task<int> GetColumnIndexUsingName(string tableName, string columnName)
        {
            int columnCount = await GetColumnCount(tableName);

            int columnIndex = 0;
            for (int i = 0; i < columnCount; i++)
            {
                string? currentColumnName = await GetHeaderColumns(tableName).Nth(i).TextContentAsync();
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

        public async Task ClickTableHeader(string tableName, string columnName)
        {
            await GetHeaderNameLocator(tableName, columnName).ClickAsync();
            Console.WriteLine($"Clicked on the column '{columnName}'");
        }

    }
}

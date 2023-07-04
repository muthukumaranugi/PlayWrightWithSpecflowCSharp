Feature: HeroKuSortingTablePage

A short summary of the feature

Scenario Outline: Verify the ascending sorting validation in a table
	Given user is navigated to the home page
	When Navigates to the "Sortable Data Tables" link from home page
	And clicks 1 times on "<ColumnName>" header of the "<TableName>" table
	Then the data in "<ColumnName>" column of the "<TableName>" table should be sorted in "ascending" order
Examples: 
| TableName | ColumnName |
| Example 1 | Last Name  |
| Example 1 | First Name |
| Example 1 | Email      |
| Example 1 | Due        |
| Example 2 | Last Name  |
| Example 2 | Due        |
| Example 2 | Web Site   |

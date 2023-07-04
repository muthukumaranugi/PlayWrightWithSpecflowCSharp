using PlayWrightWithSpecflowCSharp.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayWrightWithSpecflowCSharp.PageObjects
{
    public class HeroKuappHomePage
    {
        private IPage page;
        public HeroKuappHomePage(IPage _page) => page = _page;
        ILocator heading => page.GetByText("Available Examples");

        public async Task NavigateToPage(string pageName)
        {
            ILocator TestExamplePageNavLink = page.GetByText(pageName).Nth(0);
            pageName = pageName == "Sortable Data Tables" ? "Data Tables"
                       : pageName;
            ILocator PageHeading = page.GetByRole(AriaRole.Heading, new() { Name = pageName });
            await TestExamplePageNavLink.ClickAsync();
            await PageHeading.WaitForElementVisibility();
            Console.WriteLine($"Navigated to the Page - {pageName}");
        }

        public async Task VerifyLinks(string pageName)
        {
            ILocator TestExamplePageNavLink = page.GetByText(pageName).Nth(0);
            await TestExamplePageNavLink.IsVisibleAsync();
        }

        public async Task PrintAllTextContents()
        {
            //var version = BasePage.browserdata.Version;
            //Console.WriteLine("Browser version: " + version);
            ILocator rootNode = page.Locator("//ul");
            foreach (var textContent in await rootNode.AllTextContentsAsync())
            {
                Console.WriteLine("Printing textContent" + textContent);
            }
        }

        public async Task VerifyUserInHomePage()
        {
            page.GetByText("Available Examples");
            await heading.IsVisibleAsync();
            Console.WriteLine("Heroku app home page is visible with title \"Available Examples\"");
        }


    }
}

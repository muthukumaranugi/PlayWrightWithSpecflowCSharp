using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayWrightWithSpecflowCSharp.PageObjects
{
    public class HeroKuappCheckboxPage
    {
        private IPage page;
        public HeroKuappCheckboxPage(IPage _page) => page = _page;

        public async Task SetCheckboxStatus(string checkboxName,string status)
        {
            ILocator checkbox = page.Locator($"//*[@id='checkboxes']/text()[normalize-space()='{checkboxName}']/preceding-sibling::input[1]");
            if (status.ToLower() == "checked")
            {
                await checkbox.CheckAsync();
                Console.WriteLine("Checkbox is checked");
            }
            else if (status.ToLower() == "unchecked")
            { 
                await checkbox.UncheckAsync();
                Console.WriteLine("Checkbox is unchecked");
            }
            
        }

        public async Task VerifyCheckboxStatus(string checkboxName, string status)
        {
            ILocator checkbox = page.Locator($"//*[@id='checkboxes']/text()[normalize-space()='{checkboxName}']/preceding-sibling::input[1]");
            if (status.ToLower() == "checked")
            {
                Assert.That(await checkbox.IsCheckedAsync(), Is.True, "Checkbox is not checked as expected");
                Console.WriteLine("Checkbox is checked as expected");
            }
            else if (status.ToLower() == "unchecked")
            {
                Assert.That(await checkbox.IsCheckedAsync(), Is.False, "Checkbox is not unchecked as expected");
                Console.WriteLine("Checkbox is unchecked as expected");
            }

        }

    }
}

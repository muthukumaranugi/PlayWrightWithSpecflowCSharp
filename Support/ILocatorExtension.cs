using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayWrightWithSpecflowCSharp.Support
{
    public static class ILocatorExtension
    {
        public static async Task ClearandFill(this ILocator locator, string inputvalue)
        {
            await locator.ClearAsync();
            await locator.FillAsync(inputvalue);

        }

        public static async Task WaitForElementVisibility(this ILocator locator)
        {
            await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        }

    }
}

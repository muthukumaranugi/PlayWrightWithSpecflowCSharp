using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayWrightWithSpecflowCSharp.PageObjects
{
    public class HeroKuappJSAlertsPage
    {
        private IPage page;
        public HeroKuappJSAlertsPage(IPage _page) => page = _page;

        ILocator resultMessage => page.Locator("p#result");
        string? alertMessage = null;

        public async Task AlertAction(string? alertActionValue,string? alertInputMessage)
        {
            await page.PauseAsync();
            page.Dialog += (_, dialog) =>
            {
                Console.WriteLine("Message in the alert:" + dialog.Message);
                alertMessage = dialog.Message;
                if (alertActionValue != null)
                {
                    if (alertActionValue.ToLower() == "accept")
                    {
                        dialog.AcceptAsync(alertInputMessage);
                    }

                    if (alertActionValue.ToLower() == "dismiss" || alertActionValue.ToLower() == "cancel")
                    {
                        dialog.DismissAsync();
                    }
                }                

            };
        }

        public void VerifyAlertMessage(string expectedAlertMessage)
        {
            Assert.That(alertMessage.Equals(expectedAlertMessage), "Actual alert message is not same as expected");
            Console.WriteLine("Asserted that actual alert message is same as expected");
        }

        public async Task ClickAlertButton(string alertButtonName)
        {
            ILocator pageAlertButton = page.Locator($"//button[text()='{alertButtonName}']");
            await pageAlertButton.ClickAsync();
        }

        public async Task VerifyResultMessage(string expectedResultMessage)
        {
            string actualResultMessage = await resultMessage.TextContentAsync();
            Assert.That(actualResultMessage.Equals(expectedResultMessage), $"Actual result message - '{actualResultMessage}' is not same as expected - '{expectedResultMessage}'");
            Console.WriteLine("Asserted that actual result is same as expected");
        }


    }
}

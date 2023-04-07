using PlayWrightWithSpecflowCSharp.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayWrightWithSpecflowCSharp.PageObjects
{
    public class HeroKuappIFramePage
    {
        private IPage page;
        public HeroKuappIFramePage(IPage _page) => page = _page;

        ILocator iframeFileMenuItem => page.Locator("//button[@role='menuitem']//*[text()='File']");
        ILocator iframeNewDocumentMenuItem => page.Locator("//*[@role='menuitem' and @title='New document']");
        IFrameLocator iframeEditorFrame => page.FrameLocator("//iframe[@id='mce_0_ifr']");
        ILocator iframeEditorInputBox => iframeEditorFrame.Locator("//*[@id='tinymce']");
        public async Task CreateNewDocument()
        {
            await iframeFileMenuItem.ClickAsync();
            await iframeNewDocumentMenuItem.ClickAsync();
            Console.WriteLine("Create new document");
        }

        public async Task InputDataIntheTextEditor(string inputData)
        {
            await iframeEditorInputBox.FillAsync(inputData);
            Console.WriteLine("Inputted the string in the iframe");
        }

    }
}

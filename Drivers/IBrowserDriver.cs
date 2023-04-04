using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayWrightWithSpecflowCSharp.Drivers
{
    public interface IBrowserDriver
    {
        public Task<IBrowser> InitializeBrowser(string featureName);
        public Task<IBrowserContext> InitializeBrowserContext(IBrowser browser, string featureName);
        public Task<IPage> InitializeBrowserPage(IBrowserContext browserContext);
        public Task NaviagateToAppURL(IPage webpage);
        public Task StartTracerInDriver(string featureName, IBrowserContext browserContext);
        public Task StopTracerInDriver(string featureName, IBrowserContext browserContext);
        public Task CloseBrowser(IPage page, IBrowserContext browserContext, IBrowser browser);

    }
}

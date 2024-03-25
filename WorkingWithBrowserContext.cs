using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
namespace PlaywrightTest01
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class WorkingWithBrowserContext: PageTest
    {

        [Test]
        public async Task StartingPlaywright()
        {
            await Page.GotoAsync("/login");

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("GitHub"));            
        }

        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions()
            {
                ColorScheme = ColorScheme.Light,
                ViewportSize = new()
                {
                    Width = 1920,
                    Height = 1080
                },
                BaseURL = "https://github.com",
            };
        }
    }
}

﻿using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
namespace PlaywrightTest01
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class PlaywrightAutomation01: PageTest
    {
         [SetUp]
        public async Task Setup()
        {
            await Context.Tracing.StartAsync(new()
            {
                Title = TestContext.CurrentContext.Test.ClassName + "." + TestContext.CurrentContext.Test.Name,
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        // How to enable debugging:
        // $env:PWDEBUG=1
        // How to see trace
        // .\playwright.ps1 show-trace
        // How to enable headless mode
        // $env:HEADED="1"
        
        [TearDown]
        public async Task TearDown()
        {
            Console.WriteLine($"Directory: {TestContext.CurrentContext.WorkDirectory}");
            // This will produce e.g.:
            // bin/Debug/net8.0/playwright-traces/PlaywrightTests.Tests.Test1.zip
            await Context.Tracing.StopAsync(new()
            {
                Path = Path.Combine(
                    TestContext.CurrentContext.WorkDirectory,
                    "playwright-traces",
                    $"{TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.Name}.zip"
                )
            });
        }

        [Test]
        public async Task StartingPlaywrightTest()
        {
            await Page.GotoAsync("https://playwright.dev");

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

            // create a locator
            var getStarted = Page.GetByRole(AriaRole.Link, new() { Name = "Get started" });

            // Expect an attribute "to be strictly equal" to the value.
            await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

            // Click the get started link.
            await getStarted.ClickAsync();

            // Expects the URL to contain intro.
            await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
            
        }

        [Test]
        public async Task LaunchBrowserIncognitoTest()
        {
 
            /*
            * Slightly changed usage due to the following error. 
            'IPlaywright' does not contain a definition for 'CreateAsync' and no accessible extension method 'CreateAsync' accepting a first argument of type 'IPlaywright' could be found (are you missing a using directive or an assembly reference?)CS1061
            */
            var brow = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions{Headless=false});
            var context = await brow.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://bing.com");
            await context.CloseAsync();
        }
    }
}

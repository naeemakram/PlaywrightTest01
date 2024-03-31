namespace PlaywrightTest01
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ContosoTests: PageTest
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
        public async Task SearchContosoStoreTest()
        {
            await Page.GotoAsync("https://cloudtesting.contosotraders.com/");

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Contoso Traders"));

            // create a locator
            var searchRole = Page.GetByRole(AriaRole.Textbox, new() { Name = "Search by product name or search by image" });
            
            await searchRole.FillAsync("Surface Laptop");

            await Page.GetByRole(AriaRole.Button, new(){Name= "iconimage"}).ClickAsync();
            
            await Expect(Page.GetByRole(AriaRole.Heading, new() {Name="Suggested Products List"})).ToBeVisibleAsync();
            // var searchPlaceHolder = Page.GetByPlaceholder("Search by product name or search by image");
            
            // Expect an attribute "to be strictly equal" to the value.
            // Expects the URL to contain intro.
            await Expect(Page).ToHaveURLAsync(new Regex("suggested-products-list"));
            
        }

        [Test]
        public async Task LaunchBrowserIncognitoTest()
        {
 
            /*
            * Slightly changed usage due to the following error. 
            'IPlaywright' does not contain a definition for 'CreateAsync' and no accessible extension method 'CreateAsync' accepting a first argument of type 'IPlaywright' could be found (are you missing a using directive or an assembly reference?)CS1061
            */
            // how to run just this test
            // dotnet test --filter Name~LaunchBrowserIncognitoTest
            var brow = await Playwright.Chromium.LaunchAsync();
            var context = await brow.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://bing.com");
            await context.CloseAsync();
        }

        

    }
}

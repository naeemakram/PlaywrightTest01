using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using NUnit.Framework;
using FluentAssertions;

namespace PlaywrightTests;

public class TestGitHubAPI : PlaywrightTest
{
    
    private IAPIRequestContext Request = null;

    [SetUp]
    public async Task SetUpAPITesting()
    {
        await CreateAPIRequestContext();
    }

    private async Task CreateAPIRequestContext()
    {
        var headers = new Dictionary<string, string>();

        Request = await this.Playwright.APIRequest.NewContextAsync(new() {
            // All requests we send go to this API endpoint.
            BaseURL = "https://www.deckofcardsapi.com",
            ExtraHTTPHeaders = headers,
        });
    }

    [TearDown]
    public async Task TearDownAPITesting()
    {
        await Request.DisposeAsync();
    }

    [Test]
    public async Task MakeADeckOfCardsGet()
    {
        var response = await Request.GetAsync("/api/deck/new/shuffle/?deck_count=1");
        response.Ok.Should().BeTrue();
        var jsonResponse = await response.JsonAsync();
        Console.WriteLine(jsonResponse);
    }

    [Test]
    public async Task MakeADeckOfCardsPost()
    {
        var postValues = new Dictionary<string, string>();
        postValues.Add("deck_count", "1");
        var response = await Request.PostAsync("/api/deck/new/shuffle", new() {DataObject = postValues});
        response.Ok.Should().BeTrue();
        var jsonResponse = await response.JsonAsync();
        Console.WriteLine(jsonResponse);
    }
}
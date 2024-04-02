using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;
using NUnit.Framework;
using FluentAssertions;
using Newtonsoft.Json.Linq;

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
        PrintResponse(JObject.Parse(jsonResponse.ToString()));
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

        PrintResponse(JObject.Parse(jsonResponse.ToString()));
    }

    //https://www.deckofcardsapi.com/api/deck/<<deck_id>>/draw/?count=2
    //https://www.deckofcardsapi.com/api/deck/new/draw/?count=2

    [Test]
    public async Task DrawFromNewDeckOfCards()
    {
        var responseNewDeck = await Request.GetAsync("/api/deck/new/shuffle/?deck_count=1");
        responseNewDeck.Ok.Should().BeTrue();
        var jsonResponse = await responseNewDeck.JsonAsync();
        Console.WriteLine(jsonResponse);
        
        var newDeckJSON = JObject.Parse(jsonResponse.ToString());
        System.Threading.Thread.Sleep(3000);// Deck of Cards is a free public API, we must let it breathe. 
        var drawResponsee = await Request.GetAsync($"/api/deck/{newDeckJSON.SelectToken(".deck_id")}/draw/?count=3");
        drawResponsee.Ok.Should().BeTrue();
        Console.WriteLine($"Status:{drawResponsee.Status}");
        
        var jsonDrawResponse = JObject.Parse((await drawResponsee.JsonAsync()).ToString());
        
        Console.WriteLine(jsonDrawResponse);

        (jsonDrawResponse.SelectToken(".cards") as JArray).Count.Should().Be(3);

        jsonDrawResponse.SelectToken(".remaining").Value<int>().Should().Be(49);

        /*
        //  Example: 
        var json = "{\"Name\":\"Alice\"}";
        var customer = JsonSerializer.Deserialize<object>(json);
        Console.WriteLine(customer); // Output: { Name = Alice }
        */
    }// A sample by Naeem Akram Malik - testautomationtv.com

    private void PrintResponse(JObject jsonObj) =>
        Console.WriteLine($"Result values: \r\nSuccess: {jsonObj.SelectToken(".success")}\r\ndeck_id: {jsonObj.SelectToken(".deck_id")}\r\nshuffled: {jsonObj.SelectToken(".shuffled")}\r\nremaining: {jsonObj.SelectToken(".remaining")}");
    
}
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace DadJokeExtension;

internal sealed partial class DadJokePage : ContentPage
{
    private readonly DadJokeForm _dadJokeForm;

    internal static readonly HttpClient Client = new();
    internal static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };
    internal string jokeMarkdown = string.Empty;

    public DadJokePage()
    {
        Icon = new("😜");
        Name = "Random Dad Joke";

        LoadContent();

        _dadJokeForm = new(jokeMarkdown);
    }

    public override IContent[] GetContent() => [_dadJokeForm];
    
    public void LoadContent()
    {
        IsLoading = true;

        string jokeFailure = "Awe snap! We couldn't load a joke.";

        var t = GetJokeAsync();
        t.ConfigureAwait(false);

        jokeMarkdown = !string.IsNullOrEmpty(t.Result?.Joke) ? t.Result.Joke : jokeFailure;

        IsLoading = false;
    }

    private async Task<DadJoke> GetJokeAsync()
    {
        try
        {
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Make a GET request to the icanhazdadjoke API
            var response = await Client
                .GetAsync("https://icanhazdadjoke.com/");
            response.EnsureSuccessStatusCode();

            // Read and deserialize the response JSON into a DadJoke object
            var responseBody = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<DadJoke>(responseBody, Options);

            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }

        return null;
    }
}

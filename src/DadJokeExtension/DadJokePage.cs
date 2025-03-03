using System;
using System.Diagnostics.CodeAnalysis;
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
	internal static MarkdownContent jokeContent = new();


	private const string jokeFailure = "Awe snap! We couldn't load a joke.";

	public DadJokePage()
	{
		IsLoading = true;

		var t = GetJokeAsync();
		t.ConfigureAwait(false);

		var jokeMarkdown = !string.IsNullOrEmpty(t.Result?.Joke) ? t.Result.Joke : jokeFailure;
		jokeContent = new MarkdownContent() { Body = $"## {jokeMarkdown}" };

		IsLoading = false;

		_dadJokeForm = new(jokeMarkdown);
	}

	public override IContent[] GetContent() => [jokeContent, _dadJokeForm];

	[RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.Deserialize<TValue>(String, JsonSerializerOptions)")]
	private static async Task<DadJoke?> GetJokeAsync()
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

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace DadJokeExtension;

internal sealed partial class DadJokePage : ContentPage
{
	internal static readonly HttpClient Client = new();
	internal static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

	private const string jokeFailure = "Awe snap! We couldn't load a joke.";
	
	private static readonly DadJokeForm _dadJokeForm = new(string.Empty);

	public DadJokePage()
	{
		Id = "com.baldbeardedbuilder.cmdpal.randomdadjoke";

		_dadJokeForm.RefreshCommand += RefreshContent;
	}

	public override IContent[] GetContent()
	{
		IsLoading = true;

		RefreshContent();

	  IsLoading = false;

		return [_dadJokeForm];
	}

	public static void RefreshContent()
	{
		var t = GetJokeAsync();
		t.ConfigureAwait(false);

		var joke = t.Result;
		_dadJokeForm.UseJoke(string.IsNullOrEmpty(joke) ? jokeFailure : joke);
	}

	private void RefreshContent(object sender, object? args)
	{
		RefreshContent();
		RaiseItemsChanged(1);
	}

	private static async Task<string?> GetJokeAsync()
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

			return data?.Joke;
		}
		catch (Exception e)
		{
			Console.WriteLine($"An error occurred: {e.Message}");
		}

		return null;
	}
}

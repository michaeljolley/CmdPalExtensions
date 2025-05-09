﻿using System.Text.Json.Serialization;

namespace CmdPalRandomDadJokeExtension;

public sealed class DadJoke
{
  [JsonPropertyName("id")]
  public string Id { get; set; } = "";

  [JsonPropertyName("joke")]
  public string Joke { get; set; } = "";

  [JsonPropertyName("status")]
  public int Status { get; set; }
}

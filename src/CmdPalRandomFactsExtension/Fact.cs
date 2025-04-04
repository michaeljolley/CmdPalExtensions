using System.Text.Json.Serialization;

namespace CmdPalRandomFactsExtension;

public sealed class Fact
{
  [JsonPropertyName("id")]
  public string Id { get; set; } = "";

  [JsonPropertyName("text")]
  public string Text { get; set; } = "";

  [JsonPropertyName("source")]
  public string Source { get; set; } = "";

  [JsonPropertyName("source_url")]
  public string SourceUrl { get; set; } = "";
}

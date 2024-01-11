using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Akedas.API.RouteModels
{

  public class ClientCredentials
  {
    [JsonPropertyName("client-id")]
    [FromHeader]
    public string? ClientId { get; set; }

    [JsonPropertyName("client-secret")]
    [FromHeader]
    public string? ClientSecret { get; set; }

  }
}

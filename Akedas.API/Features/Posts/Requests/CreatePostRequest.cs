using Akedas.API.Features.Posts.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Akedas.API.Features.Posts.Requests
{
  // CreatePostRequest =>  CreatePostResponse döndürür
  public class CreatePostRequest:IRequest<CreatePostResponse>
  {
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
  }
}

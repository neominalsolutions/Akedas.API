namespace Akedas.API.Features.Posts.Responses
{
  public class CreatePostResponse
  {
    public Guid PostId { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }

  }
}

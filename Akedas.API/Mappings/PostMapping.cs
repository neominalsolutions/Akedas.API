using Akedas.API.Data.Entities;
using Akedas.API.Features.Posts.Requests;
using AutoMapper;

namespace Akedas.API.Mappings
{
  public class PostMapping:Profile
  {
    public PostMapping()
    {
      // IoC gibi registeration dosyası hangi entity hangi dto mapplenecek kısmı
      CreateMap<CreatePostRequest, Post>();
    }
  }
}

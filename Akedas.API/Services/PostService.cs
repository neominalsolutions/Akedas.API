using Akedas.API.Data.Contexts;
using Akedas.API.Data.Entities;
using Akedas.API.Dtos;
using Akedas.API.Dtos.Posts;

namespace Akedas.API.Services
{
    public class PostService
  {
    private readonly AppDbContext appDbContext;

    public PostService(AppDbContext appDbContext)
    {
      this.appDbContext = appDbContext;
    }

    public List<PostGetDto> GetPosts()
    {
      return this.appDbContext.Posts.Select(a => new PostGetDto
      {
        Id = a.Id,
        Title = a.Title,
        Description = a.Description,
        Content = a.Content

      }).ToList();
    }

    public ApiResponse<Post> ApiResponseCreate(PostCreateDto request)
    {
      var entity = new Post(request.Title, request.Content);
      entity.Description = request.Description;

      this.appDbContext.Posts.Add(entity);
      this.appDbContext.SaveChanges();

      return new ApiResponse<Post>
      {
        Data = entity,
        SuccessMessage = "Kayıt Başarılı",
        Status = 201
      };
    }


    public void Update(Guid id,PostUpdateDto request)
    {
      var entity = this.appDbContext.Posts.Find(id);

      if(entity is not null)
      {
        entity.Content = request.Content;
        entity.Description = request.Description;
       
        this.appDbContext.Update(entity);
        this.appDbContext.SaveChanges();
      }
      else
      {
        throw new Exception("Entity Not Found");
      }
    }

    public Post Create(PostCreateDto request)
    {
      var entity = new Post(request.Title, request.Content);
      entity.Description = request.Description;

      this.appDbContext.Posts.Add(entity);
      this.appDbContext.SaveChanges();

      return entity;
    }

    public void Delete(Guid id)
    {
      if(!this.appDbContext.Posts.Any(x=> x.Id == id))
      {
        throw new Exception("Entity Not Found");
      }
      else
      {
        var entity = this.appDbContext.Posts.Find(id);
        this.appDbContext.Remove(entity);
        this.appDbContext.SaveChanges();
      }
    }


    public PostGetDto GetPostById(Guid id)
    {
      var entity = this.appDbContext.Posts.Find(id);

      if(entity is not null)
      {
        var dto = new PostGetDto
        {
          Id = entity.Id,
          Title = entity.Title,
          Content = entity.Content,
          Description = entity.Description
        };

        return dto;
      }
      else
      {
        throw new Exception("Entity Not Found");
      }

    }
  }
}

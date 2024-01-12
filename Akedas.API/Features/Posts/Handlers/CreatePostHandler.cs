using Akedas.API.Data.Contexts;
using Akedas.API.Data.Entities;
using Akedas.API.Features.Posts.Requests;
using Akedas.API.Features.Posts.Responses;
using AutoMapper;
using MediatR;

namespace Akedas.API.Features.Posts.Handlers
{
  public class CreatePostHandler : IRequestHandler<CreatePostRequest, CreatePostResponse>
  {
    // bu kısımda veri tabanı işlemleri için Repositorylere bağlanıyoruz.
    // şuan repository yok dbContext üzerinden işlem yapacağız.

    private readonly AppDbContext db;
    private readonly IMapper mapper;

    public CreatePostHandler(AppDbContext db, IMapper mapper) // DI
    {
      this.db = db;
      this.mapper = mapper;
    }

    public async Task<CreatePostResponse> Handle(CreatePostRequest request, CancellationToken cancellationToken)
    {
      var entity = mapper.Map<Post>(request);

      db.Posts.Add(entity);
      db.SaveChanges();

      var response = new CreatePostResponse { PostId = entity.Id, Title = entity.Title, CreatedAt = DateTime.Now };

      // Task based async yazılmış bir sonuç döndürmek için Task.FromResult kullanıyoruz.
      // Task.FromResult

      //var taks1 = new Task();
      //var task2 = new Task();

      //Task.WhenAll(taks1, tasks);

      return await Task.FromResult<CreatePostResponse>(response);
    }
  }
}

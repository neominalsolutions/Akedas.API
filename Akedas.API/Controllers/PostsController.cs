using Akedas.API.Data.Entities;
using Akedas.API.Dtos;
using Akedas.API.Dtos.Comments;
using Akedas.API.Dtos.Posts;
using Akedas.API.RouteModels;
using Akedas.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Akedas.API.Controllers
{
    [Route("api/[controller]")]
  [ApiController]
  public class PostsController : ControllerBase
  {
    private readonly PostService postService;

    public PostsController(PostService postService)
    {
      this.postService = postService;
    }

    // [FromQuery] ile verini url üzerinden alacağız zorunlu değildir.

    [HttpGet]
    public IActionResult Get([FromQuery] FilterRequest? filters) // List
    {
    //  var postDto = new PostDto();
    //  postDto.Title = "T-1";
    //  postDto.Content = "C-1";

    //  var options = new JsonSerializerOptions
    //  {
    //    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    //};

    //  var postJson = System.Text.Json.JsonSerializer.Serialize(postDto, options);
    //  var postDto2 = System.Text.Json.JsonSerializer.Deserialize<PostDto>(postJson, options);

      var response = this.postService.GetPosts();

      return Ok(response);
    }


    //{route} [FromRoute] dan gelen değerler required'dır yazılamdan olmaz.
    // id:int ile int tipinde
    [HttpGet("{id:Guid}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
      var response = postService.GetPostById(id);

      return Ok(response);
    }

    //[HttpGet("{name}")] // attribute routing yöntemi
    //public IActionResult GetByName(string name)
    //{
    //  return Ok("ByName");
    //}

    // Kötü Örnek

    [HttpPost("createBadRequest")]
    public ApiResponse<Post> Create2([FromBody] PostCreateDto request)
    {
      var response = this.postService.ApiResponseCreate(request);


      return response; 
    }

    [HttpPost]
    public IActionResult Create([FromBody] PostCreateDto request)
    {
      var response = this.postService.Create(request);


      return Created($"/api/posts/{response.Id}", response); // 201

      // Validasyon hatası varsa kendisi badrequest ayarlıyor.
    }

    [HttpPut("{id:Guid}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] PostUpdateDto request)
    {
        postService.Update(id, request); // Hata varsa 500 dönecek önü kesilecek.

        return NoContent(); // 204
      
    }

    // Nested Route işlemi.
    // api/posts/1/comments/2 PATCH
    // api/comments/2 PUT
    [HttpPatch("{postId:int}/comments/{commentId}")]
    public IActionResult ModifyComment([FromRoute] int postId, [FromRoute] string commentId, [FromBody] CommentDto request)
    {
      return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
      this.postService.Delete(id);

      

      return NoContent(); // 204
    }

    // Header üzerinden veri gönderme
    // Header object olduğunda problem veriyor
    [HttpGet("clientCredentials")]
    public IActionResult RequestFromHeader([FromHeader] string clientId)
    {
      return Ok();
    }

  }
}

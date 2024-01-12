using Akedas.API.Features.Posts.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Akedas.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PostsMediatorController : ControllerBase
  {

    private readonly IMediator mediator;

    public PostsMediatorController(IMediator mediator)
    {
      this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
    {
      // PostService.Create(request);
      // İlgili CreatePostHandler'a yönlendirir.
      // CreatePostRequest hangi handler'a gideceğini mediator üzerinden biliyor.
      // In-direct Communication sağladık. GSARP (InDirection)
      // mediator sayesinde SRP,OCP prensiplerini de uyguladık.
      var response = await this.mediator.Send(request);

      return Ok(response);
    }

  }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Akedas.API.Dtos.Comments;

namespace Akedas.API.Dtos.Posts
{
    public class PostGetDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("title")]
        [DefaultValue("Makale-1")]
        [Required(ErrorMessage = "Title boş geçilemez")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        [DefaultValue("Açıklama-1")]
        [MaxLength(200, ErrorMessage = "200 karakterden fazla girilemez")]
        public string? Description { get; set; }

        [RegularExpression(@"^[1-9]{1}[0-9]{9}[02468]{1}$", ErrorMessage = "TC No Hatalı")]
        public string IdentityNumber { get; set; }

        [JsonPropertyName("comments")]
        public List<CommentDto> Comments { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }



    }
}

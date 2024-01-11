using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Akedas.API.Dtos.Posts
{
    public class PostCreateDto
    {

        [JsonPropertyName("title")]
        [DefaultValue("Makale-1")]
        [Required(ErrorMessage = "Title boş geçilemez")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        [DefaultValue("Açıklama-1")]
        [MaxLength(200, ErrorMessage = "200 karakterden fazla girilemez")]
        public string? Description { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

    }
}

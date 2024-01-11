using System.ComponentModel.DataAnnotations;

namespace Akedas.API.Dtos.Comments
{
    public class CommentDto
    {

        [Required]
        public string Text { get; set; }

        [Required]
        public string CommentBy { get; set; }
    }
}

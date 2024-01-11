using System.Security.Principal;

namespace Akedas.API.Data.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string CommentBy { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        // FK olucak kendisi ilişkiden dolayı db de FK açıcak.
        //public string PostId { get; set; }


        // bi-directional
        // public Post Post { get; set; }


        public Comment(string text, string commentBy)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            Text = text.Trim();
            CommentBy = commentBy.Trim();
        }


    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace Akedas.API.Data.Entities
{
  // POCO => POJO Plain Old CLR Object
  // POCO nesneleri içlerinde herhangi bir bağımlılık barındırmazlar.
  // Column("")] EF Data Annotation yöntemi dışında Fluent API yöntemi ile OnModelCreating işleminde nesnelerin PK,FK,ColumnName,TableName,Index,MaxLength,Relations gibi tüm ayarları yapıalbiliyor.
  public class Post
    {
        public Guid Id { get; set; }

        // [Column("")]
        public string Title { get; set; }

        public string Content { get; set; }

        public string? Description { get; set; }

        public List<Comment> Comments { get; set; }


        public Post()
        {

        }

        public Post(string title, string content)
        {
            Id = Guid.NewGuid();
            Title = title.Trim();
            Content = content.Trim();
        }

    }
}

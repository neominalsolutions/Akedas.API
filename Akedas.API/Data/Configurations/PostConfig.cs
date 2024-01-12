using Akedas.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Akedas.API.Data.Configurations
{
  public class PostConfig : IEntityTypeConfiguration<Post>
  {
    public void Configure(EntityTypeBuilder<Post> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasIndex(x => x.Title).IsUnique(true); // Unique Index
      builder.Property(x => x.Title).IsRequired();
      builder.HasMany(x => x.Comments); // 1 to many
      builder.Property(x => x.Content).HasMaxLength(200).IsRequired();
    }
  }
}

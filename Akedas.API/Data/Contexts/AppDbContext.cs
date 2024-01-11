using Akedas.API.Data.Configurations;
using Akedas.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Akedas.API.Data.Contexts
{
  // Add-Migration First -o "Data/Migrations"

  public class AppDbContext:DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
      // DbContextOptions sınıfı ile hangi altyapıya bağlanacağımız addDbContext tanımında belirtiyoruz
    }

    // post ile comment arasında ilişki olduğu için comment post üzerinden dbye yansır. Comment için ayrıca bir dbset ihtiyacı yok
    // Commentlere direkt uygulama tarafında sorguyu atmayacak ise bu tipi program tarafında tanıtmaya gerek oluyor.
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      // optionsBuilder.UseSqlServer("");
      base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // 1. yöntem
      modelBuilder.Entity<Comment>(opt =>
      {
        opt.ToTable("PostComment"); // table
        opt.Property(x => x.CommentBy).IsRequired().HasMaxLength(20).HasColumnType("nvarchar"); // Required
        opt.HasKey(x => x.Id); // PK , int long auto incremental olarak çalışır
        opt.Property(x => x.Text).IsRequired().HasMaxLength(200).HasColumnName("CommentText");
       
      });


      // 2.Yöntem
      modelBuilder.ApplyConfiguration(new PostConfig());

      // 3.Yöntem Reflection ile okuma

      //var mapTypes = from t in typeof(AppDbContext).Assembly.GetTypes()
      //               where t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
      //               select t;

      //foreach (Type mapType in mapTypes)
      //{
      //  dynamic configInstance = Activator.CreateInstance(mapType);
      //  modelBuilder.ApplyConfiguration(configInstance);
      //}

      base.OnModelCreating(modelBuilder);
    }
  }
}

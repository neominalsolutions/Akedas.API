using Akedas.API.Data.Contexts;
using Akedas.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt =>
{
  opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
}); // controller içerisinde baþka servislere DI yapabilmek için bu servis eklenmek zorunda.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// servis olarak uygulamanýn IoC container swagger tanýttýk.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext service olarak uygulamaya tanýttýk

string dbProvider = builder.Configuration.GetSection("DBProvider").GetValue<string>("Default");

Console.WriteLine(dbProvider);

builder.Services.AddDbContext<AppDbContext>(opt =>
{
  if(dbProvider == "MSSql")
  {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConn"));
  }
  else if (dbProvider == "Postgres")
  {
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConn"));
  }
});

// Scope serviceler web istekleri için kullanýlýyor.
builder.Services.AddScoped<PostService>();

// CROSS ORIGIN REQUEST SHARING ayarý
// Uygulama yetkilendirmesi
builder.Services.AddCors(opt =>
{
  opt.AddDefaultPolicy(policy =>
  {
    // spesifik durumlarýn yöntemi
    //policy.WithOrigins("www.a.com","www.b.com");
    //policy.WithMethods("GET,POST,PUT");
    //policy.WithHeaders("X-Client");

    policy.AllowAnyOrigin();
    policy.AllowAnyMethod();
    policy.AllowAnyHeader();
  });
});


var app = builder.Build(); // servisler iþlensin diye build edilir.


app.UseCors(); // cors middleware cors ayarlarýný spa uygulama için aç

// web application instance üzerinde middleware çalýþýr.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger(); // OPEN API için gerekli ara yazýlýmlarýn yüklenmesi
  app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // http isteklerini https yönlendirir.

app.UseAuthorization(); // yetkilendirme yönetimi, Authorize attribute kullanýmý

app.MapControllers(); // gelen isteklerin controllerlara yönlendirilmesini saðlayan middleware

app.Run();

using Akedas.API.Data.Contexts;
using Akedas.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt =>
{
  opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
}); // controller i�erisinde ba�ka servislere DI yapabilmek i�in bu servis eklenmek zorunda.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// servis olarak uygulaman�n IoC container swagger tan�tt�k.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext service olarak uygulamaya tan�tt�k

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

// Scope serviceler web istekleri i�in kullan�l�yor.
builder.Services.AddScoped<PostService>();

// CROSS ORIGIN REQUEST SHARING ayar�
// Uygulama yetkilendirmesi
builder.Services.AddCors(opt =>
{
  opt.AddDefaultPolicy(policy =>
  {
    // spesifik durumlar�n y�ntemi
    //policy.WithOrigins("www.a.com","www.b.com");
    //policy.WithMethods("GET,POST,PUT");
    //policy.WithHeaders("X-Client");

    policy.AllowAnyOrigin();
    policy.AllowAnyMethod();
    policy.AllowAnyHeader();
  });
});


var app = builder.Build(); // servisler i�lensin diye build edilir.


app.UseCors(); // cors middleware cors ayarlar�n� spa uygulama i�in a�

// web application instance �zerinde middleware �al���r.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger(); // OPEN API i�in gerekli ara yaz�l�mlar�n y�klenmesi
  app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // http isteklerini https y�nlendirir.

app.UseAuthorization(); // yetkilendirme y�netimi, Authorize attribute kullan�m�

app.MapControllers(); // gelen isteklerin controllerlara y�nlendirilmesini sa�layan middleware

app.Run();

using Akedas.API.Data.Contexts;
using Akedas.API.Middlewares;
using Akedas.API.Services;
using Azure.Core;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

#region EFCore

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

#endregion

#region Cors

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

#endregion

#region Mediator

// auto-handler register with reflection.
builder.Services.AddMediatR(config =>
{
  // bu proje i�erisindeki t�m handlerlar� serviceleri tan�t.
  config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});

//builder.Services.AddScoped<AHandler>();

#endregion

#region AutoMapper

// Reflection ile t�m Profile t�reyen nesneleri bulur. Bunlar�n programa service olarak tan�t�lmas�n� sa�lar.
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

#endregion

#region FluentValidation

// uygulama b�t�n validatorlar�n reflection ile tan�t�lmas�
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddFluentValidationAutoValidation(); // mvc art�k kendi validationlar�n� kullanmas�n bunun yerine validator nesnelerini kullans�n.
builder.Services.AddFluentValidationClientsideAdapters(); // json format�nda validasyon hata ��kt�s�

#endregion


#region Middlewares
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddTransient<ResponseBodyReadMiddleware>();
#endregion 

var app = builder.Build(); // servisler i�lensin diye build edilir.

app.UseMiddleware<ResponseBodyReadMiddleware>();

// app.UseCors(); // cors middleware cors ayarlar�n� spa uygulama i�in a�

// web application instance �zerinde middleware �al���r.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger(); // OPEN API i�in gerekli ara yaz�l�mlar�n y�klenmesi
  app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // http isteklerini https y�nlendirir.

app.UseAuthorization(); // yetkilendirme y�netimi, Authorize attribute kullan�m�

// uygulamaya middleware tan�tt�k.
app.UseMiddleware<ExceptionHandlingMiddleware>();


/*
app.Use(async (context, next) =>
{
  if(context.Request.Method == HttpMethod.Post.ToString())
  {

  }

  Console.WriteLine("IsAuthenticated" + context.User.Identity.IsAuthenticated);

  Console.WriteLine("request Path, Method" + context.Request.Path + context.Request.Method );


  // context => HttpContext
  await next(); // bir sonraki middleware i�i devretmek.
  Console.WriteLine("Response" + context.Response.StatusCode);

  // string bodyContent = new StreamReader(context.Response.Body).ReadToEnd();

  // Console.WriteLine($"bodyContent: {bodyContent}");

  await context.Response.WriteAsJsonAsync(new { message = "myMessage" });
  // iste�i sonland�r ekrana json ��kt� yazd�r.
});

*/

app.MapControllers(); // gelen isteklerin controllerlara y�nlendirilmesini sa�layan middleware



app.Run(); // K�sa devre middleware, buradan sonra sunucu response d�ner. bu rundan sonra i� bir ara yaz�l�m �al��amaz.

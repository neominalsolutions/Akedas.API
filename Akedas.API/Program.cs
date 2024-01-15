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
}); // controller içerisinde baþka servislere DI yapabilmek için bu servis eklenmek zorunda.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// servis olarak uygulamanýn IoC container swagger tanýttýk.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext service olarak uygulamaya tanýttýk

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

// Scope serviceler web istekleri için kullanýlýyor.
builder.Services.AddScoped<PostService>();

#endregion

#region Cors

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

#endregion

#region Mediator

// auto-handler register with reflection.
builder.Services.AddMediatR(config =>
{
  // bu proje içerisindeki tüm handlerlarý serviceleri tanýt.
  config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});

//builder.Services.AddScoped<AHandler>();

#endregion

#region AutoMapper

// Reflection ile tüm Profile türeyen nesneleri bulur. Bunlarýn programa service olarak tanýtýlmasýný saðlar.
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

#endregion

#region FluentValidation

// uygulama bütün validatorlarýn reflection ile tanýtýlmasý
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddFluentValidationAutoValidation(); // mvc artýk kendi validationlarýný kullanmasýn bunun yerine validator nesnelerini kullansýn.
builder.Services.AddFluentValidationClientsideAdapters(); // json formatýnda validasyon hata çýktýsý

#endregion


#region Middlewares
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddTransient<ResponseBodyReadMiddleware>();
#endregion 

var app = builder.Build(); // servisler iþlensin diye build edilir.

app.UseMiddleware<ResponseBodyReadMiddleware>();

// app.UseCors(); // cors middleware cors ayarlarýný spa uygulama için aç

// web application instance üzerinde middleware çalýþýr.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger(); // OPEN API için gerekli ara yazýlýmlarýn yüklenmesi
  app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // http isteklerini https yönlendirir.

app.UseAuthorization(); // yetkilendirme yönetimi, Authorize attribute kullanýmý

// uygulamaya middleware tanýttýk.
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
  await next(); // bir sonraki middleware iþi devretmek.
  Console.WriteLine("Response" + context.Response.StatusCode);

  // string bodyContent = new StreamReader(context.Response.Body).ReadToEnd();

  // Console.WriteLine($"bodyContent: {bodyContent}");

  await context.Response.WriteAsJsonAsync(new { message = "myMessage" });
  // isteði sonlandýr ekrana json çýktý yazdýr.
});

*/

app.MapControllers(); // gelen isteklerin controllerlara yönlendirilmesini saðlayan middleware



app.Run(); // Kýsa devre middleware, buradan sonra sunucu response döner. bu rundan sonra iç bir ara yazýlým çalýþamaz.

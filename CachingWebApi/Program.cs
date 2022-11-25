using CachingWebApi.Data;
using CachingWebApi.Interfaces;
using CachingWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEntityFrameworkSqlServer()
    .AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("SampleDbConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Drivers API",
        Version = "v1",
        Description = "An API to perform work with Redis",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Anto Kharchenko",
            Email = "gumb1t97@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/anton-kharchenko-898870195/"),
        },
        License = new OpenApiLicense
        {
            Name = "Drivers API LICX",
            Url = new Uri("https://example.com/license"),
        },

    });
});
    
    builder.Services.AddTransient<ICacheService, CacheService>();

    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
using MongoDbCrud.Api.Models;
using MongoDbCrud.Api.Services;
using MongoDbCrud.Api.Middleware;
using MongoDbCrud.Api.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var databaseSettings = new DatabaseSettings
{
    ConnectionString = builder.Configuration["DatabaseSettings:ConnectionString"] 
        ?? "mongodb://localhost:27017",
    DatabaseName = builder.Configuration["DatabaseSettings:DatabaseName"] 
        ?? "ProductDb",
    CollectionName = builder.Configuration["DatabaseSettings:CollectionName"] 
        ?? "Products"
};

builder.Services.AddSingleton(databaseSettings);
builder.Services.AddSingleton<IProductService, ProductService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
        options.RoutePrefix = string.Empty; 
    });
    
    using (var scope = app.Services.CreateScope())
    {
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        await SeedData.SeedDatabase(productService);
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

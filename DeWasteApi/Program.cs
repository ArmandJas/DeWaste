using DeWasteApi.Data;
using DeWasteApi.Interceptors;
using DeWasteApi.Middleware;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMvc(c => c.Conventions.Add(new ApiExplorerIgnores()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ErrorHandlingMiddleware>();
builder.Services.AddDbContext<DeWasteDbContext>();



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();


app.MapControllers();

app.Run();


//try to get intercepted
var db = app.Services.GetService<DeWasteDbContext>();
var items = db.item.ToList();


public class ApiExplorerIgnores : IActionModelConvention
{
    public void Apply(ActionModel action)
    {
        if (action.Controller.ControllerName.Equals("Pwa"))
            action.ApiExplorer.IsVisible = false;
    }
}
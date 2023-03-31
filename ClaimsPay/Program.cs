using ClaimsPay.Filters;
using ClaimsPay.Modules;
using ClaimsPay.Modules.ClaimsPay.Models.Comman_Model;
using ClaimsPay.Shared;
using FluentValidation;
using NLog;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", option =>
    option.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );
});

builder.Services.RegisterModules();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IValidator<RestData>, ModelValidator>();

AppConfig.configuration = builder.Configuration;

var app = builder.Build();


app.MapEndpoints();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.Run();


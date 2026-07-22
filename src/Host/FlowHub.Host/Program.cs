using FlowHub.Modules.Identity.Infrastructure;
using FlowHub.Modules.Identity.Presentation;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FlowHub",
        Version = "v1",
        Description = "API documentation for FlowHub project",
    });
});

//Identity project
builder.Services.AddIdentityInfrastructure(builder.Configuration);

var app = builder.Build();

//Identity project
app.AddIdentityPresentation();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlowHub v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.Run();

using FlowOps.API.ExceptionHandling;
using FlowOps.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddInfrastructure(builder.Configuration);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "FlowOps API v1");

        options.DocumentTitle = "FlowOps API";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

//if (app.Environment.IsDevelopment())
//{
//    app.MapGet("/api/test/error", () =>
//    {
//        throw new InvalidOperationException(
//            "Testing global exception handling");
//    });
//}

app.Run();

using System.Linq;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Api.Middlewares;
using Pedidos.Application;
using Pedidos.Infrastructure;
using Pedidos.Infrastructure.Persistence;
using Reporting.Projections;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPedidosApplication();
builder.Services.AddPedidosInfrastructure(builder.Configuration);
builder.Services.AddReportingProjections(builder.Configuration);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(ms => ms.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>());

            return new BadRequestObjectResult(new { Errors = errors });
        };
    });

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var pedidosContext = scope.ServiceProvider.GetRequiredService<PedidosDbContext>();
    pedidosContext.Database.Migrate();

    var reportingContext = scope.ServiceProvider.GetRequiredService<Reporting.Projections.Persistence.ReportingDbContext>();
    reportingContext.Database.Migrate();
}

app.UseJsonExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

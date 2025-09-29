using Microsoft.EntityFrameworkCore;
using Reporting.Api.Middlewares;
using Reporting.Projections;
using Reporting.Projections.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReportingProjections(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ReportingDbContext>();
    context.Database.Migrate();
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

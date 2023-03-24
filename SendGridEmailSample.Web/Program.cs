using SendGridEmailSample.Application;
using SendGridEmailSample.Infrastructure;
using SendGridEmailSample.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var allwedOrigins = builder.Configuration["Cors:AllowedOrigins"].Split(",");
builder.Services.AddCors(options =>
{
    options.AddPolicy("defaultCorsPolicy", policy =>
    {
        policy.WithOrigins(allwedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("defaultCorsPolicy");

app.UseAuthorization();

app.MapControllers();
app.MapHub<EmailStatusChangeEventHub>("/email-status-change-event-hub");
app.Run();

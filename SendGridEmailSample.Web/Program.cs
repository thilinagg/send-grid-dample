using SendGridEmailSample.Application;
using SendGridEmailSample.Infrastructure;
using SendGridEmailSample.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("defaultCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200", "https://stplazasendgridtest.z6.web.core.windows.net")
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

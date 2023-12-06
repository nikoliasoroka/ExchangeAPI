using ExchangeAPI;
using ExchangeAPI.Modules.Identity.WebAPI;
using ExchangeAPI.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBootstrapperServices();

builder.Services.AddShared(builder.Configuration);

builder.Services.AddIdentityModule(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseShared();

app.UseIdentityModule();

app.MapControllers();

app.Run();
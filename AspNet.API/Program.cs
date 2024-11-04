using AspNet.Core.Extensions;
using AspNet.Data;
using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog();

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.ConfigureRateLimiting();

builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddAuthentication();

builder.Services.AddCors(o =>
{
	o.AddPolicy("AllowAll", builder =>
		builder.AllowAnyOrigin()
		.AllowAnyMethod()
		.AllowAnyHeader());
});

builder.Services.ConfigureAutoMapper();

builder.Services.ConfigureSwaggerDoc();

builder.Services.AddControllers(/*config => {
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
                {
                    Duration = 120
                });
            }*/).AddNewtonsoftJson(op =>
			op.SerializerSettings.ReferenceLoopHandling =
				Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.ConfigureVersioning();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
	string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
	c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "MyApplicationName API");
});

app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseResponseCaching();
app.UseHttpCacheHeaders();
//app.UseIpRateLimiting();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

app.Services
   .CreateScope()
   .ServiceProvider
   .GetRequiredService<DatabaseContext>()
   .Database.Migrate();

Log.Logger = new LoggerConfiguration()
				.WriteTo.File(
					path: "logs\\log-.txt",
					outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
					rollingInterval: RollingInterval.Day,
					restrictedToMinimumLevel: LogEventLevel.Information
				).CreateLogger();
try
{
	Log.Information("Aplicação rodando");
	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Erro ao iniciar aplicação");
}
finally
{
	Log.CloseAndFlush();
}
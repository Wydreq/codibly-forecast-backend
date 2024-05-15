using codibly_forecast;
using codibly_forecast.Middleware;
using codibly_forecast.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


builder.Services.AddControllers(options => options.Filters.Add<HttpMethodFilter>());
builder.Services.AddAutoMapper(typeof(ForecastMappingProfile));
// builder.Services.AddScoped<IForecastService, ForecastService>();
builder.Services.AddHttpClient<IForecastService, ForecastService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendClient",
        builder => builder.AllowAnyMethod().AllowAnyHeader().WithOrigins(configuration["AllowedOrigins"])
            .AllowCredentials());
});

var app = builder.Build();


app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("FrontendClient");
app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using NotenTool.API.IoC;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

namespace NotenTool.API;

internal class Program
{
    internal static IApiModule ApiModule { get; set; } = new ApiModule(GetConnectionString());

    public static string GetConnectionString()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        return configuration.GetConnectionString("DefaultConnection");
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        LoadDependencies(builder.Services, builder);
        
        builder.Services.AddHttpContextAccessor();
        // Add services to the container.
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddInMemoryTokenCaches();
        
        builder.Services.AddAuthorization(options => { options.FallbackPolicy = options.DefaultPolicy; });

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                build =>
                {
                    build.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173").AllowCredentials();
                    build.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://127.0.0.1:5173").AllowCredentials();
                });
        });


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddApiVersioning(options =>
        {
            // Default API versioning configs
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "NotenTool API", Version = "v1" });
        });


        var app = builder.Build();


        app.UseCors();
        app.UseApiVersioning();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.Run();
    }

    internal static void LoadDependencies(IServiceCollection services, WebApplicationBuilder builder) =>
        ApiModule.RegisterDependencies(services, builder);
}
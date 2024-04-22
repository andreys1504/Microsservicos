using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Gateway.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        EnvironmentSettings environmentSettings = BuildApplication(builder);

        WebApplication app = builder.Build();
        PostBuildApplication(app, environmentSettings);

        app.Run();
    }


    //
    private static EnvironmentSettings BuildApplication(WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

        EnvironmentSettings environmentSettings = Base.PresentationLayer.Build.ObjectEnvironmentSettings.Create(
           builder.Configuration,
           currentProject: "Poc.Microsservicosv2.Gateway.Api",
           currentProjectShortName: "Gateway.Api",
           currentContext: null,
           applicationLayer: null,
           domainLayer: null);

        builder.Services.AddSingleton(environmentSettings);
        builder.Services.AddOcelot(builder.Configuration);
        builder.Services.AddSwaggerForOcelot(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        return environmentSettings;
    }

    private static void PostBuildApplication(WebApplication app, EnvironmentSettings environmentSettings)
    {
        if (environmentSettings.CurrentEnvironment != EnvironmentApp.Production)
        {
            app.UseStaticFiles();
            app.UseSwaggerForOcelotUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.UseOcelot().Wait();
    }
}
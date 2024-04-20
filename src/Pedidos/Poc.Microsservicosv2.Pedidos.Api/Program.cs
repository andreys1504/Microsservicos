using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Pedidos.Api;

public sealed class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        EnvironmentSettings environmentSettings = BuildApplication(builder);

        WebApplication app = builder.Build();
        PostBuildApplication(app, environmentSettings);

        app.Run();
    }

    //
    private static EnvironmentSettings BuildApplication(WebApplicationBuilder builder)
    {
        EnvironmentSettings environmentSettings = Ioc.ObjectEnvironmentSettings.Create(
            builder.Configuration,
            currentProject: "Poc.Microsservicosv2.Pedidos.Api",
            currentProjectShortName: "Pedidos.Api");
        Dependencies.Register(builder.Services, environmentSettings);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return environmentSettings;
    }

    private static void PostBuildApplication(WebApplication app, EnvironmentSettings environmentSettings)
    {
        if (environmentSettings.CurrentEnvironment != EnvironmentApp.Production)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        Ioc.PresentationLayerDependencies.UseAuthRequestsBetweenApis(app);
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}

using Microsoft.Extensions.Configuration;
using Poc.Microsservicosv2.Base.Settings.Authentication;
using Poc.Microsservicosv2.Base.Settings.MessageBroker;
using Poc.Microsservicosv2.Base.Settings.RabbitMq;
using Poc.Microsservicosv2.Base.Settings.ServicesInMicrosservicosv2;

namespace Poc.Microsservicosv2.Base.Settings;

public sealed class EnvironmentSettings
{
    public EnvironmentSettings(
        string currentEnvironment,
        string currentProject,
        string currentProjectShortName,
        string currentContext,
        string applicationLayer,
        string domainLayer)
    {
        currentEnvironment = currentEnvironment.ToLower();
        EnvironmentApp? environment = EnvironmentsApp.GetEnvironments.FirstOrDefault(env_ => env_.ToString().ToLower() == currentEnvironment);
        ArgumentNullException.ThrowIfNull(environment, $"Ambiente \"{CurrentEnvironment}\" inválido");

        CurrentEnvironment = environment.Value;
        CurrentProject = currentProject;
        CurrentProjectShortName = currentProjectShortName;
        CurrentContext = currentContext;
        ApplicationLayer = applicationLayer;
        DomainLayer = domainLayer;
    }

    public EnvironmentApp CurrentEnvironment { get; set; }
    public string CurrentProject { get; set; }
    public string CurrentProjectShortName { get; set; }
    public string CurrentContext { get; set; }
    public string ApplicationLayer { get; set; }
    public string DomainLayer { get; set; }
    public string DatabaseConnectionString { get; set; }
    public RabbitMqSettings RabbitMq { get; set; } = new RabbitMqSettings();
    public MessageBrokerSettings MessageBroker { get; set; } = new MessageBrokerSettings();
    public IList<ServicesInMicrosservicosv2Settings> ServicesInMicrosservicosv2 { get; set; } = [];
    public AuthenticationSettings Authentication { get; set; } = new AuthenticationSettings();

    public void SetOtherSettings(IConfiguration configuration)
    {
        //if (string.IsNullOrWhiteSpace(SqlServerConnectionString))
        //    SqlServerConnectionString = ;
    }
}

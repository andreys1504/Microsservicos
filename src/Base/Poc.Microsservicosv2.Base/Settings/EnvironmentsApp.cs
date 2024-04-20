namespace Poc.Microsservicosv2.Base.Settings;

public enum EnvironmentApp
{
    Local,
    Development,
    Production
}

public static class EnvironmentsApp
{
    public static IList<EnvironmentApp> GetEnvironments
    {
        get
        {
            return
                [
                    EnvironmentApp.Local,
                    EnvironmentApp.Development,
                    EnvironmentApp.Production
                ];
        }
    }

    public static IList<(EnvironmentApp, string)> GetEnvironmentShortName
    {
        get
        {
            return new List<(EnvironmentApp, string)>
                {
                    (EnvironmentApp.Local, "local"),
                    (EnvironmentApp.Development, "dev"),
                    (EnvironmentApp.Production, "prod")
                };
        }
    }
}

namespace KeryxNews.Infrastructure.Persistence.Configuration;

public class EfCoreSettings
{
    public bool EnableSensitiveDataLogging { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public int CommandTimeout { get; set; }
}
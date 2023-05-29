using Microsoft.Extensions.Configuration;

namespace HM_WorkingWithDB
{
    public class ConfigSettingsManager
    {
        IConfiguration _configuration;
        public ConfigSettingsManager() 
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
            _configuration = builder.Build();
        }
        public string GetConnectionString()
        {
            //const string connectionString = "Host=localhost;Username=otus5;Password=otus5;Database=otus5";
            var result = _configuration.GetConnectionString("DefaultConnectionString");
            return result ?? throw new NullReferenceException("Can't get this connection string");
        }
    }
}

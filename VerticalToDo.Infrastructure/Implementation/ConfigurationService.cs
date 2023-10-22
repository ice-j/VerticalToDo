using Microsoft.Extensions.Configuration;
using VerticalToDo.Abstractions;

namespace VerticalToDo.Infrastructure.Implementation
{
    public class ConfigurationService : IConfigurationService
    {
        IConfiguration _configuration { get; set; }

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration.GetConnectionString("VerticalToDo");

        public string AuthTokenLifetimeInSeconds => _configuration.GetSection("AppSettings")[nameof(AuthTokenLifetimeInSeconds)];
    }
}

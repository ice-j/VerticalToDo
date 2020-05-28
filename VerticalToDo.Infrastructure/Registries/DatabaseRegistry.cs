using Lamar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VerticalToDo.Core;

namespace VerticalToDo.Infrastructure.Registries
{
    public class DatabaseRegistry : ServiceRegistry
    {
        public DatabaseRegistry()
        {
            this.AddDbContext<VerticalToDoDbContext>((options) =>
            {
                var config = Find(x => x.ServiceType == typeof(IConfiguration)).ImplementationInstance as IConfiguration;
                options.UseSqlServer(config.GetConnectionString("VerticalToDo"));
            });
        }
    }
}

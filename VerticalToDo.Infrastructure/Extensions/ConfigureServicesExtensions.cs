using Lamar;
using System;
using System.Collections.Generic;
using System.Text;
using VerticalToDo.Infrastructure.Registries;

namespace VerticalToDo.Infrastructure.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static ServiceRegistry SetupRegistries(this ServiceRegistry services)
        {
            services.IncludeRegistry<MediatrRegistry>();
            services.IncludeRegistry<AbstractionsRegistry>();
            services.IncludeRegistry<DatabaseRegistry>();
            return services;
        }
    }
}

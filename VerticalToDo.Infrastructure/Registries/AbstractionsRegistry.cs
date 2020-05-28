using Lamar;
using VerticalToDo.Abstractions;
using VerticalToDo.Infrastructure.Implementation;

namespace VerticalToDo.Infrastructure.Registries
{
    public class AbstractionsRegistry : ServiceRegistry
    {
        public AbstractionsRegistry()
        {
            For<IConfigurationService>().Use<ConfigurationService>().Singleton();
        }
    }
}

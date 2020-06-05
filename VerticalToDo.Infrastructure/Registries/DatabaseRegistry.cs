using Lamar;
using Microsoft.Extensions.Configuration;
using VerticalToDo.Core;

namespace VerticalToDo.Infrastructure.Registries
{
    public class DatabaseRegistry : ServiceRegistry
    {
        public DatabaseRegistry()
        {
            For<VerticalToDoDbContext>().Use(x =>
            {
                var c = x.GetInstance<IConfiguration>();
                return new VerticalToDoDbContext(c.GetConnectionString("VerticalTodo"));
            }).Scoped();
            
        }
    }
}

using Lamar;
using Microsoft.EntityFrameworkCore;
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
                var dbc = new VerticalToDoDbContext(c.GetConnectionString("VerticalToDo"));
                dbc.Database.EnsureCreated();
                //dbc.Database.Migrate();
                return dbc;
            }).Scoped();
            
        }
    }
}

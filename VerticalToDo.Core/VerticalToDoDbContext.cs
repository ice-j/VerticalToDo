using Microsoft.EntityFrameworkCore;
using VerticalToDo.Core.Features.Accounts;
using VerticalToDo.Core.Features.ToDos;

namespace VerticalToDo.Core
{
    public class VerticalToDoDbContext : DbContext
    {
        private readonly string _connectionString;

        public VerticalToDoDbContext()
        {

        }

        public VerticalToDoDbContext(DbContextOptions<VerticalToDoDbContext> options) : base(options)
        {

        }

        public VerticalToDoDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<ToDo> ToDos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}

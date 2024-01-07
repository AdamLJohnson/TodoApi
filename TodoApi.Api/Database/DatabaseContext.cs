using TodoApi.Api.Todos;

namespace TodoApi.Api.Database
{
    //Entity Framework Core DatabaseContext for the Todo domain object
    public class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DatabaseContext(Microsoft.EntityFrameworkCore.DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public Microsoft.EntityFrameworkCore.DbSet<Todo> Todos { get; set; }
    }
}

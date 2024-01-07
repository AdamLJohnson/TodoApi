using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Api.Database;
using Xunit;
using Testcontainers.PostgreSql;

namespace TodoApi.Api.Tests.Integration
{
    [CollectionDefinition("TodoApiCollection")]
    public class TodoApiCollection : ICollectionFixture<TodoApiFactory>
    {
    }

    public sealed class TodoApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithDatabase("TodoDb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                //Remove all the EF Core registrations
                var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(DatabaseContext));
                if (context != null)
                {
                    services.Remove(context);
                    var options = services.Where(r => (r.ServiceType == typeof(DbContextOptions))
                                                      || (r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))).ToArray();
                    foreach (var option in options)
                    {
                        services.Remove(option);
                    }
                }

                //Add Postgres DB
                services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(_dbContainer.GetConnectionString()));
            });
        }
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            var dbInitializer = new DatabaseInitializer(_dbContainer.GetConnectionString());
            await dbInitializer.InitializeAsync();
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.DisposeAsync().AsTask();
        }
    }
}

using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;

namespace TaskManager.Database
{
    public class MigrationRunner
    {
        private readonly string _connectionString;

        public MigrationRunner(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task RunAsync()
        {
            // Add database migrations
            var serviceProvider = CreateServices();

            // Create a scope for the service provider to ensure proper disposal
            using var scope = serviceProvider.CreateScope();
            await EnsureDatabaseCreatedAsync();
            UpdateDatabase(scope.ServiceProvider);
        }

        /// <summary>
        /// Configure the dependency injection services.
        /// </summary>
        private IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLServer support to FluentMigrator
                    .AddSqlServer2016()
                    .WithGlobalConnectionString(_connectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(MigrationRunner).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }

        /// <summary>
        /// Ensure that the database is created
        /// </summary>
        private async Task EnsureDatabaseCreatedAsync()
        {
            var builder = new SqlConnectionStringBuilder(_connectionString);
            var databaseName = builder.InitialCatalog;

            if (string.IsNullOrEmpty(databaseName))
                throw new InvalidOperationException("Connection string must specify an InitialCatalog/Database.");

            // Switch to master database to create the new database if it does not exist
            builder.InitialCatalog = "master";

            await using var connection = new SqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            // Check if the database exists and create it if it does not
            command.CommandText = $"IF DB_ID(N'{databaseName}') IS NULL CREATE DATABASE [{databaseName}]";
            await command.ExecuteNonQueryAsync();
        }
    }
}

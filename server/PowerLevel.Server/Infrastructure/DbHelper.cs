namespace PowerLevel.Server.Infrastructure;

using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Xdxd.DotNet.Postgres;

public static class DbFactory
{
    public static TracerProvider CreateTracerProvider()
    {
        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("powerlevel"))
            .SetSampler(new AlwaysOnSampler())
            .AddNpgsql()
            .AddConsoleExporter()
            .Build();

        return tracerProvider;
    }

    /// <summary>
    /// Creates a new `NpgsqlConnection` connection.
    /// </summary>
    public static NpgsqlConnection CreateConnection(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Enlist = false, // Turn this off in order to save some perf. It disables the support for `TransactionScope`.
            IncludeErrorDetail = true,
        };

        return new NpgsqlConnection(builder.ToString());
    }
}

public class DbService : DbService<DbPocos>, IDbService
{
    public DbService(NpgsqlConnection dbConnection) : base(dbConnection) { }
}

public interface IDbService : IDbService<DbPocos> { }

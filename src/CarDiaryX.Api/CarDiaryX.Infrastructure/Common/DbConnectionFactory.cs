using CarDiaryX.Infrastructure.Common.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CarDiaryX.Infrastructure.Common
{
    internal class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string dbConnectionString;

        public DbConnectionFactory(IConfiguration configuration)
            => this.dbConnectionString = configuration.GetDefaultConnectionString();

        public SqlConnection GetConnection
            => new (this.dbConnectionString);
    }
}

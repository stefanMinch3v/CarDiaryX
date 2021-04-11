using Microsoft.Data.SqlClient;

namespace CarDiaryX.Infrastructure.Common
{
    internal interface IDbConnectionFactory
    {
        SqlConnection GetConnection { get; }
    }
}

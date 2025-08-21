using System.Data;

namespace rp_challenge.Infraestructure.Data
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
}

using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace MusicCollection2017.DAL
{
    public class MusicConfiguration : DbConfiguration
    {
        public MusicConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}




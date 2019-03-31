using System.Data.SqlClient;
using System.Web.Configuration;

namespace NBL.DAL
{
    public class DbGateway
    {
        private SqlConnection connectionObj;

        private SqlCommand commandObj;

        public DbGateway()
        {
            string connectionString =
                WebConfigurationManager.ConnectionStrings["UniversalBusinessSolutionDbConnectionString"]
                    .ConnectionString;
            connectionObj = new SqlConnection(connectionString);
            commandObj = new SqlCommand();
        }

        public SqlConnection ConnectionObj
        {
            get
            {
                return connectionObj;
            }


        }

        public SqlCommand CommandObj
        {
            get
            {
                commandObj.Connection = connectionObj;
                return commandObj;
            }

        }
    }
}
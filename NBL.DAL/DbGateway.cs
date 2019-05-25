using System.Data.SqlClient;
using System.Web.Configuration;
using NBL.Models.EntityModels.Securities;

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
            var str = StringCipher.Decrypt(connectionString, "salam_cse_10_R");
            connectionObj = new SqlConnection(str);
            //connectionObj = new SqlConnection(connectionString);
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
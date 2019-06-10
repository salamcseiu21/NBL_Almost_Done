using System.Data.SqlClient;
using System.Web.Configuration;
using NBL.Models.EntityModels.Securities;

namespace NBL.DAL
{
    public class DbGateway
    {
        private readonly SqlConnection _connectionObj; 

        private readonly SqlCommand _commandObj;

        public DbGateway()
        {
            string connectionString =
                WebConfigurationManager.ConnectionStrings["UniversalBusinessSolutionDbConnectionString"]
                    .ConnectionString;
            var str = StringCipher.Decrypt(connectionString, "salam_cse_10_R");
            _connectionObj = new SqlConnection(str);
            //_connectionObj = new SqlConnection(connectionString);
            _commandObj = new SqlCommand();
        }

        public SqlConnection ConnectionObj
        {
            get
            {
                return _connectionObj;
            }


        }

        public SqlCommand CommandObj
        {
            get
            {
                _commandObj.Connection = _connectionObj;
                return _commandObj;
            }

        }
    }
}
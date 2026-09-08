using Managementsystem.Helper;
using Managementsystem.Interface;
using Managementsystem.Model;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using System.Data;

namespace Managementsystem.Service
{
    public class LoginService : ILoginService
    {
        private readonly SqlHelper _sqlHelper;
        private readonly PasswordHasher<LoginRequest> _passwordHasher;

        public LoginService(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
            _passwordHasher = new PasswordHasher<LoginRequest>();
        }

        public DataSet Login(LoginRequest request)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@Username", request.Username)
            };

            DataSet ds = _sqlHelper.ExecuteDataSet("SP_Login",param);
            if (ds.Tables.Count == 0 || ds.Tables[1].Rows.Count == 0)
            {
                throw new Exception("Invalid username");
            }

            string passwordHash = ds.Tables[1].Rows[0]["PasswordHash"].ToString();

            var result = _passwordHasher.VerifyHashedPassword(request,passwordHash,request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid password");
            }
            return ds;
        }

        public DataTable DeleteUser(int id)
        {
            SqlParameter[] param =
            {
                    new SqlParameter("@Id", id)
            };

            return _sqlHelper.ExecuteDataTable("SP_DeleteUser", param);

        }

        public DataTable getuserDetails()
        {
            SqlParameter[] parameters =
            {

            };

            return _sqlHelper.ExecuteDataTable("SP_GetUserDetail", parameters);
        }
    }
}

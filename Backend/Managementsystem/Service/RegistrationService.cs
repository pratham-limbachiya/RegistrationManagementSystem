using Managementsystem.Helper;
using Managementsystem.Interface;
using Managementsystem.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Data;
using System.Data.SqlClient;

namespace Managementsystem.Service
{
    public class RegistrationService : IRegistrationService
    {
        private readonly SqlHelper _sqlHelper;
        private readonly PasswordHasher<Registration> _passwordHasher;

        public RegistrationService(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
            _passwordHasher = new PasswordHasher<Registration>();
        }
        public DataTable SaveOrUpdate(Registration request)
        {
            if (request.Files != null && request.Files.Count > 0)
            {
                request.FileName = string.Join(",",
                    request.Files.Select(file => Path.GetFileName(file.FileName)));
            }

            // Password hash
            string? passwordHash = null;

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                passwordHash = _passwordHasher.HashPassword(
                    request,
                    request.Password
                );
            }
            SqlParameter[] param =
            {
                    new SqlParameter("@Id", request.Id),
                    new SqlParameter("@Name", request.Name),
                    new SqlParameter("@Username", request.Username),
                    new SqlParameter("@PasswordHash",(object?)passwordHash ?? DBNull.Value),
                    new SqlParameter("@DateOfBirth", request.DateOfBirth),
                    new SqlParameter("@Gender", request.Gender),
                    new SqlParameter("@Hobbies", request.Hobbies),
                    new SqlParameter("@Address", request.Address),
                    new SqlParameter("@StateId", request.StateId),
                    new SqlParameter("@CityId", request.CityId),
                    new SqlParameter("@Pincode", request.Pincode),
                    new SqlParameter("@FileName", (object?)request.FileName ?? DBNull.Value)
            };

             return _sqlHelper.ExecuteDataTable("SP_SaveOrUpdateUser", param);
        }

        public DataTable GetStates()
        {
            string query = @"SELECT StateId,StateName FROM State ORDER BY StateName";

            return _sqlHelper.ExecuteNonQuery(query);
        }

        public DataTable GetCities(int stateId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@StateId", stateId)
            };

            return _sqlHelper.ExecuteDataTable("GetCitiesByState", parameters);
        }

    }
}

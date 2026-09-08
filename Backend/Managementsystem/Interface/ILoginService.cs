using Managementsystem.Model;
using System.Data;

namespace Managementsystem.Interface
{
    public interface ILoginService
    {
        DataSet Login(LoginRequest request);
        DataTable DeleteUser(int id);
        DataTable getuserDetails();
    }
}

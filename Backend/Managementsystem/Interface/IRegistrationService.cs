using Managementsystem.Model;
using System.Data;

namespace Managementsystem.Interface
{
    public interface IRegistrationService
    {
        DataTable SaveOrUpdate(Registration request);
        DataTable GetStates();
        DataTable GetCities(int stateId);
    }
}

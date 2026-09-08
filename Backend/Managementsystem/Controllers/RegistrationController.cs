using Managementsystem.Helper;
using Managementsystem.Interface;
using Managementsystem.Model;
using Managementsystem.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Managementsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly CommonHelper _commonHelper;
        public RegistrationController(IRegistrationService RegistrationService,CommonHelper commonHelper)
        {
            _registrationService=RegistrationService;
            _commonHelper=commonHelper;
        }

        [HttpPost("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(Registration request)
        {
            try
            {
                DataTable dt = _registrationService.SaveOrUpdate(request);

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                return Ok( _commonHelper.ConvertDataSetToJson(ds,200,request.Id == 0? "Data saved successfully":"Data updated successfully"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetStates")]
        public IActionResult GetStates()
        {
            try
            {
                DataTable dt = _registrationService.GetStates();

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                return Ok(_commonHelper.ConvertDataSetToJson(ds, 200, "States fetched successfully"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("GetCities/{stateId}")]
        public IActionResult GetCities(int stateId)
        {
            try
            {
                DataTable dt = _registrationService.GetCities(stateId);

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                return Ok(_commonHelper.ConvertDataSetToJson(ds, 200, "Cities fetched successfully"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

using Managementsystem.Helper;
using Managementsystem.Interface;
using Managementsystem.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Managementsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly CommonHelper _commonHelper;

        public LoginController(ILoginService loginService,CommonHelper commonHelper)
        {
            _loginService = loginService;
            _commonHelper = commonHelper;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                DataSet ds = _loginService.Login(request);

                //DataSet ds = new DataSet();
                //ds.Tables.Add(dt);

                return Ok(_commonHelper.ConvertDataSetToJson(ds,200,"Login successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(_commonHelper.ConvertDataSetToJson(null,400,ex.Message));
            }
        }

        [HttpPost("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                DataTable dt = _loginService.DeleteUser(id);

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                int result = Convert.ToInt32(ds.Tables[0].Rows[0]["Result"]);

                if (result ==1 )
                {
                    return Ok(_commonHelper.ConvertDataSetToJson(ds, 200, "Data deleted successfully"));

                }
                else
                {
                    return Ok(_commonHelper.ConvertDataSetToJson(ds,404, "User not found"));

                }
            }
            catch (Exception ex)
            {
                return BadRequest(_commonHelper.ConvertDataSetToJson(null, 400, ex.Message));

            }
        }


        [HttpGet("getuserDetails")]
        public IActionResult getuserDetails()
        {
            try
            {
                DataTable dt = _loginService.getuserDetails();

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                return Ok(_commonHelper.ConvertDataSetToJson(ds, 200, "User Data Fetched successfully"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

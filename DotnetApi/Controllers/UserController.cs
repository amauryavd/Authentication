
using DotnetApi.Data;
using DotnetApi.DTOs;
using DotnetApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        DataContextDapper _dapper;
        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("Users")]
        public IEnumerable<Users> GetUser()
        {
            string data = @"SELECT * FROM TutorialAppSchema.Users WHERE Gender= 'Female';";
            IEnumerable<Users> users = _dapper.LoadData<Users>(data);
            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public Users GetSingleuser(int userId)
        {
            string data = @"SELECT * FROM TutorialAppSchema.Users WHERE UserId=" + userId.ToString();
            Users user = _dapper.LoadDataSingle<Users>(data);
            return user;
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(Users user)
        {
            string sql = @"
                    UPDATE TutorialAppSchema.Users
                    SET [FirstName] = '" + user.FirstName +
                        "', [LastName] = '" + user.LastName +
                        "', [Email] = '" + user.Email +
                        "', [Gender] = '" + user.Gender +
                        "', [Active] = '" + user.Active +
                        "' WHERE userId = " + user.UserId;
            if (_dapper.Execute(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to update user.");
        }

        [HttpPost("Adduser")]
        public IActionResult AddUser(UsersDto user)
        {
            string sql = @"INSERT INTO TutorialAppSchema.Users(
                        [FirstName],
                        [LastName],
                        [Email],
                        [Gender], 
                        [Active]
                    ) VALUES(
                        '" + user.FirstName +
                        "','" + user.LastName +
                        "','" + user.Email +
                        "','" + user.Gender +
                        "','" + user.Active +
                        "')";
            if (_dapper.Execute(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to update user.");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            string sql = "DELETE FROM TutorialAppSchema.Users WHERE UserId=" + userId.ToString();

            if (_dapper.Execute(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to delete user.");

        }

    }
}



using DotnetApi.Data;
using DotnetApi.DTOs;
using DotnetApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

namespace DotnetApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;

        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDTO registerUser)
        {
            try
            {
                if (registerUser.Password == registerUser.PasswordConfirm)
                {
                    string sqlCheckUserExists = "SELECT EMAIL FROM TutorialAppSchema.Auth WHERE EMAIL='" + registerUser.Email + "'";
                    IEnumerable<string> userExists = _dapper.LoadData<string>(sqlCheckUserExists);
                    if (userExists.Count() == 0)
                    {
                        byte[] passwordSalt = new byte[128 / 8];
                        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                        {
                            rng.GetNonZeroBytes(passwordSalt);
                        }

                        byte[] passwordHash = _authHelper.GetPasswordHash(registerUser.Password, passwordSalt);
                        string sqlAuth = "INSERT INTO TutorialAppSchema.Auth([EMAIL], [PasswordHash], [PasswordSalt]) VALUES('" + registerUser.Email + "', @Passwordhash, @PasswordSalt)";

                        List<SqlParameter> sqlParameters = new List<SqlParameter>();

                        SqlParameter passwordSaltParam = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                        passwordSaltParam.Value = passwordSalt;

                        SqlParameter passwordHashParam = new SqlParameter("@Passwordhash", SqlDbType.VarBinary);
                        passwordHashParam.Value = passwordHash;

                        sqlParameters.Add(passwordSaltParam);
                        sqlParameters.Add(passwordHashParam);
                        if (_dapper.ExecuteSQLWithParameters(sqlAuth, sqlParameters))
                        {
                            string sqlAddUser = @"INSERT INTO TutorialAppSchema.Users(
                                            [FirstName],
                                            [LastName],
                                            [Email],
                                            [Gender], 
                                            [Active]
                                        ) VALUES(
                                            '"+ registerUser.FirstName +
                                            "','" + registerUser.LastName +
                                            "','" + registerUser.Email +
                                            "','" + registerUser.Gender +
                                            "',1)";
                            if (_dapper.Execute(sqlAddUser))
                            {
                                return Ok();
                            }
                            throw new AccessFailedException("Failed to add user");
                        }
                        throw new AccessFailedException("Failed to register user");
                    }
                    throw new AccessFailedException("User with this email already exists");
                }
                throw new AccessFailedException("Password doesn't match");
            }
            catch(AccessFailedException af)
            {
                return BadRequest(af.Message);
            }
            
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDTO loginUser)
        {
            try
            {
                string sqlEmailExist = "SELECT [Email] FROM TutorialAppSchema.Users WHERE Email='" + loginUser.Email + "'";
                IEnumerable<string> userNotExists = _dapper.LoadData<string>(sqlEmailExist);
                if (userNotExists.Count() != 0)
                {
                    string sqlforuserLogin = "SELECT [PasswordHash], [PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email='" + loginUser.Email + "'";
                    UserForLoginConfirmationDTO loginConfirm = _dapper.LoadDataSingle<UserForLoginConfirmationDTO>(sqlforuserLogin);
                    byte[] passwordHash = _authHelper.GetPasswordHash(loginUser.Password, loginConfirm.PasswordSalt);

                    for (int i = 0; i < passwordHash.Length; i++)
                    {
                        if (passwordHash[i] != loginConfirm.Passwordhash[i])
                        {
                            throw new AccessFailedException("Incorrect Password");
                        }
                    }

                    string userIdSql = "SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" + loginUser.Email + "'";

                    int userId = _dapper.LoadDataSingle<int>(userIdSql);

                    return Ok(new Dictionary<string, string>
                    {
                        {"token", _authHelper.CreateToken(userId) }
                    });
                }
                throw new AccessFailedException("User does not exist");
                
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            string userIdSql = "SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = '" + User.FindFirst("userId")?.Value + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return _authHelper.CreateToken(userId);
        }
    }

    public class AccessFailedException : Exception
    {
        public AccessFailedException(string msg) : base(msg){}
    }
}

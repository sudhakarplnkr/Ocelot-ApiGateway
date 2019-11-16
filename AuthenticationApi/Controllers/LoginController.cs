namespace AuthenticationApi.Controllers
{
    using AuthenticationApi.Model;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // POST api/authentication
        [HttpPost, Route("")]
        public User Post([FromBody] Login login)
        {
            return new User
            {
                Name = login.Username,
                Email = $"{login.Username}@abc.de",
                Token = "test"
            };
        }
    }
}
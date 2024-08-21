using Microsoft.AspNetCore.Mvc;
using MyFace.Repositories;
using MyFace.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Principal;
using System;


namespace MyFace.Controllers
{
    [ApiController]
    [Route("/login")]
    public class LoginController : ControllerBase
    {
        private readonly IUsersRepo _usersRepo;

        public LoginController(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }
        //default page if not logged in
        //take user's username and password
        //store in the context
        //if 401 response, remove user's details from context and display this page again
        [HttpPost("/login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserLoginRequest loginUser)
        {
            Console.WriteLine($"inside the Login controller - user = {loginUser.Username}, pass = {loginUser.Password}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _usersRepo.Authenticate(loginUser.Username, loginUser.Password);
            if (user == null)
            {
                return BadRequest(ModelState);
            }

            return Accepted(user);
            //throw new NotImplementedException();

        }
    }
}
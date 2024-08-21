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
        public IActionResult Login()
        {
            // get the Authorization part of the request
            string authHeader = HttpContext.Request.Headers["Authorization"];


            string username = "";
            string passwd = "";
            // remove the 'Basic' from the Authorization string
            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            // decode
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
            // now extract the username and password 
            int seperatorIndex = usernamePassword.IndexOf(':');
                username = usernamePassword.Substring(0, seperatorIndex);
                passwd = usernamePassword.Substring(seperatorIndex + 1);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // pass to get validated
            var user = _usersRepo.Authenticate(username, passwd);
            if (user == null)
            {
                return BadRequest(ModelState);
            }

            return Accepted(user);
            //throw new NotImplementedException();

        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DotnetSampleApi.Controllers
{
    public class AccountController : Controller
    {
        private Random _random;

        public AccountController()
        {
            _random = new Random();
        }

        [HttpPost]
        public IActionResult CreateAccount(string login, string password)
        {
            if (_random.Next(3) % 3 == 0)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            };

            return Ok($"Account created {login}");
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            if (_random.Next(3) % 3 == 0)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            };

            return Ok();
        }
    }
}

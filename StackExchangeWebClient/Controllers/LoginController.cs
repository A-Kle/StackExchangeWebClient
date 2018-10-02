using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchangeWebClient.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl = "/Home")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }
    }
}

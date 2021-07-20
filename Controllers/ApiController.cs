using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using KakaoManagerBeta.Util;
using KakaoManagerBeta.Models;

namespace KakaoManagerBeta.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController
    {
        [HttpPost("login/{id}/{password}")]
        public IActionResult Login(string id, string password)
        {
            if (id == "***REMOVED***" && password == "***REMOVED***")
            {
                Globals.Session = Guid.NewGuid().ToString();
                return new OkObjectResult(Globals.Session);
            }
            else
            {   
                return new UnauthorizedResult();
            }
        }

        [HttpGet("domains/{session}")]
        public IActionResult GetDomains(string session)
        {
            if(session != Globals.Session)
            {
                return new UnauthorizedResult();
            }
            else
            {
                return new OkObjectResult(Globals.Domains);
            }
        }

        [HttpPost("domain/{url}/{session}")]
        public IActionResult AddDomain(string url, string session)
        {
            if(session != Globals.Session)
            {
                return new UnauthorizedResult();
            }
            else
            {
                Globals.Domains.Add(url);
                Globals.Domains.Sort();
                Globals.Save();

                return new OkResult();
            }
        }

        [HttpDelete("domain/{url}/{session}")]
        public IActionResult DeleteDomain(string url, string session)
        {
            if (session != Globals.Session)
            {
                return new UnauthorizedResult();
            }
            else
            {
                Globals.Domains.Remove(url);
                Globals.Domains.Sort();
                Globals.Save();

                return new OkResult();
            }
        }
    }
}

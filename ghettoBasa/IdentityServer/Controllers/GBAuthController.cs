using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Respositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedResources.Models;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GBAuthController : ControllerBase
    {
        private IAuthRepository _auth;

        public GBAuthController(IAuthRepository auth)
        {
            _auth = auth;
        }

        // GET: api/GBAuth
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/GBAuth/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/GBAuth
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPost("/api/[controller]/login")]
        public IActionResult UserLogin(UserCredentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resp = _auth.AuthenticateUser(credentials);

            if(resp == null)
            {
                return NotFound();
            }

            return Ok(resp);
        }

        [HttpPost("/api/[controller]/reset")]
        public IActionResult UserReset(ResetDetails reset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resp = _auth.ResetRequest(reset.userId, reset.Front);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(resp);
        }

        // PUT: api/GBAuth/5
        [HttpPut("/api/[controller]/changepassword")]
        public IActionResult ChangePassword(ChangePassword change)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resp = _auth.ChangePassword(change);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(resp);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

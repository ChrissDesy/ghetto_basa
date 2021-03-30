using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedResources.Models;
using System.IdentityModel.Tokens.Jwt;

namespace ghettoBasa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2", "Test" };
        }

        // GET api/values/5
        [HttpGet("/decodedToken")]
        public string DecodeToken()
        {

            var tok = HttpContext.Request.Headers["Authorization"];

            if (tok.Count == 0)
            {
                return "No Token";
            }

            tok = tok.ToString().Substring(7);

            var token = new JwtSecurityToken(tok);
            var claims = token.Claims.First(cl => cl.Type == "name");

            return claims.Value;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

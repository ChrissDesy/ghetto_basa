using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ghettoBasa.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedResources.Models;

namespace ghettoBasa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository _users;

        public UsersController(IUserRepository userRepository)
        {
            _users = userRepository;
        }

        [HttpGet]
        public IEnumerable<Users> GetUsers()
        {
            return _users.GetUsers();
        }

        [HttpGet("/api/[controller]/admin")]
        public IEnumerable<Users> GetAdmins()
        {
            return _users.GetAdminUsers();
        }

        [HttpGet("/api/[controller]/clients")]
        public IEnumerable<Users> GetClients()
        {
            return _users.GetClientUsers();
        }

        [HttpGet("{id}")]
        public Users GetUser(string id)
        {
            return _users.GetUser(id);
        }

        [HttpGet("/api/[controller]/username/{username}")]
        public Users GetUserByUsername(string username)
        {
            return _users.GetUserByUsername(username);
        }

        [HttpGet("/api/[controller]/nationalid/{natid}")]
        public Users GetUserByIdentity(string natid)
        {
            return _users.GetUserByIdentity(natid);
        }

        [HttpGet("/api/[controller]/deleted")]
        public IEnumerable<Users> GetDeletedUsers()
        {
            return _users.GetDeletedUsers();
        }

        [HttpPost]
        public IActionResult PostUser(Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _users.CreateUser(user);

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        [HttpPost("/api/[controller]/un-delete/{userId}")]
        public IActionResult UpdateDeleteStatus(string userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            var resp = _users.UserDeleteStatus(userId, false);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetUser(userId));
        }

        [HttpPost("/api/[controller]/enable-user/{userId}")]
        public IActionResult EnableUser(string userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            var resp = _users.UserStatus(userId, "active");

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetUser(userId));
        }

        [HttpPost("/api/[controller]/disable-user/{userId}")]
        public IActionResult DisableUser(string userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            var resp = _users.UserStatus(userId, "disabled");

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetUser(userId));
        }

        [HttpPut("{id}")]
        public IActionResult PutUser(string id, Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(id != user.UserId)
            {
                return BadRequest();
            }

            var resp = _users.UpdateUser(user);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("/api/[controller]/delete/{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            if(userId == null)
            {
                return BadRequest();
            }

            var resp = _users.UserDeleteStatus(userId, true);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetUser(userId));
        }
    }
}
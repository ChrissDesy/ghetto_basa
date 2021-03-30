using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using ghettoBasa.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedResources.DTOs;
using SharedResources.Models;

namespace ghettoBasa.Controllers
{
    [Authorize]
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
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetUsers(tok);
        }

        [HttpGet("/api/[controller]/{page}/{size}")]
        public MyResponse GetPaginatedUsers(int page, int size)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetPaginatedUsers(page, size, tok);
        }

        [HttpGet("/api/[controller]/admin")]
        public IEnumerable<Users> GetAdmins()
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetAdminUsers(tok);
        }

        [HttpGet("/api/[controller]/clients")]
        public IEnumerable<Users> GetClients()
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetClientUsers(tok);
        }

        [HttpGet("/api/[controller]/admin/{page}/{size}")]
        public MyResponse GetPaginatedAdmins(int page, int size)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetPaginatedAdminUsers(page, size, tok);
        }

        [HttpGet("/api/[controller]/clients/{page}/{size}")]
        public MyResponse GetPaginatedClients(int page, int size)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetPaginatedClientUsers(page, size, tok);
        }

        [HttpGet("{id}")]
        public Users GetUser(string id)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetUser(id, tok);
        }

        [HttpGet("/api/[controller]/username/{username}")]
        public Users GetUserByUsername(string username)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetUserByUsername(username, tok);
        }

        [HttpGet("/api/[controller]/nationalid/{natid}")]
        public Users GetUserByIdentity(string natid)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetUserByIdentity(natid, tok);
        }

        [HttpGet("/api/[controller]/deleted")]
        public IEnumerable<Users> GetDeletedUsers()
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetDeletedUsers(tok);
        }

        [HttpPost]
        public IActionResult PostUser(Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            _users.CreateUser(user, tok);

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        [HttpPost("/api/[controller]/un-delete/{userId}")]
        public IActionResult UpdateDeleteStatus(string userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _users.UserDeleteStatus(userId, false, tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _users.UserStatus(userId, "active", tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];
                
            var resp = _users.UserStatus(userId, "disabled", tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _users.UpdateUser(user, tok);

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

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _users.UserDeleteStatus(userId, true, tok);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetUser(userId));
        }

        // Manage User Reviews
        [HttpGet("/api/[controller]/reviews/user/{userid}")]
        public IEnumerable<Reviews> GetUserReviews(string userid)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetUserReviews(userid, tok);
        }

        [HttpGet("/api/[controller]/reviews/{id}")]
        public Reviews GetReview(int id)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetReview(id, tok);
        }

        [HttpPost("/api/[controller]/reviews")]
        public IActionResult PostReview(Reviews review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            _users.CreateReview(review, tok);

            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }

        [HttpDelete("/api/[controller]/review/delete/{Id}")]
        public IActionResult DeleteReview(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _users.ReviewDeleteStatus(Id, true, tok);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetReview(Id));
        }

        [HttpPost("/api/[controller]/review/un-delete/{Id}")]
        public IActionResult UnDeleteReview(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _users.ReviewDeleteStatus(Id, false, tok);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetReview(Id));
        }

        // Manage User Ratings
        [HttpGet("/api/[controller]/ratings/user/{userid}")]
        public IEnumerable<Ratings> GetUserRatings(string userid)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetUserRatings(userid, tok);
        }

        [HttpGet("/api/[controller]/ratings/user-rating/{userid}")]
        public double GetUserRating(string userid)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetUserRating(userid, tok);
        }

        [HttpGet("/api/[controller]/ratings/{id}")]
        public Ratings GetRating(int id)
        {
            var tok = HttpContext.Request.Headers["Authorization"];

            return _users.GetRating(id, tok);
        }

        [HttpPost("/api/[controller]/ratings")]
        public IActionResult PostRatings(Ratings rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            _users.CreateRating(rating, tok);

            return CreatedAtAction("GetRating", new { id = rating.Id }, rating);
        }

        [HttpDelete("/api/[controller]/ratings/delete/{Id}")]
        public IActionResult DeleteRatings(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _users.RatingDeleteStatus(Id, true, tok);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetRating(Id));
        }

        [HttpPost("/api/[controller]/ratings/un-delete/{Id}")]
        public IActionResult UnDeleteRating(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }

            var tok = HttpContext.Request.Headers["Authorization"];

            var resp = _users.RatingDeleteStatus(Id, false, tok);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetRating(Id));
        }


    }
}
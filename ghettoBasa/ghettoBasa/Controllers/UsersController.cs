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

        // Manage User Reviews
        [HttpGet("/api/[controller]/reviews/user/{userid}")]
        public IEnumerable<Reviews> GetUserReviews(string userid)
        {
            return _users.GetUserReviews(userid);
        }

        [HttpGet("/api/[controller]/reviews/{id}")]
        public Reviews GetReview(int id)
        {
            return _users.GetReview(id);
        }

        [HttpPost("/api/[controller]/reviews")]
        public IActionResult PostReview(Reviews review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _users.CreateReview(review);

            return CreatedAtAction("GetReview", new { id = review.Id }, review);
        }

        [HttpDelete("/api/[controller]/review/delete/{Id}")]
        public IActionResult DeleteReview(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }

            var resp = _users.ReviewDeleteStatus(Id, true);

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

            var resp = _users.ReviewDeleteStatus(Id, false);

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
            return _users.GetUserRatings(userid);
        }

        [HttpGet("/api/[controller]/ratings/user-rating/{userid}")]
        public double GetUserRating(string userid)
        {
            return _users.GetUserRating(userid);
        }

        [HttpGet("/api/[controller]/ratings/{id}")]
        public Ratings GetRating(int id)
        {
            return _users.GetRating(id);
        }

        [HttpPost("/api/[controller]/ratings")]
        public IActionResult PostRatings(Ratings rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _users.CreateRating(rating);

            return CreatedAtAction("GetRating", new { id = rating.Id }, rating);
        }

        [HttpDelete("/api/[controller]/ratings/delete/{Id}")]
        public IActionResult DeleteRatings(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }

            var resp = _users.RatingDeleteStatus(Id, true);

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

            var resp = _users.RatingDeleteStatus(Id, false);

            if (!resp)
            {
                return NotFound();
            }

            return Ok(GetRating(Id));
        }


    }
}
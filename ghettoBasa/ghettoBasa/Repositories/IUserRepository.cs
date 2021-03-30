using SharedResources.DTOs;
using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ghettoBasa.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<Users> GetUsers(string token);
        IEnumerable<Users> GetAdminUsers(string token);
        IEnumerable<Users> GetClientUsers(string token);
        MyResponse GetPaginatedUsers(int page, int size, string token);
        MyResponse GetPaginatedAdminUsers(int page, int size, string token);
        MyResponse GetPaginatedClientUsers(int page, int size, string token);
        Users GetUser(string UserId, string token);
        Users GetUserByUsername(string Username, string token);
        Users GetUserByIdentity(string Natid, string token);
        void CreateUser(Users user, string token);
        bool UpdateUser(Users user, string token);
        IEnumerable<Users> SearchUsers(string[] Parameter);
        IEnumerable<Users> GetDeletedUsers(string token);
        bool UserDeleteStatus(string UserId, bool action, string token);
        bool UserStatus(string UserId, string action, string token);

        // Manage user reviews
        IEnumerable<Reviews> GetUserReviews(string UserId, string token);
        Reviews GetReview(int Id, string token);
        void CreateReview(Reviews review, string token);
        bool ReviewDeleteStatus(int Id, bool action, string token);

        // Manage user Ratings
        IEnumerable<Ratings> GetUserRatings(string UserId, string token);
        double GetUserRating(string UserId, string token);
        Ratings GetRating(int Id, string token);
        void CreateRating(Ratings rating, string token);
        bool RatingDeleteStatus(int Id, bool action, string token);

    }
}

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
        IEnumerable<Users> GetUsers();
        IEnumerable<Users> GetAdminUsers();
        IEnumerable<Users> GetClientUsers();
        MyResponse GetPaginatedUsers(int page, int size);
        MyResponse GetPaginatedAdminUsers(int page, int size);
        MyResponse GetPaginatedClientUsers(int page, int size);
        Users GetUser(string UserId);
        Users GetUserByUsername(string Username);
        Users GetUserByIdentity(string Natid);
        void CreateUser(Users user);
        bool UpdateUser(Users user);
        IEnumerable<Users> SearchUsers(string[] Parameter);
        IEnumerable<Users> GetDeletedUsers();
        bool UserDeleteStatus(string UserId, bool action);
        bool UserStatus(string UserId, string action);

        // Manage user reviews
        IEnumerable<Reviews> GetUserReviews(string UserId);
        Reviews GetReview(int Id);
        void CreateReview(Reviews review);
        bool ReviewDeleteStatus(int Id, bool action);

        // Manage user Ratings
        IEnumerable<Ratings> GetUserRatings(string UserId);
        double GetUserRating(string UserId);
        Ratings GetRating(int Id);
        void CreateRating(Ratings rating);
        bool RatingDeleteStatus(int Id, bool action);

    }
}

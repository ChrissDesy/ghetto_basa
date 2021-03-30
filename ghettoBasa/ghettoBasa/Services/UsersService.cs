using ghettoBasa.Repositories;
using Microsoft.EntityFrameworkCore;
using SharedResources.DTOs;
using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ghettoBasa.Services
{
    public class UsersService : IUserRepository
    {
        private readonly ghettoBasaContext ctx;
        private static Random random = new Random();

        public UsersService(ghettoBasaContext context)
        {
            ctx = context; 
        }

        public void CreateUser(Users user, string token)
        {
            user.UserId = GenerateUserId();

            try
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();

                var syUser = new SystemUsers()
                {
                    UserType = user.UserType,
                    Username = user.Username,
                    Password = GenerateUserPassword(),
                    Status = "active",
                    Hash = "-",
                    Deleted = false,
                    UserRefe = user.UserId,
                    ResetRequest = "-"
                };
                var trail = new AuditTrails()
                {
                    
                };

                try
                {
                    ctx.SystemUsers.Add(syUser);
                    ctx.SaveChanges();
                    SendEmail(user.Email);
                }
                catch
                {
                    return;
                }
            }
            catch
            {
                return;
            }
        }

        public IEnumerable<Users> GetDeletedUsers(string token)
        {
            var users = from u in ctx.Users.Where(ab => ab.Deleted)
                        select u;

            return users;
        }

        public Users GetUser(string UserId, string token)
        {
            var user = ctx.Users.Where(ab => ab.UserId == UserId).FirstOrDefault();

            return user;
        }

        public Users GetUserByUsername(string Username, string token)
        {
            var user = ctx.Users.Where(ab => ab.Username == Username).FirstOrDefault();

            return user;
        }

        public IEnumerable<Users> GetUsers(string token)
        {
            var users = from u in ctx.Users.Where(ab => !ab.Deleted)
                        select u;

            return users;
        }

        public IEnumerable<Users> SearchUsers(string[] Parameter)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(Users user, string token)
        {
            try
            {
                ctx.Entry(user).State = EntityState.Modified;

                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        string GenerateUserId()
        {
            var unum = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString();

            var gid = RandomNumbers(2) + unum + RandomString(1);

            return gid;
        }

        string GenerateUserPassword()
        {
            return "Pass1234";
        }

        bool SendEmail(string email)
        {
            return true;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumbers(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool UserDeleteStatus(string UserId, bool action, string token)
        {
            var user = ctx.Users.Where(ab => ab.UserId == UserId).FirstOrDefault();
            var syUser = ctx.SystemUsers.Where(cd => cd.UserRefe == UserId).FirstOrDefault();

            user.Deleted = action;
            syUser.Deleted = action;

            try
            {
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Users GetUserByIdentity(string Natid, string token)
        {
            var user = ctx.Users.Where(ab => ab.NationalId == Natid).FirstOrDefault();

            return user;
        }

        public bool UserStatus(string UserId, string action, string token)
        {
            var user = ctx.Users.Where(ab => ab.UserId == UserId).FirstOrDefault();
            var syUser = ctx.SystemUsers.Where(cd => cd.UserRefe == UserId).FirstOrDefault();

            user.Status = action;
            syUser.Status = action;

            try
            {
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Users> GetAdminUsers(string token)
        {
            var users = from u in ctx.Users.Where(ab => !ab.Deleted && ab.UserType == "Admin")
                        select u;

            return users;
        }

        public IEnumerable<Users> GetClientUsers(string token)
        {
            var users = from u in ctx.Users.Where(ab => !ab.Deleted && ab.UserType != "Admin")
                        select u;

            return users;
        }

        public MyResponse GetPaginatedUsers(int page, int size, string token)
        {
            var users = from u in ctx.Users.Where(ab => !ab.Deleted)
                         .OrderBy(cd => cd.Id)
                         .Skip(page * size)
                         .Take(size)
                        select u;

            var count = ctx.Users.Where(bd => !bd.Deleted).Count();

            var response = new MyResponse()
            {
                totalElements = count,
                content = users,
                totalPages = (int)(count / size),
                currentPage = page + 1
            };

            return response;
        }

        public MyResponse GetPaginatedAdminUsers(int page, int size, string token)
        {
            var users = from u in ctx.Users.Where(ab => !ab.Deleted && ab.UserType == "Admin")
                        .OrderBy(cd => cd.Id)
                         .Skip(page * size)
                         .Take(size)
                        select u;

            var count = ctx.Users.Where(bd => !bd.Deleted && bd.UserType == "Admin").Count();

            var response = new MyResponse()
            {
                totalElements = count,
                content = users,
                totalPages = (int)(count / size),
                currentPage = page + 1
            };

            return response;
        }

        public MyResponse GetPaginatedClientUsers(int page, int size, string token)
        {
            var users = from u in ctx.Users.Where(ab => !ab.Deleted && ab.UserType != "Admin")
                        .OrderBy(cd => cd.Id)
                         .Skip(page * size)
                         .Take(size)
                        select u;

            var count = ctx.Users.Where(bd => !bd.Deleted && bd.UserType != "Admin").Count();

            var response = new MyResponse()
            {
                totalElements = count,
                content = users,
                totalPages = (int)(count / size),
                currentPage = page + 1
            };

            return response;
        }


        // Manage User Reviews
        public IEnumerable<Reviews> GetUserReviews(string UserId, string token)
        {
            var reviews = from r in ctx.Reviews.Where(ab => ab.UserRef == UserId && !ab.Deleted)
                          select r;

            return reviews;
        }

        public Reviews GetReview(int Id, string token)
        {
            var review = ctx.Reviews.FirstOrDefault(ab => ab.Id == Id);

            return review;
        }

        public void CreateReview(Reviews review, string token)
        {
            try
            {
                ctx.Reviews.Add(review);
                ctx.SaveChanges();
            }
            catch
            {
                return;
            }
        }

        public bool ReviewDeleteStatus(int Id, bool action, string token)
        {
            var rvw = ctx.Reviews.Where(ab => ab.Id == Id).FirstOrDefault();

            rvw.Deleted = action;

            try
            {
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        // Manage User Ratings.
        public IEnumerable<Ratings> GetUserRatings(string UserId, string token)
        {
            var ratings = from r in ctx.Ratings.Where(ab => ab.UserRefer == UserId && !ab.Deleted)
                          select r;

            return ratings;
        }

        public double GetUserRating(string UserId, string token)
        {
            var ratings = ctx.Ratings.Where(ab => ab.UserRefer == UserId && !ab.Deleted);

            var rt = new List<int>();

            foreach(var r in ratings)
            {
                rt.Add(r.Rating);
            }

            if(rt.Count > 0)
            {
                return rt.Average();
            }
            else
            {
                return 0;
            }
        }

        public Ratings GetRating(int Id, string token)
        {
            var rating = ctx.Ratings.FirstOrDefault(ab => ab.Id == Id);

            return rating;
        }

        public void CreateRating(Ratings rating, string token)
        {
            try
            {
                ctx.Ratings.Add(rating);
                ctx.SaveChanges();
            }
            catch
            {
                return;
            }
        }

        public bool RatingDeleteStatus(int Id, bool action, string token)
        {
            var rtn = ctx.Ratings.Where(ab => ab.Id == Id).FirstOrDefault();

            rtn.Deleted = action;

            try
            {
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // get logged in user from token
        public string getUsername(string theToken)
        {
            var tok = "";

            tok = theToken.Substring(7);

            var token = new JwtSecurityToken(tok);
            var claims = token.Claims.First(cl => cl.Type == "name");

            return claims.Value;
        }
    }
}

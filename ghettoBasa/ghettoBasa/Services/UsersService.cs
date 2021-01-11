using ghettoBasa.Repositories;
using Microsoft.EntityFrameworkCore;
using SharedResources.Models;
using System;
using System.Collections.Generic;
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

        public void CreateUser(Users user)
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

        public IEnumerable<Users> GetDeletedUsers()
        {
            var users = from u in ctx.Users.Where(ab => ab.Deleted)
                        select u;

            return users;
        }

        public Users GetUser(string UserId)
        {
            var user = ctx.Users.Where(ab => ab.UserId == UserId).FirstOrDefault();

            return user;
        }

        public Users GetUserByUsername(string Username)
        {
            var user = ctx.Users.Where(ab => ab.Username == Username).FirstOrDefault();

            return user;
        }

        public IEnumerable<Users> GetUsers()
        {
            var users = from u in ctx.Users.Where(ab => !ab.Deleted)
                        select u;

            return users;
        }

        public IEnumerable<Users> SearchUsers(string[] Parameter)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(Users user)
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

        public bool UserDeleteStatus(string UserId, bool action)
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

        public Users GetUserByIdentity(string Natid)
        {
            var user = ctx.Users.Where(ab => ab.NationalId == Natid).FirstOrDefault();

            return user;
        }

        public bool UserStatus(string UserId, string action)
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

        public IEnumerable<Users> GetAdminUsers()
        {
            var users = from u in ctx.Users.Where(ab => !ab.Deleted && ab.UserType == "Admin")
                        select u;

            return users;
        }

        public IEnumerable<Users> GetClientUsers()
        {
            var users = from u in ctx.Users.Where(ab => !ab.Deleted && ab.UserType != "Admin")
                        select u;

            return users;
        }
    }
}

using IdentityModel;
using IdentityServer.Respositories;
using IdentityServer4;
using Microsoft.EntityFrameworkCore;
using SharedResources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class AuthService : IAuthRepository
    {
        private readonly ghettoBasaContext ctx;
        private readonly IdentityServerTools _tools;

        public AuthService(ghettoBasaContext context, IdentityServerTools serverTools)
        {
            ctx = context;
            _tools = serverTools;
        }

        public AuthenticatedUser AuthenticateUser(UserCredentials cred)
        {
            var user = ctx.SystemUsers.Where(ab => ab.Username == cred.Username && ab.UserType == cred.UserType && !ab.Deleted).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            bool response = false;
            bool change = false;

            if (user.Hash == "-")
            {
                if(cred.Password == user.Password)
                {
                    change = true;
                    response = true;
                }
            }
            else
            {
                response = ComparePasswords(user.Hash, user.Password, cred.Password);
            }

            if (!response)
            {
                return null;
            }

            var tok = GenerateToken(cred.UserType, cred.Username, user.UserRefe);

            string dnam = "";

            var theUser = ctx.Users.FirstOrDefault(cd => cd.UserId == user.UserRefe && !cd.Deleted);

            if(theUser == null)
            {
                return null;
            }

            dnam = theUser.Lastname + " " + theUser.Firstname;

            var info = new AuthenticatedUser()
            {
                AccessToken = tok.Result,
                DisplayName = dnam,
                RefreshToken = "-",
                PhotoUrl = theUser.PhotoUrl,
                UserId = theUser.UserId,
                Username = theUser.Username,
                UserType = cred.UserType,
                ChangePassword = change
            };

            return info;
        }

        bool ComparePasswords(string salt, string dbPass, string userPass)
        {
            //turn salt to bytes
            byte[] saltByte = new byte[16];
            saltByte = Convert.FromBase64String(salt);

            //hash the input pwd with the salt
            var salted = new Rfc2898DeriveBytes(userPass, saltByte, 1000);

            //place password in byte array
            byte[] hash = salted.GetBytes(20);

            //store hashed password 
            byte[] userBytes = new byte[36];
            Array.Copy(saltByte, 0, userBytes, 0, 16);
            Array.Copy(hash, 0, userBytes, 16, 20);

            //convert to string
            string EncPass = Convert.ToBase64String(userBytes);

            //compare passwords
            if (dbPass.Equals(EncPass))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Tuple<string, string> GeneratePassword(string password)
        {
            //generate salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            //hashing password with salt
            var salted = new Rfc2898DeriveBytes(password, salt, 1000);

            //place password in byte array
            byte[] hash = salted.GetBytes(20);

            //store hashed password 
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            //convert to string
            string EncPass = Convert.ToBase64String(hashBytes);
            string EncSalt = Convert.ToBase64String(salt);


            return new Tuple<string, string>(EncPass, EncSalt);
        }

        public async Task<string> GenerateToken(string role, string username, string userId)
        {
            var claims = new HashSet<Claim>(new ClaimComparer());

            claims.Add(new Claim(JwtClaimTypes.Role, role));
            claims.Add(new Claim(JwtClaimTypes.Scope, "ghettoBasa-api"));
            claims.Add(new Claim(JwtClaimTypes.ClientId, "ghettoBasa-frontend"));
            claims.Add(new Claim(JwtClaimTypes.Name, username));
            claims.Add(new Claim(JwtClaimTypes.Actor, userId));
            claims.Add(new Claim(JwtClaimTypes.Audience, "ghettoBasa-api"));

            var token = await _tools.IssueJwtAsync(

                lifetime: 3600,

                claims
            );

            return token;
        }

        public bool ChangePassword(ChangePassword info)
        {
            if(info.Type == "Reset")
            {
                var user = ctx.SystemUsers.Where(ab => ab.UserRefe == info.UserId && !ab.Deleted).FirstOrDefault();

                if (info.Token != user.ResetRequest)
                {
                    return false;
                }

                var pwds = GeneratePassword(info.NewPassword);

                user.Password = pwds.Item1;
                user.Hash = pwds.Item2;
                user.ResetRequest = "-";

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
            else if(info.Type == "Change")
            {
                var pwds = GeneratePassword(info.NewPassword);

                var user = ctx.SystemUsers.Where(ab => ab.UserRefe == info.UserId && !ab.Deleted).FirstOrDefault();

                user.Password = pwds.Item1;
                user.Hash = pwds.Item2;
                user.ResetRequest = "-";

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
            else
            {
                return false;
            }
        }

        public bool ResetRequest(string userid, string front)
        {
            var user = ctx.Users.Where(ab => ab.UserId == userid && !ab.Deleted).FirstOrDefault();

            if(user == null)
            {
                return false;
            }

            //generate salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            string token = Convert.ToBase64String(salt);

            var theUser = ctx.SystemUsers.Where(cd => !cd.Deleted && cd.UserRefe == userid).FirstOrDefault();

            theUser.ResetRequest = token;

            string myUrl = front + "/CompleteReset?token=" + token;

            string body = "You have requested a Password reset.<br><br>" +
                    "Click this link to complete your Request: " + myUrl + "<br> Alternatively, you can copy and paste this token for confirmation: <br><br>" + token + "<br><br>Regards,";


            try
            {
                ctx.Entry(theUser).State = EntityState.Modified;
                sendMail(user.Email, body);
                ctx.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool sendMail(string email, string body)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("chrissd@transhumantec.com");
            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = "Password Reset";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;

            SmtpClient client = new SmtpClient("mail.transhumantec.com", 587);
            //client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("chrissd@transhumantec.com", "cr0c0@_2040#");

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

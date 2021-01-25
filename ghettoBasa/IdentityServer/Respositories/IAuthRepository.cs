using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedResources.Models;

namespace IdentityServer.Respositories
{
    public interface IAuthRepository
    {
        AuthenticatedUser AuthenticateUser(UserCredentials cred);
        bool ChangePassword(ChangePassword change);
        bool ResetRequest(string email, string front);
    }
}

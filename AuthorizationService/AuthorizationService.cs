using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AuthorizationService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AuthorizationService" in both code and config file together.
    public class AuthorizationService : IAuthorizationService
    {
        User IAuthorizationService.AuthorizeUser(string token)
        {
            User user = new User();

            try
            {
                var claims = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(ConfigurationManager.AppSettings["secret_key"])
                .MustVerifySignature()
                .Decode<IDictionary<string, object>>(token);
                user.Id = Int64.Parse(claims["_id"].ToString());
                user.Username = (string)claims["username"];
                user.Email_Id = (string)claims["email_id"];
            }
            catch (TokenExpiredException)
            {
                throw new FaultException<AuthorizationFault>(new AuthorizationFault(AuthorizationFault.AuthorizationFaultType.TokenExpired));
            }
            catch (SignatureVerificationException)
            {
                throw new FaultException<AuthorizationFault>(new AuthorizationFault(AuthorizationFault.AuthorizationFaultType.InvalidSignature));
            }

            return user;
        }
    }
}

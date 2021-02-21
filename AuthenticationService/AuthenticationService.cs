using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace AuthenticationService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AuthenticationService" in both code and config file together.
    public class AuthenticationService : IAuthenticationService
    {
        string IAuthenticationService.AreCorrectCredentials(string username, string password)
        {
            string token = "";
            bool correct_password = true;
            bool user_found = true;
            string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE username = @username", conn);
                command.Parameters.AddWithValue("@username", username);
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    HashAlgorithm hashAlgorithm = new SHA512Managed();
                    byte[] salt = (byte[])reader["salt"];
                    byte[] hashed_password = (byte[])reader["hashed_password"];
                    byte[] password_bytes = Encoding.ASCII.GetBytes(password);
                    byte[] pt_password = new byte[salt.Length + password_bytes.Length];
                    byte[] h_password;
                    password_bytes.CopyTo(pt_password, 0);
                    salt.CopyTo(pt_password, password.Length);
                    h_password = hashAlgorithm.ComputeHash(pt_password);
                    for (int i = 0; i < h_password.Length; i++)
                    {
                        if (h_password[i] != hashed_password[i])
                        {
                            correct_password = false;
                            break;
                        }
                    }
                }
                else
                {
                    user_found = false;
                }

                if (user_found && correct_password)
                {
                    token = new JwtBuilder()
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        .WithSecret(ConfigurationManager.AppSettings["secret_key"].ToString())
                        .AddClaim("exp", Convert.ToUInt64(DateTimeOffset.UtcNow.AddDays(15).ToUnixTimeSeconds()))
                        .AddClaim("_id", Convert.ToString(reader["_id"]))
                        .AddClaim("username", Convert.ToString(reader["username"]))
                        .AddClaim("email_id", Convert.ToString(reader["email_id"]))
                        .Encode();
                } 
                else
                {
                    if (!user_found)
                    {
                        throw new FaultException<AuthenticationFault>(new AuthenticationFault(AuthenticationFault.AuthenticationFaultType.NoSuchUser));
                    }
                    else if (!correct_password)
                    {
                        throw new FaultException<AuthenticationFault>(new AuthenticationFault(AuthenticationFault.AuthenticationFaultType.InvalidPassword));
                    } 
                    else
                    {
                        throw new FaultException<AuthenticationFault>(new AuthenticationFault(AuthenticationFault.AuthenticationFaultType.ServerFault));
                    }
                }
            }

            return token;
        }
    }
}

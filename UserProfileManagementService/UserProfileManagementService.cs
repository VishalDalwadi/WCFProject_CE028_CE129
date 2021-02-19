using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.ServiceModel;

namespace UserProfileManagementService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RegistrationService" in both code and config file together.
    public class UserProfileManagementService : IUserProfileManagementService
    {
        void IUserProfileManagementService.RegisterUser(User user)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            HashAlgorithm hashAlgorithm = new SHA512Managed();
            byte[] salt = new byte[32];
            byte[] password = Encoding.ASCII.GetBytes(user.Password);
            byte[] pt_password = new byte[salt.Length + password.Length];
            byte[] hashed_password;
            password.CopyTo(pt_password, 0);
            rng.GetBytes(salt);
            salt.CopyTo(pt_password, password.Length);
            hashed_password = hashAlgorithm.ComputeHash(pt_password);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Users (username, email_id, hashed_password, salt) VALUES (@username, @email_id, @hashed_password, @salt)", conn);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@email_id", user.EmailID);
                command.Parameters.Add("@hashed_password", System.Data.SqlDbType.Binary, hashed_password.Length).Value = hashed_password;
                command.Parameters.Add("@salt", System.Data.SqlDbType.Binary, salt.Length).Value = salt;
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        bool IUserProfileManagementService.IsUsernameTaken(string username)
        {
            bool returnVal = false;
            string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT username FROM Users WHERE username = @username", conn);
                command.Parameters.AddWithValue("@username", username);
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    returnVal = true;
                }
            }

            return returnVal;
        }

        bool IUserProfileManagementService.UserWithEmailIdExists(string email_id)
        {
            bool returnVal = false;
            string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT username FROM Users WHERE email_id = @email_id", conn);
                command.Parameters.AddWithValue("@email_id", email_id);
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    returnVal = true;
                }
            }

            return returnVal;
        }

        void IUserProfileManagementService.SendPasswordResetToken(string email_id)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

                using (RegistrationService.EmailServiceReference.EmailServiceClient client = new RegistrationService.EmailServiceReference.EmailServiceClient())
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    byte[] token = new byte[32];
                    rng.GetBytes(token);
                    string message = "Here's your one-time token. This token expires in 24 hours.\n";
                    message += "TOKEN: " + GetString(token);
                    
                    Int64 user_id = -1;
                    SqlCommand command = new SqlCommand("SELECT _id FROM Users WHERE email_id = @email_id", conn);
                    command.Parameters.AddWithValue("@email_id", email_id);
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        user_id = (long)reader["_id"];
                    }

                    if (user_id == -1)
                    {
                        throw new FaultException("No user with given email id.");
                    }
                    else
                    {
                        command = new SqlCommand("IF EXISTS(SELECT * FROM EmailTokens WHERE user_id = @user_id)" +
                            " UPDATE EmailTokens SET token = @token WHERE user_id = @user_id" +
                            " ELSE" +
                            " INSERT INTO EmailTokens (user_id, token) VALUES (@user_id, @token);", conn);
                        command.Parameters.AddWithValue("@user_id", Convert.ToString(user_id));
                        command.Parameters.Add("@token", System.Data.SqlDbType.Binary, token.Length).Value = token;
                        command.ExecuteNonQuery();
                    }
                    client.SendEmail(email_id, "ChessOnline - Password Reset Token", message, false);

                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        void IUserProfileManagementService.ResetPassword(string token, string email_id, string new_password)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

                using (RegistrationService.EmailServiceReference.EmailServiceClient client = new RegistrationService.EmailServiceReference.EmailServiceClient())
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    byte[] token_bytes;

                    Int64 user_id = -1;
                    SqlCommand command = new SqlCommand("SELECT _id FROM Users WHERE email_id = @email_id", conn);
                    command.Parameters.AddWithValue("@email_id", email_id);
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        user_id = (long)reader["_id"];
                    }

                    if (user_id == -1)
                    {
                        throw new FaultException("No user with given email id.");
                    }
                    else
                    {
                        command = new SqlCommand("SELECT token FROM EmailTokens WHERE user_id = @user_id", conn);
                        command.Parameters.AddWithValue("@user_id", user_id);
                        reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            token_bytes = (byte[])reader["token"];
                            string expected_token = GetString(token_bytes);
                            if (expected_token.Equals(token))
                            {
                                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                                HashAlgorithm hashAlgorithm = new SHA512Managed();
                                byte[] salt = new byte[32];
                                byte[] password = Encoding.ASCII.GetBytes(new_password);
                                byte[] pt_password = new byte[salt.Length + password.Length];
                                byte[] hashed_password;
                                password.CopyTo(pt_password, 0);
                                rng.GetBytes(salt);
                                salt.CopyTo(pt_password, password.Length);
                                hashed_password = hashAlgorithm.ComputeHash(pt_password);

                                command = new SqlCommand("UPDATE Users SET hashed_password = @hashed_password, salt = @salt WHERE _id = @_id", conn);
                                command.Parameters.Add("@hashed_password", System.Data.SqlDbType.Binary, hashed_password.Length).Value = hashed_password;
                                command.Parameters.Add("@salt", System.Data.SqlDbType.Binary, salt.Length).Value = salt;
                                command.Parameters.AddWithValue("@_id", user_id);
                                command.ExecuteNonQuery();
                            }
                            else
                            {
                                throw new FaultException("InvalidToken");
                            }
                        }
                        else
                        {
                            throw new FaultException("NoTokenFound");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        void IUserProfileManagementService.DeleteUser(string jwtToken)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

                using (RegistrationService.UPMAuthorizationServiceReference.AuthorizationServiceClient client = new RegistrationService.UPMAuthorizationServiceReference.AuthorizationServiceClient())
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    RegistrationService.UPMAuthorizationServiceReference.User user = client.AuthorizeUser(jwtToken);
                    SqlCommand command = new SqlCommand("DELETE FROM Users WHERE _id = @_id", conn);
                    command.Parameters.AddWithValue("@_id", user.Id);
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (FaultException<RegistrationService.UPMAuthorizationServiceReference.AuthorizationFault> ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new FaultException("ServerFault");
            }
        }

        static string GetString(byte[] bytes)
        {
            string returnVal = "";

            foreach (byte b in bytes)
            {
                if (Char.IsLetter((char)b))
                {
                    returnVal += Convert.ToString((char)b);
                }
                else
                {
                    returnVal += Convert.ToString(b);
                }
            }

            return returnVal;
        }
    }
}

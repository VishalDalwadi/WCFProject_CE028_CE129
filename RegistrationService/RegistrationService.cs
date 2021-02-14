using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace RegistrationService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RegistrationService" in both code and config file together.
    public class RegistrationService : IRegistrationService
    {
        void IRegistrationService.RegisterUser(User user)
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

        bool IRegistrationService.IsUsernameTaken(string username)
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

        bool IRegistrationService.UserWithEmailIdExists(string email_id)
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
    }
}

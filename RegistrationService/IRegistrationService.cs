using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RegistrationService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRegistrationService" in both code and config file together.
    [ServiceContract]
    public interface IRegistrationService
    {
        [OperationContract]
        void RegisterUser(User user);

        [OperationContract]
        bool IsUsernameTaken(string username);

        [OperationContract]
        bool UserWithEmailIdExists(string email_id);
    }

    [DataContract]
    public class User
    {
        private string _username;
        private string _email_id;
        private string _password;

        [DataMember]
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        [DataMember]
        public string EmailID
        {
            get { return _email_id; }
            set { _email_id = value; }
        }

        [DataMember]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}

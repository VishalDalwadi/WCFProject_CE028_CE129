using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AuthorizationService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAuthorizationService" in both code and config file together.
    [ServiceContract]
    public interface IAuthorizationService
    {
        [OperationContract]
        [FaultContract(typeof(AuthorizationFault))]
        User AuthorizeUser(string token);
    }

    [DataContract]
    public class User
    {
        private Int64 _id;
        private string _username;
        private string _email_id;

        [DataMember]
        public Int64 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [DataMember]
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        [DataMember]
        public string Email_Id
        {
            get { return _email_id; }
            set { _email_id = value; }
        }
    }

    [DataContract]
    public class AuthorizationFault
    {
        public enum AuthorizationFaultType
        {
            TokenExpired,
            InvalidSignature
        }

        private AuthorizationFaultType _faultType;

        [DataMember]
        public AuthorizationFaultType FaultType
        {
            get { return _faultType; }
        }

        public AuthorizationFault(AuthorizationFaultType type)
        {
            _faultType = type;
        }
    }
}

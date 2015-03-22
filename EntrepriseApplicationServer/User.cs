using System.Collections.Generic;
using System.Runtime.Serialization;
namespace EntrepriseApplicationServer
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "message_type", IsRequired = true)]
        private int _messageType;

        [DataMember(Name = "first_name", IsRequired = true)]
        private string _firstName;

        [DataMember(Name = "last_name", IsRequired = true)]
        private string _lastName;

        [DataMember(Name = "password", IsRequired = false)]
        private string _password;

        [DataMember(Name = "email", IsRequired = true)]
        private string _email;

        [DataMember(Name = "birthday", IsRequired = true)]
        private string _birthday;

        [DataMember(Name = "sexe", IsRequired = true)]
        private string _sexe;

        [DataMember(Name = "isConnected", IsRequired = true)]
        private bool _isConnected;

        [DataMember(Name = "userInscriptionDate", IsRequired = false)]
        private string _userInscriptionDate;

        [DataMember(Name = "lastConnectionDate", IsRequired = false)]
        private string _lastConnectionDate;

        [DataMember(Name = "ID", IsRequired = false)]
        private int _id;


        // Use only for test !
        public User(string firstName, string lastName, string email, string birthday, string sexe,
            bool isConnected, string userInscriptionDate = "", string lastConnectionDate = "", string password = "")
        {
            _messageType = (int)MessageType.USER_INFORMATION;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _birthday = birthday;
            _sexe = sexe;
            _isConnected = isConnected;
            _userInscriptionDate = userInscriptionDate;
            _lastConnectionDate = lastConnectionDate;
            _password = password;
        }

        public override bool Equals(object obj)
        {
            if (obj is User)
            {
                User u = obj as User;
                return (_firstName == u._firstName && _lastName == u._lastName && _password == u._password &&
                        _email == u._email
                        && _birthday == u._birthday && _isConnected == u._isConnected &&
                        _lastConnectionDate == u._lastConnectionDate
                        && _userInscriptionDate == u._userInscriptionDate);
            }
            return base.Equals(obj);
        }
    }
}

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace EntrepriseApplicationServer
{
    [DataContract]
    [KnownType(typeof (HashSet<int>))]
    public class FriendList
    {
        [DataMember(Name = "message_type", IsRequired = true)] private int _messageType;

        [DataMember(Name = "ID", IsRequired = true)] private int _id;

        [DataMember(Name = "contact_list", IsRequired = false)] private HashSet<int> _contactList;

        public FriendList(int id, HashSet<int> contactList)
        {
            _messageType = (int) MessageType.FRIEND_LIST;
            _id = id;
            _contactList = contactList;
        }

        public override bool Equals(object obj)
        {
            if (obj is FriendList)
            {
                FriendList fObj = obj as FriendList;
                return fObj._contactList.SetEquals(_contactList) && fObj._messageType == _messageType && fObj._id == _id;
            }
            return base.Equals(obj);
        }
    }
}

using System.Runtime.Serialization;

namespace EntrepriseApplicationServer
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "message_type", IsRequired = true)]
        private int _message_type;

        [DataMember(Name = "id_sender", IsRequired = true)]
        private int _idSender;

        [DataMember(Name = "id_receiver", IsRequired = true)]
        private int _idReceiver;

        [DataMember(Name = "message_text", IsRequired = true)]
        private string _messageText;

        [DataMember(Name = "date_message", IsRequired = false)]
        private string _dateMessage;

        public Message(int idSender, int idReceiver, string messageText, string dateMessage)
        {
            _message_type = (int)MessageType.MESSAGE;
            _idReceiver = idReceiver;
            _idSender = idSender;
            _messageText = messageText;
            _dateMessage = dateMessage;
        }

        public override bool Equals(object obj)
        {
            if (obj is Message)
            {
                Message m = obj as Message;
                return (m._dateMessage == _dateMessage && m._messageText == _messageText && m._idReceiver == _idReceiver &&
                        m._idSender == _idSender);
            }
            return base.Equals(obj);
        }
    }
}

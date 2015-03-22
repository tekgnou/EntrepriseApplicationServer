using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using EntrepriseApplicationServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Message = EntrepriseApplicationServer.Message;

namespace EntrepriseApplicationServerTest
{
    /// <summary>
    /// Summary description for SerializationTest
    /// </summary>
    [TestClass]
    public class SerializationTest
    {
        public SerializationTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        #region User Test

        [TestMethod]
        public void UserDeserializationCorrectFormat_1()
        {
            StreamReader s = new StreamReader("UserCorrectFormat_1.json", Encoding.UTF8);
 
            var user = JsonValue.Load(s) as JsonObject;
            Assert.IsNotNull(user);
            Assert.IsTrue((int)user["message_type"] == (int)MessageType.USER_INFORMATION);

            s.Close();
            s = new StreamReader("UserCorrectFormat_1.json", Encoding.UTF8);
            DataContractJsonSerializer jsonUser = new DataContractJsonSerializer(typeof(User));
            MemoryStream m = new MemoryStream(Encoding.ASCII.GetBytes(s.ReadToEnd()));
            User userClass = jsonUser.ReadObject(m) as User;

            User couvignouUser = new User("Vincent", "Couvignou", "vincent.couvignou@epitech.eu", "16/09/1990", "masculin", false, "19/03/2015", "19/03/2015", "test_password");
            Assert.IsTrue(couvignouUser.Equals(userClass));
            s.Close();
        }

        [TestMethod]
        public void UserDeserializationCorrectFormat_2()
        {
            StreamReader s = new StreamReader("UserCorrectFormat_2.json", Encoding.UTF8);

            var user = JsonValue.Load(s) as JsonObject;
            Assert.IsNotNull(user);
            Assert.IsTrue((int)user["message_type"] == (int)MessageType.USER_INFORMATION);

            s.Close();
            s = new StreamReader("UserCorrectFormat_2.json", Encoding.UTF8);
            DataContractJsonSerializer jsonUser = new DataContractJsonSerializer(typeof(User));
            MemoryStream m = new MemoryStream(Encoding.ASCII.GetBytes(s.ReadToEnd()));
            User userClass = jsonUser.ReadObject(m) as User;

            User couvignouUser = new User("Vincent", "Couvignou", "vincent.couvignou@epitech.eu", "16/09/1990", "masculin", false, null, null, "test_password");
            Assert.IsTrue(couvignouUser.Equals(userClass));
            s.Close();
        }

        [TestMethod]
        public void UserSerialization()
        {
            User couvignouUser = new User("Vincent", "Couvignou", "vincent.couvignou@epitech.eu", "16/09/1990", "masculin", false, "19/03/2015", "19/03/2015", "test_password");            
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(User));
            MemoryStream m = new MemoryStream();
            js.WriteObject(m, couvignouUser);

            m.Position = 0;
            StreamReader s_object = new StreamReader(m);
            StreamReader s_file = new StreamReader("UserSerializationCorrectFormat.json", Encoding.UTF8);
            DataContractJsonSerializer jsonUser = new DataContractJsonSerializer(typeof(User));
            MemoryStream mUserFile = new MemoryStream(Encoding.ASCII.GetBytes(s_file.ReadToEnd()));
            MemoryStream mUserObject = new MemoryStream(Encoding.ASCII.GetBytes(s_object.ReadToEnd()));
            User userFile = jsonUser.ReadObject(mUserFile) as User;
            User serializationUser = jsonUser.ReadObject(mUserObject) as User;
            Assert.IsNotNull(userFile);
            Assert.IsTrue(userFile.Equals(serializationUser));
        }

        //Missing required field
        [TestMethod]
        [ExpectedException(typeof(System.Runtime.Serialization.SerializationException))]
        public void UserDeserializationUncorrectFormat_1()
        {
            StreamReader s = new StreamReader("UserUncorrectFormat_1.json", Encoding.UTF8);

            var user = JsonValue.Load(s) as JsonObject;
            Assert.IsNotNull(user);
            Assert.IsTrue((int)user["message_type"] == (int)MessageType.USER_INFORMATION);

            s = new StreamReader("UserUncorrectFormat_1.json", Encoding.UTF8);
            DataContractJsonSerializer jsonUser = new DataContractJsonSerializer(typeof(User));
            MemoryStream m = new MemoryStream(Encoding.ASCII.GetBytes(s.ReadToEnd()));
            jsonUser.ReadObject(m);
        }

        #endregion

        #region FriendList Test
        [TestMethod]
        public void FriendListDeserializationCorrectFormat_1()
        {
            StreamReader s = new StreamReader("FriendListCorrectFormat_1.json", Encoding.UTF8);

            var user = JsonValue.Load(s) as JsonObject;
            Assert.IsNotNull(user);
            Assert.IsTrue((int)user["message_type"] == (int)MessageType.FRIEND_LIST);

            s = new StreamReader("FriendListCorrectFormat_1.json", Encoding.UTF8);
            DataContractJsonSerializer jsonFriendList = new DataContractJsonSerializer(typeof(FriendList));
            MemoryStream m = new MemoryStream(Encoding.ASCII.GetBytes(s.ReadToEnd()));
            FriendList friendListClass = jsonFriendList.ReadObject(m) as FriendList;
            HashSet<int> f_contactList = new HashSet<int>();
            f_contactList.Add(0);
            f_contactList.Add(2);
            f_contactList.Add(3);
            FriendList friendListObject = new FriendList(1, f_contactList);

            Assert.IsTrue(friendListObject.Equals(friendListClass));
        }

        // Not the same FriendList.
        [TestMethod]
        public void FriendListDeserializationNotSameObject()
        {
            StreamReader s = new StreamReader("FriendListCorrectFormat_1.json", Encoding.UTF8);

            var user = JsonValue.Load(s) as JsonObject;
            Assert.IsNotNull(user);
            Assert.IsTrue((int)user["message_type"] == (int)MessageType.FRIEND_LIST);

            s = new StreamReader("FriendListCorrectFormat_1.json", Encoding.UTF8);
            DataContractJsonSerializer jsonFriendList = new DataContractJsonSerializer(typeof(FriendList));
            MemoryStream m = new MemoryStream(Encoding.ASCII.GetBytes(s.ReadToEnd()));
            FriendList friendListClass = jsonFriendList.ReadObject(m) as FriendList;
            HashSet<int> f_contactList = new HashSet<int>();
            f_contactList.Add(1);
            f_contactList.Add(4);
            f_contactList.Add(3);
            FriendList friendListObject = new FriendList(1, f_contactList);

            Assert.IsNotNull(friendListClass);
            Assert.IsFalse(friendListObject.Equals(friendListClass));
        }

        [TestMethod]
        public void FriendListSerialization()
        {
            HashSet<int> f_contactList = new HashSet<int>();
            f_contactList.Add(1);
            f_contactList.Add(2);
            f_contactList.Add(3);
            FriendList f = new FriendList(0, f_contactList);
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(FriendList));
            MemoryStream m = new MemoryStream();
            js.WriteObject(m, f);

            m.Position = 0;
            StreamReader s_object = new StreamReader(m);
            StreamReader s_file = new StreamReader("FriendListSerializationCorrectFormat.json", Encoding.UTF8);
            DataContractJsonSerializer jsonFriendList = new DataContractJsonSerializer(typeof(FriendList));
            MemoryStream mFriendList = new MemoryStream(Encoding.ASCII.GetBytes(s_file.ReadToEnd()));
            MemoryStream mFriendListObject = new MemoryStream(Encoding.ASCII.GetBytes(s_object.ReadToEnd()));
            FriendList friendListFile = jsonFriendList.ReadObject(mFriendList) as FriendList;
            FriendList friendListObject = jsonFriendList.ReadObject(mFriendListObject) as FriendList;
            Assert.IsNotNull(friendListFile);
            Assert.IsTrue(friendListFile.Equals(friendListObject));
        }


        #endregion

        #region Message Test

        [TestMethod]
        public void MessageDeserializationCorrectFormat_1()
        {
            StreamReader s = new StreamReader("MessageDeserializationCorrectFormat_1.json", Encoding.UTF8);

            var message = JsonValue.Load(s) as JsonObject;
            Assert.IsNotNull(message);
            Assert.IsTrue((int)message["message_type"] == (int)MessageType.MESSAGE);

            s.Close();
            s = new StreamReader("MessageDeserializationCorrectFormat_1.json", Encoding.UTF8);
            DataContractJsonSerializer jsonUser = new DataContractJsonSerializer(typeof(Message));
            MemoryStream m = new MemoryStream(Encoding.ASCII.GetBytes(s.ReadToEnd()));
            Message messageClass = jsonUser.ReadObject(m) as Message;

            Message mObject = new Message(1, 2, "Message dans le bon format", "19/03/2015");
            Assert.IsTrue(mObject.Equals(messageClass));
            s.Close();
        }

        [TestMethod]
        public void MessageDeserializationCorrectFormat_2()
        {
            StreamReader s = new StreamReader("MessageDeserializationCorrectFormat_2.json", Encoding.UTF8);

            var message = JsonValue.Load(s) as JsonObject;
            Assert.IsNotNull(message);
            Assert.IsTrue((int)message["message_type"] == (int)MessageType.MESSAGE);

            s.Close();
            s = new StreamReader("MessageDeserializationCorrectFormat_2.json", Encoding.UTF8);
            DataContractJsonSerializer jsonUser = new DataContractJsonSerializer(typeof(Message));
            MemoryStream m = new MemoryStream(Encoding.ASCII.GetBytes(s.ReadToEnd()));
            Message messageClass = jsonUser.ReadObject(m) as Message;

            Message mObject = new Message(1, 2, "Message dans le bon format", null);
            Assert.IsTrue(mObject.Equals(messageClass));
            s.Close();
        }

        [TestMethod]
        public void MessageSerialization()
        {
            Message message = new Message(0, 1, "Serialization - Message dans le bon format", "21/03/2015");
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Message));
            MemoryStream m = new MemoryStream();
            js.WriteObject(m, message);

            m.Position = 0;
            StreamReader s_object = new StreamReader(m);
            StreamReader s_file = new StreamReader("MessageSerializationCorrectFormat.json", Encoding.UTF8);
            DataContractJsonSerializer jsonUser = new DataContractJsonSerializer(typeof(Message));
            MemoryStream mMessageFile = new MemoryStream(Encoding.ASCII.GetBytes(s_file.ReadToEnd()));
            MemoryStream mMessageObject = new MemoryStream(Encoding.ASCII.GetBytes(s_object.ReadToEnd()));
            Message messageFile = jsonUser.ReadObject(mMessageFile) as Message;
            Message serializationMessage = jsonUser.ReadObject(mMessageObject) as Message;
            Assert.IsNotNull(messageFile);
            Assert.IsTrue(messageFile.Equals(serializationMessage));
        }

        //Missing required field
        [TestMethod]
        [ExpectedException(typeof(System.Runtime.Serialization.SerializationException))]
        public void MessageDeserializationUncorrectFormat_1()
        {
            StreamReader s = new StreamReader("MessageDeserializationUncorrectFormat.json", Encoding.UTF8);

            var user = JsonValue.Load(s) as JsonObject;
            Assert.IsNotNull(user);
            Assert.IsTrue((int)user["message_type"] == (int)MessageType.MESSAGE);

            s = new StreamReader("MessageDeserializationUncorrectFormat.json", Encoding.UTF8);
            DataContractJsonSerializer jsonUser = new DataContractJsonSerializer(typeof(Message));
            MemoryStream m = new MemoryStream(Encoding.ASCII.GetBytes(s.ReadToEnd()));
            jsonUser.ReadObject(m);
        }

        #endregion

        [TestMethod]
        public void DeserializationMultipleMessageEntrepriseApplication()
        {
            Message message = new Message(0, 1, "Serialization - Message dans le bon format", "21/03/2015");
            User user = new User("Vincent", "Couvignou", "vcouvignou@email.com", "19/03/2015", "masculin", true, null, null, null);
            DataContractJsonSerializer jsMessage = new DataContractJsonSerializer(typeof(Message));
            DataContractJsonSerializer jsUser = new DataContractJsonSerializer(typeof(User));
            MemoryStream m = new MemoryStream();
            jsUser.WriteObject(m, user);
            jsMessage.WriteObject(m, message);

            m.Position = 0;
            StreamReader s = new StreamReader(m, Encoding.UTF8);
            List<string> listJson = ParseStream.ParseStreamToString(s);
            Assert.IsTrue(listJson.Count == 2);

            Message me = null;
            User u = null;
            foreach (var jsonItem in listJson)
            {
                StreamReader itemStream = new StreamReader(ParseStream.GenerateStreamFromString(jsonItem), Encoding.UTF8);
                var userObject = JsonValue.Load(itemStream) as JsonObject;
                Assert.IsNotNull(userObject);
                MemoryStream mItem = new MemoryStream(Encoding.ASCII.GetBytes(jsonItem));
                if ((int)userObject["message_type"] == (int)MessageType.USER_INFORMATION)
                {
                    u = jsUser.ReadObject(mItem) as User;
                    Assert.IsTrue(true);

                }
                else if ((int)userObject["message_type"] == (int)MessageType.MESSAGE)
                {
                    me = jsMessage.ReadObject(mItem) as Message;
                    Assert.IsTrue(true);
                }
                else
                    Assert.IsTrue(false);
            }

            Assert.IsNotNull(u);
            Assert.IsNotNull(me);
            Assert.IsTrue(me.Equals(message));
            Assert.IsTrue(u.Equals(user) && me.Equals(message));
        }

    }
}

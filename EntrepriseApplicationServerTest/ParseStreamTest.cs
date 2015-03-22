using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntrepriseApplicationServer;
using Message = EntrepriseApplicationServer.Message;

namespace EntrepriseApplicationServerTest
{
    [TestClass]
    public class ParseStreamTest
    {
        [TestMethod]
        public void ParseStreamToStringTest_1()
        {
            StreamReader s = new StreamReader("UserCorrectFormat_1.json", Encoding.UTF8);

            List<string> listJson = ParseStream.ParseStreamToString(s);
            Assert.IsTrue(listJson.Count == 1);
            MemoryStream memory = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memory);
            streamWriter.Write(listJson[0]);
            streamWriter.Flush();
            memory.Position = 0;
            var user = JsonValue.Load(memory) as JsonObject;
            Assert.IsNotNull(user);
            Assert.IsTrue((int)user["message_type"] == (int)MessageType.USER_INFORMATION);

            s.Close();
            s = new StreamReader("UserCorrectFormat_1.json", Encoding.UTF8);
            DataContractJsonSerializer jsonUser = new DataContractJsonSerializer(typeof(User));
            MemoryStream m = new MemoryStream(Encoding.ASCII.GetBytes(s.ReadToEnd()));
            User userClass = jsonUser.ReadObject(m) as User;
        }


        [TestMethod]
        public void ParseStreamToStringTestMultipleJson_1()
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
                if ((int) userObject["message_type"] == (int) MessageType.USER_INFORMATION)
                {
                    u = jsUser.ReadObject(mItem) as User;
                    Assert.IsTrue(true);

                }
                else if ((int) userObject["message_type"] == (int) MessageType.MESSAGE)
                {
                    me = jsMessage.ReadObject(mItem) as Message;
                    Assert.IsTrue(true);
                }
                else
                    Assert.IsTrue(false);
            }
        }
    }
}

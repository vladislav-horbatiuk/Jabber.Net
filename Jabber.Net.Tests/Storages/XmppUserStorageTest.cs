using agsXMPP.protocol.iq.last;
using agsXMPP.protocol.iq.vcard;
using Jabber.Net.Server;
using Jabber.Net.Server.Storages;
using NUnit.Framework;

namespace Jabber.Net.Tests.Storages
{
    [TestFixture]
    public class XmppUserStorageTest
    {
        private readonly IXmppUserStorage storage;
        private const string username = "user";


        public XmppUserStorageTest()
        {
            storage = new XmppUserStorage("users", new XmppElementStorage("elements"));
        }


        [Test]
        public void UserTest()
        {
            storage.RemoveUser(username);

            var u1 = new XmppUser(username, "password");
            storage.SaveUser(u1);

            var u2 = storage.GetUser(username);
            Assert.AreEqual(u1.Name, u2.Name);
            Assert.AreEqual(u1.Password, u2.Password);

            u1 = new XmppUser(username, "password2");
            storage.SaveUser(u1);

            u2 = storage.GetUser(username);
            Assert.AreEqual(u1.Name, u2.Name);
            Assert.AreEqual(u1.Password, u2.Password);

            u2 = storage.GetUser("sss");
            Assert.IsNull(u2);

            storage.RemoveUser(username);
            u2 = storage.GetUser(username);
            Assert.IsNull(u2);
        }

        [Test]
        public void VCardTest()
        {
            storage.RemoveUser(username);

            var vcard1 = new Vcard { Fullname = username };
            storage.SetVCard(username, vcard1);
            var vcard2 = storage.GetVCard(username);
            Assert.IsNull(vcard2);

            var u = new XmppUser(username, "password");
            storage.SaveUser(u);

            vcard2 = storage.GetVCard(username);
            Assert.IsNull(vcard2);

            storage.SetVCard(username, vcard1);
            vcard2 = storage.GetVCard(username);
            Assert.AreEqual(vcard1.ToString(), vcard2.ToString());

            vcard2 = storage.GetVCard("sss");
            Assert.IsNull(vcard2);

            storage.SetVCard(username, null);
            vcard2 = storage.GetVCard(username);
            Assert.IsNull(vcard2);

            storage.RemoveUser(username);
        }
    }
}

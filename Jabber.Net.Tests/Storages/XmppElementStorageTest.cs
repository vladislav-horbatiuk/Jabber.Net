using System.Linq;
using agsXMPP;
using agsXMPP.protocol.client;
using Jabber.Net.Server.Storages;
using NUnit.Framework;

namespace Jabber.Net.Tests.Storages
{
    [TestFixture]
    public class XmppElementStorageTest
    {
        private readonly IXmppElementStorage storage;


        public XmppElementStorageTest()
        {
            storage = new XmppElementStorage("elements");
        }


        [Test]
        public void ElementsTest()
        {
            var e1 = new Message()
            {
                From = new Jid("from@s/R"),
                To = new Jid("to@s/R"),
                Type = MessageType.headline,
                Body = "body",
                Subject = "subject",
            };
            var e2 = new Message()
            {
                From = new Jid("from@s/R"),
                To = new Jid("to@s/R"),
                Type = MessageType.headline,
                Body = "body",
                Subject = "subject",
            };
            var key = "key";

            storage.SaveElement(e1.To, key + "|1", e1);
            storage.SaveElement(e2.To, key + "|2", e2);

            var fromdb = storage.GetElements(Jid.Empty, key);
            CollectionAssert.IsEmpty(fromdb);

            fromdb = storage.GetElements(e1.To, key + "%");
            Assert.AreEqual(2, fromdb.Count());
            Assert.AreEqual(e1.ToString(), fromdb.ElementAt(0).ToString());
            Assert.AreEqual(e2.ToString(), fromdb.ElementAt(1).ToString());

            var frome = storage.GetElement(e1.To, key + "|1");
            Assert.AreEqual(e1.ToString(), frome.ToString());
            frome = storage.GetElement(e2.To, key + "|2");
            Assert.AreEqual(e2.ToString(), frome.ToString());

            fromdb = storage.GetElements(e1.To, key + "|1");
            Assert.AreEqual(1, fromdb.Count());
            Assert.AreEqual(e1.ToString(), fromdb.ElementAt(0).ToString());

            storage.RemoveElements(e1.To, key + "|1");

            fromdb = storage.GetElements(e1.To, key + "|1");
            CollectionAssert.IsEmpty(fromdb);

            fromdb = storage.GetElements(e1.To, "%");
            Assert.AreEqual(1, fromdb.Count());
            Assert.AreEqual(e2.ToString(), fromdb.ElementAt(0).ToString());

            storage.RemoveElements(e1.To, "%");

            fromdb = storage.GetElements(e1.To, key);
            CollectionAssert.IsEmpty(fromdb);
        }
    }
}

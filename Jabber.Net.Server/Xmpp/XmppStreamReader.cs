using System;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.iq.last;
using agsXMPP.protocol.iq.@private;
using agsXMPP.protocol.iq.register;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.session;
using agsXMPP.protocol.iq.time;
using agsXMPP.protocol.iq.vcard;
using agsXMPP.protocol.iq.version;
using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

namespace Jabber.Net.Server.Xmpp
{
    class XmppStreamReader
    {
        private readonly StreamParser parser;
        private readonly System.IO.Stream stream;
        private volatile bool canceled;


        public event EventHandler<XmppStreamArgs> ReadElementComleted;


        public XmppStreamReader(System.IO.Stream stream)
        {
            Args.NotNull(stream, "stream");

            this.stream = stream;
            this.parser = new StreamParser();

            parser.OnStreamStart += OnElement;
            parser.OnStreamElement += OnElement;
            parser.OnStreamEnd += OnElement;
            parser.OnStreamError += OnError;
            parser.OnError += OnError;
        }


        public void ReadElementAsync()
        {
            try
            {
                var buffer = new byte[1024];
                stream.BeginRead(buffer, 0, buffer.Length, ReadCallback, buffer);
            }
            catch (Exception ex)
            {
                OnError(this, ex);
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                var buffer = (byte[])ar.AsyncState;
                var readed = stream.EndRead(ar);

                if (0 < readed)
                {
                    parser.Push(buffer, 0, readed);

                    if (!canceled)
                    {
                        stream.BeginRead(buffer, 0, buffer.Length, ReadCallback, buffer);
                    }
                }
                else
                {
                    OnElement(parser, null);
                }
            }
            catch (Exception ex)
            {
                OnError(parser, ex);
            }
        }

        public void ReadElementCancel()
        {
            canceled = true;
        }


        private void OnElement(object sender, Node node)
        {
            var ev = ReadElementComleted;
            if (ev != null)
            {
                if (node is Element)
                {
                    ev(this, new XmppStreamArgs(XmppStreamState.Success, CorrectType((Element)node), null));
                }
                else if (node == null)
                {
                    ev(this, new XmppStreamArgs(XmppStreamState.Closed));
                }
            }
        }

        private void OnError(object sender, Exception ex)
        {
            var ev = ReadElementComleted;
            if (ev != null)
            {
                ev(this, new XmppStreamArgs(XmppStreamState.Error, null, ex));
            }
        }

        private Element CorrectType(Element element)
        {
            var iq = element as IQ;
            if (iq != null)
            {
                if (iq.SelectSingleElement<Bind>() != null)
                {
                    return new BindIq(iq);
                }
                else if (iq.SelectSingleElement<Session>() != null)
                {
                    return new SessionIq(iq);
                }
                else if (iq.SelectSingleElement("vCard") as Vcard != null)
                {
                    return new VcardIq(iq);
                }
                else if (iq.SelectSingleElement<DiscoInfo>() != null)
                {
                    return new DiscoInfoIq(iq);
                }
                else if (iq.SelectSingleElement<DiscoItems>() != null)
                {
                    return new DiscoItemsIq(iq);
                }
                else if (iq.SelectSingleElement<Last>() != null)
                {
                    return new LastIq(iq);
                }
                else if (iq.SelectSingleElement<agsXMPP.protocol.iq.version.Version>() != null)
                {
                    return new VersionIq(iq);
                }
                else if (iq.SelectSingleElement<Private>() != null)
                {
                    return new PrivateIq(iq);
                }
                else if (iq.SelectSingleElement<Register>() != null)
                {
                    return new RegisterIq(iq);
                }
                else if (iq.SelectSingleElement<Roster>() != null)
                {
                    return new RosterIq(iq);
                }
                else if (iq.SelectSingleElement<EntityTime>() != null)
                {
                    return new EntityTimeIq(iq);
                }
            }
            var stream = element as agsXMPP.protocol.Base.Stream;
            if (stream != null)
            {
                if (parser.DefaultNamespace == agsXMPP.Uri.CLIENT)
                {
                    return new Stream(stream, parser.DefaultNamespace);
                }
                if (parser.DefaultNamespace == agsXMPP.Uri.ACCEPT)
                {
                    return new agsXMPP.protocol.component.Stream(stream, parser.DefaultNamespace);
                }
            }
            return element;
        }
    }
}

namespace agsXMPP.protocol.iq.jingle
{
    using Xml.Dom;

    public class Jingle : Element
    {
        public Jingle()
        {
            TagName = "jingle";
            Namespace	= Uri.IQ_JINGLE1;
        }
        
    }
}
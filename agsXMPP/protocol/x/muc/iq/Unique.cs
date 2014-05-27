namespace agsXMPP.protocol.x.muc.iq
{
    using Xml.Dom;

    public class Unique:Element
    {
        public Unique()
        {
            TagName = "unique";
            Namespace = Uri.MUC_UNIQUE;
        }
    }
}
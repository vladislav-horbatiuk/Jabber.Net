namespace agsXMPP.protocol.x.muc
{
    using client;
    using iq;

    public class UniqueIQ:IQ
    {
        public virtual Unique Unique
        {
            get { return SelectSingleElement("unique") as Unique; }

            set
            {
                if (value != null)
                {
                    ReplaceChild(value);
                }
                else
                {
                    RemoveTag("unique");
                }
            }
        }
    }
}
namespace agsXMPP.protocol.extensions.multicast
{
    using Xml.Dom;

    public class Addresses: Element
    {
        public Addresses()
		{
            this.TagName = "addresses";
			this.Namespace	= Uri.ADDRESS;			
		}

        public Address AddAddress(Address address)
        {
            AddChild(address);
            return address;
        }

        public Jid[] GetAddressList()
        {
            ElementList nl = SelectElements("address");
            Jid[] addresses = new Jid[nl.Count];

            int i = 0;
            foreach (Element e in nl)
            {
                addresses[i] = ((Address)e).Jid;
                i++;
            }
            return addresses;
        }

        public void RemoveAllBcc()
        {
            foreach (Address address in GetAddresses())
            {
                if (address.Type == AddressType.bcc)
                {
                    address.Remove();
                }
            }
        }

        public Address[] GetAddresses()
        {
            ElementList nl = SelectElements("address");
            Address[] addresses = new Address[nl.Count];

            int i = 0;
            foreach (Element e in nl)
            {
                addresses[i] = (Address)e;
                i++;
            }
            return addresses;
        }
    }
}
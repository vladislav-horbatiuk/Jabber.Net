using System;
using agsXMPP.Xml.Dom;

namespace agsXMPP.protocol.extensions.bosh
{
	public class BodyEnd : Element
	{
        public override string ToString()
        {
            return "</body>";
        }
	}
}
using System;
using agsXMPP.Xml.Dom;

namespace agsXMPP.protocol.iq.time
{
	using Xml.Dom;

	// Send:<iq type='get' id='MX_7' to='jfrankel@coversant.net/SoapBox'>
	//			<time xmlns='urn:jabber:time'/>
	//		</iq>
	//
	// Recv:<iq from="jfrankel@coversant.net/SoapBox" id="MX_7" to="gnauck@myjabber.net/Office" type="result">
	//			<time xmlns='urn:jabber:time'>
	//				<tzo>-06:00</tzo>
	//				<utc>2006-12-19T17:58:35Z</utc>
	//			</time>
	//		</iq>

	public class EntityTime : Element
	{
		public EntityTime()
		{
			this.TagName = "time";
			this.Namespace = Uri.ENTITY_TIME;
		}

		/// <summary>
		/// The entity's numeric time zone offset from UTC. The format MUST conform to the Time Zone Definition (TZD) specified in XEP-0082.
		/// </summary>
		public string Tzo
		{
			get
			{
				return GetTag("tzo");
			}
			set
			{
				SetTag("tzo", value);
			}
		}

		/// <summary>
		/// The UTC time according to the responding entity. The format MUST conform to the dateTime profile specified in XEP-0082 and MUST be expressed in UTC.
		/// </summary>
		public string Utc
		{
			get
			{
				return GetTag("utc");
			}
			set
			{
				SetTag("utc", value);
			}
		}
	}
}
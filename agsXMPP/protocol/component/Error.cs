
using System;

namespace agsXMPP.protocol.component
{
    using client;

    /// <summary>
    /// Summary description for Error.
    /// </summary>
    public class Error : client.Error
    {
        public Error() : base()
        {
            this.Namespace = Uri.ACCEPT;
        }
                
        public Error(int code)
            : base(code)
        {
            this.Namespace = Uri.ACCEPT;
        }

        public Error(ErrorCode code)
            : base(code)
        {
            this.Namespace = Uri.ACCEPT;
        }

        public Error(ErrorType type)
            : base(type)
        {
            this.Namespace = Uri.ACCEPT;
        }

        /// <summary>
        /// Creates an error Element according the the condition
        /// The type attrib as added automatically as decribed in the XMPP specs
        /// This is the prefered way to create error Elements
        /// </summary>
        /// <param name="condition"></param>
        public Error(ErrorCondition condition)
            : base(condition)
        {
            this.Namespace = Uri.ACCEPT;
        }
    }
}

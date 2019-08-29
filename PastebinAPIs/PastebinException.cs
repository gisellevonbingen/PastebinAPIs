using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs
{
    [Serializable]
    public class PastebinException : Exception
    {
        public PastebinException() { }
        public PastebinException(string message) : base(message) { }
        public PastebinException(string message, Exception inner) : base(message, inner) { }
        protected PastebinException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

}

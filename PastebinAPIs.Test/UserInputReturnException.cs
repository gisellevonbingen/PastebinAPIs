using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PastebinAPIs.Test
{
    public class UserInputReturnException : Exception
    {
        public UserInputReturnException()
        {

        }

        public UserInputReturnException(string message) : base(message)
        {

        }

        public UserInputReturnException(string message, Exception innerException) : base(message, innerException)
        {

        }

        [SecuritySafeCritical]
        protected UserInputReturnException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }

}

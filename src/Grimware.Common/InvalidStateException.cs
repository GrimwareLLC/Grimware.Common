using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Grimware
{
    [Serializable]
    [ComVisible(true)]
    [ExcludeFromCodeCoverage]
    public class InvalidStateException
        : Exception
    {
        public InvalidStateException()
        {
        }

        public InvalidStateException(string message)
            : base(message)
        {
        }

        public InvalidStateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Grimware
{
    [Serializable]
    [ComVisible(true)]
    public class InvalidStateException
        : Exception
    {
        #region Constructors & Destructor

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

        #endregion

        #region ISerializable Members

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Grimware
{
    /// <summary>
    ///     Exception thrown to indicate that an inappropriate type argument was used for
    ///     a type parameter to a generic type or method.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [ExcludeFromCodeCoverage]
    public class TypeArgumentException
        : Exception
    {
        public TypeArgumentException()
        {
        }

        public TypeArgumentException(string message)
            : base(message)
        {
        }

        public TypeArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TypeArgumentException(SerializationInfo info, StreamingContext context)
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
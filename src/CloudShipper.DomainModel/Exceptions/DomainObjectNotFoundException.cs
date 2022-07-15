using System.Runtime.Serialization;

namespace CloudShipper.DomainModel.Exceptions
{
    [Serializable]
    public class DomainObjectNotFoundException : Exception
    {
        public DomainObjectNotFoundException()
            : base() { }

        public DomainObjectNotFoundException(string message)
            : base(message) { }

        public DomainObjectNotFoundException(string message, Exception inner)
            : base(message, inner) { }

        public DomainObjectNotFoundException(SerializationInfo info, StreamingContext context)
            :base(info, context) { }
    }
}

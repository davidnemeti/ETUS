using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainCore
{
    public interface Identity
    {
        Name Name { get; set; }
    }

    public class Name
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class NameRef
    {
        public NameRef(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public interface Reference<out TId>
        where TId : Identity
    {
        NameRef NameRef { get; }
        TId Identity { get; }
    }

    internal class ReferenceImpl<TId> : Reference<TId>
        where TId : Identity
    {
        public NameRef NameRef { get; private set; }
        public TId Identity { get; set; }

        public ReferenceImpl(NameRef nameRef)
        {
            this.NameRef = nameRef;
        }

        public ReferenceImpl(TId identity)
        {
            this.Identity = identity;
            this.NameRef = new NameRef(identity.Name.Value);
        }

        public override string ToString()
        {
            return NameRef != null
                ? string.Format("[refByName: {0}]", NameRef.ToString())
                : string.Format("[refById: {0}]", Identity.Name.ToString());
        }
    }

    public static class Reference
    {
        public static Reference<TId> GetReference<TId>(this TId identity)
            where TId : Identity
        {
            return Get(identity);
        }

        public static Reference<TId> Get<TId>(TId identity)
            where TId : Identity
        {
            return new ReferenceImpl<TId>(identity);
        }

        public static Reference<TId> Get<TId>(NameRef nameRef)
            where TId : Identity
        {
            return new ReferenceImpl<TId>(nameRef);
        }
    }
}

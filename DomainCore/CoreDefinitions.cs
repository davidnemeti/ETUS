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
    }

    public class NameRef
    {
        public NameRef(string Value)
        {
            this.Value = Value;
        }

        public string Value { get; private set; }
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
    }

    public static class Extensions
    {
        public static Reference<TId> GetReference<TId>(this TId identity)
            where TId : Identity
        {
            return new ReferenceImpl<TId>(identity);
        }

        public static Reference<TId> GetReference<TId>(NameRef nameRef)
            where TId : Identity
        {
            return new ReferenceImpl<TId>(nameRef);
        }
    }
}

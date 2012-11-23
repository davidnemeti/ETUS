using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETUS.DomainModel
{
    public class Package
    {
        public ICollection<NamespaceUsing> NamespaceUsings { get; set; }
        public NamespaceDeclaration NamespaceDeclaration { get; set; }
        public ICollection<Definition> Definitions { get; set; }
    }

    public class NamespaceUsing
    {
    }

    public class NamespaceDeclaration
    {
    }

    public abstract class Definition
    {
    }

    public class PrefixDefinition
    {
    }

    public class UnitDefinition
    {
    }

    public class QuantityDefinition
    {
    }
}

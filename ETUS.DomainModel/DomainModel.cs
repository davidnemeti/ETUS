using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETUS.DomainModel
{
    public class Package
    {
        public IEnumerable<NamespaceUsing> NamespaceUsings { get; set; }
        public NamespaceDeclaration NamespaceDeclaration { get; set; }
        public IEnumerable<Definition> Definitions { get; set; }
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

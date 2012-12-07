using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony;
using Irony.Ast;
using Irony.Parsing;
using DomainCore;
using ETUS.DomainModel.Expressions;

namespace ETUS.DomainModel
{
    public class Package
    {
        public IList<Group> Groups { get; set; }
    }

    public class Group
    {
        public IList<NamespaceUsing> NamespaceUsings { get; set; }
        public IList<Namespace> Namespaces { get; set; }
    }

    public class Namespace
    {
        public int? Foo;
        public Name Name { get; set; }
        public IList<Definition> Definitions { get; set; }
    }

    public class NamespaceUsing
    {
        public NameRef NameRef { get; set; }
    }

    public abstract class Definition : Identity
    {
        public Name Name { get; set; }

        public override string ToString()
        {
            return Name.ToString();
        }
    }

    public class PrefixDefinition : Definition
    {
        public Expression Factor { get; set; }
    }

    public class UnitDefinition : Definition
    {
        public Reference<QuantityDefinition> Quantity { get; set; }
        public IList<Conversion> Conversions { get; set; }
    }

    public class QuantityDefinition : Definition
    {
    }

    public abstract class Conversion
    {
        public Reference<UnitDefinition> OtherUnit { get; set; }
        public Direction Direction { get; set; }
    }

    public class SimpleConversion : Conversion
    {
        public Expression Factor { get; set; }
    }

    public class ComplexConversion : Conversion
    {
        public ExpressionWithUnit Expr { get; set; }
    }

    public enum Direction
    {
        BiDir,
        To,
        From
    }

    public enum Constant
    {
        PI
    }
}

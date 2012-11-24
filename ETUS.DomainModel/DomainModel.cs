using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony;
using Irony.Ast;
using Irony.Parsing;
using Expressions;

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
        public string Name { get; set; }
    }

    public class NamespaceDeclaration
    {
        public string Name { get; set; }
    }

    public abstract class Definition
    {
        public string Name { get; set; }
    }

    public class PrefixDefinition
    {
        public Expression Factor { get; set; }
    }

    public class UnitDefinition
    {
        public QuantityDefinition Quantity { get; set; }
        public Conversion Conversion { get; set; }
    }

    public class QuantityDefinition
    {
        Expression Expression = new Expression.BinaryExpression(new Expression.Parameter(), new BinaryOperator.Add(), new Expression.Number());
    }

    public abstract class Conversion
    {
    }

    public class SimpleConversion : Conversion
    {
        public Expression Value { get; set; }
        public UnitExpression Unit { get; set; }
    }
}

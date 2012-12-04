using System;
using System.Collections.Generic;
using System.Linq;

namespace ETUS.DomainModel.Expressions
{
    public abstract class ExpressionWithUnit
    {
        public class Binary : ExpressionWithUnit
        {
            public ExpressionWithUnit Term1 { get; set; }
            public BinaryOperator Op { get; set; }
            public Expression Term2 { get; set; }
        }

        public class Binary2 : ExpressionWithUnit
        {
            public Expression Term1 { get; set; }
            public BinaryOperator Op { get; set; }
            public ExpressionWithUnit Term2 { get; set; }
        }

        public class Unary : ExpressionWithUnit
        {
            public UnaryOperator Op { get; set; }
            public ExpressionWithUnit Term { get; set; }
        }

        public class Unit : ExpressionWithUnit
        {
            public UnitDefinition Value { get; set; }
        }
    }
}

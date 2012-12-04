using System;
using System.Collections.Generic;
using System.Linq;

namespace ETUS.DomainModel.Expressions
{
    public abstract class ExpressionWithUnit
    {
        public class BinaryWithUnit : ExpressionWithUnit
        {
            public ExpressionWithUnit Expr1 { get; set; }
            public Expression.Binary.Operator Op { get; set; }
            public Expression Expr2 { get; set; }
        }

        public class BinaryWithUnit2 : ExpressionWithUnit
        {
            public Expression Expr1 { get; set; }
            public Expression.Binary.Operator Op { get; set; }
            public ExpressionWithUnit Expr2 { get; set; }
        }

        public class UnaryWithUnit : ExpressionWithUnit
        {
            public Expression.Unary.Operator Op { get; set; }
            public ExpressionWithUnit Expr { get; set; }
        }

        public class UnitWithUnit : ExpressionWithUnit
        {
            public UnitDefinition Value { get; set; }
        }
    }
}

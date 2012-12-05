using System;
using System.Collections.Generic;
using System.Linq;

using DomainCore;

namespace ETUS.DomainModel.Expressions
{
    public abstract class ExpressionWithUnit
    {
        public class Binary : ExpressionWithUnit
        {
            public ExpressionWithUnit Term1 { get; set; }
            public BinaryOperator Op { get; set; }
            public Expression Term2 { get; set; }

            public override string ToString()
            {
                return string.Format("({0} {1} {2})", Term1, Op, Term2);
            }
        }

        public class Binary2 : ExpressionWithUnit
        {
            public Expression Term1 { get; set; }
            public BinaryOperator Op { get; set; }
            public ExpressionWithUnit Term2 { get; set; }

            public override string ToString()
            {
                return string.Format("({0} {1} {2})", Term1, Op, Term2);
            }
        }

        public class Unary : ExpressionWithUnit
        {
            public UnaryOperator Op { get; set; }
            public ExpressionWithUnit Term { get; set; }

            public override string ToString()
            {
                return string.Format("({0} {1})", Op, Term);
            }
        }

        public class Unit : ExpressionWithUnit
        {
            public Reference<UnitDefinition> Value { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using DomainCore;
using ETUS.DomainModel;

namespace ETUS.DomainModel.Expressions
{
    public abstract class UnitExpression
    {
        public class Binary : UnitExpression
        {
            public UnitExpression Term1 { get; set; }
            public Operator Op { get; set; }
            public UnitExpression Term2 { get; set; }

            public enum Operator
            {
                Mul = BinaryOperator.Mul,
                Div = BinaryOperator.Div
            }

            public override string ToString()
            {
                return string.Format("({0} {1} {2})", Term1, Op, Term2);
            }
        }

        public class Recip : UnitExpression
        {
            public UnitExpression Denominator { get; set; }

            public override string ToString()
            {
                return string.Format("(Recip {0})", Denominator);
            }
        }

        public class Square : UnitExpression
        {
            public UnitExpression Base { get; set; }

            public override string ToString()
            {
                return string.Format("({0}²)", Base);
            }
        }

        public class Cube : UnitExpression
        {
            public UnitExpression Base { get; set; }

            public override string ToString()
            {
                return string.Format("({0}³)", Base);
            }
        }

        public class Unit : UnitExpression
        {
            public Reference<UnitDefinition> Value { get; set; }

            public override string ToString()
            {
                return string.Format("{{UnitExpression: {0}}}", Value);
            }
        }
    }
}

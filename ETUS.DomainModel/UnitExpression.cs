using System;
using System.Collections.Generic;
using System.Linq;

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
        }

        public class Recip : UnitExpression
        {
            public UnitExpression Denominator { get; set; }
        }

        public class Pow : UnitExpression
        {
            public UnitExpression Base { get; set; }
            public int Exponent { get; set; }
        }

        public class Unit : UnitExpression
        {
            public UnitDefinition Value { get; set; }
        }
    }
}

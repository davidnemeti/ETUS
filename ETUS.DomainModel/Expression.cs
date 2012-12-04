using System;
using System.Collections.Generic;
using System.Linq;

namespace ETUS.DomainModel.Expressions
{
    public abstract class Expression
    {
        public class Binary : Expression
        {
            public Expression Term1 { get; set; }
            public BinaryOperator Op { get; set; }
            public Expression Term2 { get; set; }
        }

        public class Unary : Expression
        {
            public UnaryOperator Op { get; set; }
            public Expression Term { get; set; }
        }

        public class Number<T> : Expression
        {
            public T Value;
        }

        public class Parameter : Expression
        {
        }
    }

    public enum BinaryOperator
    {
        Add,
        Sub,
        Mul,
        Div,
        Pow
    }

    public enum UnaryOperator
    {
        Pos,
        Neg
    }
}

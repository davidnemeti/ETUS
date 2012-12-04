using System;
using System.Collections.Generic;
using System.Linq;

namespace ETUS.DomainModel.Expressions
{
    public abstract class Expression
    {
        public class Binary : Expression
        {
            public Expression Expr1 { get; set; }
            public Operator Op { get; set; }
            public Expression Expr2 { get; set; }

            public enum Operator
            {
                Add,
                Sub,
                Mul,
                Div,
                Pow
            }
        }

        public class Unary : Expression
        {
            public Operator Op { get; set; }
            public Expression Expr { get; set; }

            public enum Operator
            {
                Pos,
                Neg
            }
        }

        public class Number<T> : Expression
        {
            public T Value;
        }

        public class Parameter : Expression
        {
        }
    }
}

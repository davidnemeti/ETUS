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
            public Operator Op { get; set; }
            public Expression Term2 { get; set; }

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
            public Expression Term { get; set; }

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

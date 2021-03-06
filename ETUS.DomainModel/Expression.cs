﻿using System;
using System.Collections.Generic;
using System.Linq;

using DomainCore;

namespace ETUS.DomainModel.Expressions
{
    public abstract class Expression
    {
        public class Binary : Expression
        {
            public Expression Term1 { get; set; }
            public BinaryOperator Op { get; set; }
            public Expression Term2 { get; set; }

            public override string ToString()
            {
                return string.Format("({0} {1} {2})", Term1, Op, Term2);
            }
        }

        public class Unary : Expression
        {
            public UnaryOperator Op { get; set; }
            public Expression Term { get; set; }

            public override string ToString()
            {
                return string.Format("({0} {1})", Op, Term);
            }
        }

        public class Number : Expression
        {
            public Number() {}

            public Number(object value)
            {
                this.Value = value;
            }

            public object Value { get; set; }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class ExternalVariable : Expression
        {
            public NameRef NameRef { get; set; }
        }

        public class Constant : Expression
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace ETUS.DomainModel.Expressions
{
    public abstract class Expression
    {
    }

    public class BinaryExpression : Expression
    {
        public Expression Expr1 { get; set; }
        public BinaryOperator Op { get; set; }
        public Expression Expr2 { get; set; }
    }

    public class UnaryExpression : Expression
    {
        public UnaryOperator Op { get; set; }
        public Expression Expr { get; set; }
    }

    public class NumberExpression<T> : Expression
    {
        public T Value;
    }

    public class ParameterExpression : Expression
    {
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

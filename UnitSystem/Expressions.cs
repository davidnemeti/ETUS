using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Measures
{
    public abstract class Expression
    {
        public abstract dynamic Eval(dynamic parameter);

        public Measure<TNumber, TUnit> ToMeasure<TNumber, TUnit>(TNumber parameter)
            where TNumber : struct
            where TUnit : Unit
        {
            return new Measure<TNumber, TUnit>(Eval(parameter));
        }

        #region ConstExpression operators

        public static AddExpression operator +(Expression expression, object number)
        {
            return new AddExpression(expression, new ConstExpression(number));
        }

        public static AddExpression operator +(object number, Expression expression)
        {
            return new AddExpression(new ConstExpression(number), expression);
        }

        public static SubExpression operator -(Expression expression, object number)
        {
            return new SubExpression(expression, new ConstExpression(number));
        }

        public static SubExpression operator -(object number, Expression expression)
        {
            return new SubExpression(new ConstExpression(number), expression);
        }

        public static MulExpression operator *(Expression expression, object number)
        {
            return new MulExpression(expression, new ConstExpression(number));
        }

        public static MulExpression operator *(object number, Expression expression)
        {
            return new MulExpression(new ConstExpression(number), expression);
        }

        public static DivExpression operator /(Expression expression, object number)
        {
            return new DivExpression(expression, new ConstExpression(number));
        }

        public static DivExpression operator /(object number, Expression expression)
        {
            return new DivExpression(new ConstExpression(number), expression);
        }

        public static PowExpression operator ^(Expression expression, object number)
        {
            return new PowExpression(expression, new ConstExpression(number));
        }

        public static PowExpression operator ^(object number, Expression expression)
        {
            return new PowExpression(new ConstExpression(number), expression);
        }

        #endregion

        #region ParameterExpression operators

        public static AddExpression operator +(Expression expression, ParameterExpression param)
        {
            return new AddExpression(expression, param);
        }

        public static AddExpression operator +(ParameterExpression param, Expression expression)
        {
            return new AddExpression(param, expression);
        }

        public static SubExpression operator -(Expression expression, ParameterExpression param)
        {
            return new SubExpression(expression, param);
        }

        public static SubExpression operator -(ParameterExpression param, Expression expression)
        {
            return new SubExpression(param, expression);
        }

        public static MulExpression operator *(Expression expression, ParameterExpression param)
        {
            return new MulExpression(expression, param);
        }

        public static MulExpression operator *(ParameterExpression param, Expression expression)
        {
            return new MulExpression(param, expression);
        }

        public static DivExpression operator /(Expression expression, ParameterExpression param)
        {
            return new DivExpression(expression, param);
        }

        public static DivExpression operator /(ParameterExpression param, Expression expression)
        {
            return new DivExpression(param, expression);
        }

        public static PowExpression operator ^(Expression expression, ParameterExpression param)
        {
            return new PowExpression(expression, param);
        }

        public static PowExpression operator ^(ParameterExpression param, Expression expression)
        {
            return new PowExpression(param, expression);
        }

        #endregion
    }

    public abstract class BinaryExpression : Expression
    {
        protected readonly Expression expression1;
        protected readonly Expression expression2;

        protected BinaryExpression(Expression expression1, Expression expression2)
        {
            this.expression1 = expression1;
            this.expression2 = expression2;
        }
    }

    public class AddExpression : BinaryExpression
    {
        internal AddExpression(Expression expression1, Expression expression2)
            : base(expression1, expression2)
        {
        }

        public override dynamic Eval(dynamic parameter)
        {
            return expression1.Eval(parameter) + expression2.Eval(parameter);
        }
    }

    public class SubExpression : BinaryExpression
    {
        internal SubExpression(Expression expression1, Expression expression2)
            : base(expression1, expression2)
        {
        }

        public override dynamic Eval(dynamic parameter)
        {
            return expression1.Eval(parameter) - expression2.Eval(parameter);
        }
    }

    public class MulExpression : BinaryExpression
    {
        internal MulExpression(Expression expression1, Expression expression2)
            : base(expression1, expression2)
        {
        }

        public override dynamic Eval(dynamic parameter)
        {
            return expression1.Eval(parameter) * expression2.Eval(parameter);
        }
    }

    public class DivExpression : BinaryExpression
    {
        internal DivExpression(Expression expression1, Expression expression2)
            : base(expression1, expression2)
        {
        }

        public override dynamic Eval(dynamic parameter)
        {
            return expression1.Eval(parameter) / expression2.Eval(parameter);
        }
    }

    public class PowExpression : BinaryExpression
    {
        internal PowExpression(Expression expression1, Expression expression2)
            : base(expression1, expression2)
        {
        }

        public override dynamic Eval(dynamic parameter)
        {
            return expression1.Eval(parameter) ^ expression2.Eval(parameter);
        }
    }

    public class ParameterExpression : Expression
    {
        internal ParameterExpression()
        {
        }

        public static ParameterExpression FromMeasure<TNumber, TUnit>(Measure<TNumber, TUnit> measure)
            where TNumber : struct
            where TUnit : Unit
        {
            return new ParameterExpression();
        }

        public override dynamic Eval(dynamic parameter)
        {
            return parameter;
        }
    }

    public class ConstExpression : Expression
    {
        private readonly dynamic number;

        internal ConstExpression(object number)
        {
            this.number = number;
        }

        public override dynamic Eval(dynamic parameter)
        {
            return number;
        }

        public static ConstExpression FromMeasure(object number)
        {
            return new ConstExpression(number);
        }
   }
}

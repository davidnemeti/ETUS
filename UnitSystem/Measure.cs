using System;

namespace Utilities.Measures
{
    public struct Ratio<TNumber>
    {
        private readonly TNumber number;

        public Ratio(TNumber number)
        {
            this.number = number;
        }

        public static implicit operator TNumber(Ratio<TNumber> ratio)
        {
            return ratio.number;
        }

        public static implicit operator Ratio<TNumber>(TNumber number)
        {
            return new Ratio<TNumber>(number);
        }
    }

    public static class Ratio
    {
        public static Ratio<TNumber> New<TNumber>(TNumber number)
        {
            return new Ratio<TNumber>(number);
        }
    }

    public struct Measure<TNumber, TUnit>
        where TNumber : struct
        where TUnit : Unit
    {
        private TNumber number;

        public TNumber Number { get { return this.number; } set { this.number = value; } }

        public Measure(TNumber number)
        {
            this.number = number;
        }

        public static explicit operator Measure<TNumber, TUnit>(TNumber number)
        {
            return new Measure<TNumber,TUnit>(number);
        }

        public static implicit operator ImplicitlyConvertible<TNumber>(Measure<TNumber, TUnit> measure)
        {
            return new ImplicitlyConvertible<TNumber>(measure.Number);
        }

        public static implicit operator Measure<TNumber, TUnit>(ImplicitlyConvertible<TNumber> implicitlyConvertible)
        {
            return new Measure<TNumber,TUnit>(implicitlyConvertible.Number);
        }

        public static Measure<TNumber, TUnit> operator +(Measure<TNumber, TUnit> measure1, Measure<TNumber, TUnit> measure2)
        {
            return Measure.Add(measure1, measure2);
        }

        public static Measure<TNumber, TUnit> operator -(Measure<TNumber, TUnit> measure1, Measure<TNumber, TUnit> measure2)
        {
            return Measure.Sub(measure1, measure2);
        }

        public static Measure<TNumber, TUnit> operator *(Measure<TNumber, TUnit> measure, TNumber number)
        {
            return Measure.Mul(measure, number);
        }

        public static Measure<TNumber, TUnit> operator *(TNumber number, Measure<TNumber, TUnit> measure)
        {
            return Measure.Mul(number, measure);
        }

        public static Measure<TNumber, TUnit> operator /(Measure<TNumber, TUnit> measure, TNumber number)
        {
            return Measure.Div(measure, number);
        }

        public static Measure<TNumber, UnitReciprocal<TUnit>> operator /(TNumber number, Measure<TNumber, TUnit> measure)
        {
            return Measure.Div(number, measure);
        }

        // Div: TUnit1 / TUnit1 -> Ratio
        public Ratio<TNumber> Div(Measure<TNumber, TUnit> that)
        {
            return ((dynamic)this.Number) / ((dynamic)that.Number);
        }

        public Measure<TNumber, TUnit> Add(Measure<TNumber, TUnit> that)
        {
            return Measure.Add(this, that);
        }

        public Measure<TNumber, TUnit> Sub(Measure<TNumber, TUnit> that)
        {
            return Measure.Sub(this, that);
        }

        public Measure<TNumber, TUnit> Mul(TNumber number)
        {
            return Measure.Mul(this, number);
        }

        public Measure<TNumber, TUnit> Div(TNumber number)
        {
            return Measure.Div(this, number);
        }

        // Mul: TUnit * Recip(TUnit) -> Ratio
        public Ratio<TNumber> Mul(Measure<TNumber, UnitReciprocal<TUnit>> that)
        {
            return ((dynamic)this.Number) * ((dynamic)that.Number);
        }

        //public Measure<TNumber, TUnitTo> ConvertToExplicit<TUnitTo>(IConvertibleFromExplicit<TUnit, TUnitTo> unitTo)
        //    where TUnitTo : Unit, IConvertibleFromExplicit<TUnit, TUnitTo>
        //{
        //    return new Measure<TNumber, TUnitTo>(new TUnitTo().ConvertFrom(this.Number));
        //}

        //public Measure<TNumber, TUnitTo> ConvertToImplicit<TUnitTo>(IConvertibleFromImplicit<TUnit, TUnitTo> unitTo)
        //    where TUnitTo : Unit, IConvertibleFromImplicit<TUnit, TUnitTo>
        //{
        //    return new Measure<TNumber, TUnitTo>(this.Number);
        //}
    }

    public static class Measure
    {
        public static Measure<TNumber, TUnitTo> Convert<TNumber, TUnitFrom, TUnitTo>(Measure<TNumber, TUnitFrom> measureFrom, Func<ParameterExpression, Expression> convertTo)
            where TNumber : struct
            where TUnitFrom : Unit
            where TUnitTo : Unit
        {
            return new Measure<TNumber, TUnitTo>(convertTo(ParameterExpression.FromMeasure(measureFrom)).Eval(measureFrom.Number));
        }

        public static ImplicitlyConvertible<TNumber> Implicit<TNumber, TUnit>(Measure<TNumber, TUnit> measure)
            where TNumber : struct
            where TUnit : Unit
        {
            return measure;
        }

        public static Measure<TNumber, TUnit> Normalize<TNumber, TUnit>(this Measure<TNumber, TUnit> measure)
            where TNumber : struct
            where TUnit : Unit
        {
            return measure;
        }

        public static Measure<TNumber, UnitQuotient<UnitProduct<TUnit1, TUnit2>, UnitProduct<TUnit3, TUnit3>>>
            Normalize<TNumber, TUnit1, TUnit2, TUnit3>
            (this Measure<TNumber, UnitProduct<TUnit1, UnitQuotient<TUnit2, UnitProduct<TUnit3, TUnit3>>>> measure)
            where TNumber : struct
            where TUnit1 : Unit
            where TUnit2 : Unit
            where TUnit3 : Unit
        {
            return new Measure<TNumber, UnitQuotient<UnitProduct<TUnit1, TUnit2>, UnitProduct<TUnit3, TUnit3>>>();
        }

        public static Measure<TNumber, TUnitTo> ConvertFromExplicit<TNumber, TUnitFrom, TUnitTo>(this TUnitTo unitTo, Measure<TNumber, TUnitFrom> measureFrom)
            where TNumber : struct
            where TUnitFrom : Unit, IConvertibleToExplicit<TUnitFrom, TUnitTo>, new()
            where TUnitTo : Unit
        {
            return new TUnitFrom().ConvertToExplicit(measureFrom, unitTo);
        }

        public static Measure<TNumber, TUnitTo> ConvertFromImplicit<TNumber, TUnitFrom, TUnitTo>(this TUnitTo unitTo, Measure<TNumber, TUnitFrom> measureFrom)
            where TNumber : struct
            where TUnitFrom : Unit, IConvertibleToImplicit<TUnitFrom, TUnitTo>
            where TUnitTo : Unit
        {
            //            return new TUnitFrom().ConvertToImplicit(measureFrom, unitTo);
            return new Measure<TNumber, TUnitTo>(measureFrom.Number);
        }

        //public static Measure<TNumber, TUnitTo> ConvertToXXX<TNumber, TUnitFrom, TUnitTo>(this Measure<TNumber, TUnitFrom> measureFrom, TUnitTo unitTo)
        //    where TUnitFrom : Unit
        //    where TUnitTo : Unit
        //{
        //    return new Measure<TNumber, TUnitTo>(measureFrom.Unit.ConvertTo(measureFrom.Number, unitTo));
        //}

        //public static Measure<TNumber, TUnitTo> ConvertTo<TNumber, TUnitFrom, TUnitTo>(this Measure<TNumber, TUnitFrom> measureFrom, IConvertibleFromExplicit<TUnitFrom, TUnitTo> unitTo)
        //    where TUnitFrom : Unit
        //    where TUnitTo : Unit
        //{
        //    return new Measure<TNumber, TUnitTo>(unitTo.ConvertFrom(measureFrom.Number));
        //}

        //public static Measure<TNumber, TUnitTo> ConvertToExplicit<TNumber, TUnitFrom, TUnitTo>(this TUnitFrom unitFrom, IConvertibleFromExplicit<TUnitFrom, TUnitTo> unitTo)
        //    where TUnitFrom : Unit
        //    where TUnitTo : Unit
        //{
        //    return new Measure<TNumber, TUnitTo>();
        //}

        //public static Measure<TNumber, TUnitTo> ConvertToExplicit<TNumber, TUnitFrom, TUnitTo>(this IConvertibleToExplicit<TUnitFrom, TUnitTo> unitFrom, TUnitTo unitTo)
        //    where TUnitFrom : Unit
        //    where TUnitTo : Unit
        //{
        //    return new Measure<TNumber, TUnitTo>();
        //}

        //public static Measure<TNumber, TUnitTo> ConvertToExplicit<TNumber, TUnitFrom, TUnitTo>(this Measure<TNumber, TUnitFrom> measure, IConvertibleFromExplicit<TUnitFrom, TUnitTo> unitTo)
        //    where TUnitFrom : Unit
        //    where TUnitTo : Unit, IConvertibleFromExplicit<TUnitFrom, TUnitTo>
        //{
        //    return new Measure<TNumber, TUnitTo>(new TUnitTo().ConvertFrom(measure.Number));
        //}

        //public static Measure<TNumber, TUnitTo> ConvertToImplicit<TNumber, TUnitFrom, TUnitTo>(this Measure<TNumber, TUnitFrom> measure, IConvertibleFromImplicit<TUnitFrom, TUnitTo> unitTo)
        //    where TUnitFrom : Unit
        //    where TUnitTo : Unit, IConvertibleFromImplicit<TUnitFrom, TUnitTo>
        //{
        //    return new Measure<TNumber, TUnitTo>(measure.Number);
        //}

        //public static Measure<TNumber, TUnitTo> ConvertToExplicit<TNumber, TUnitFrom, TUnitTo>(this IMeasure<TNumber, IConvertibleToExplicit<TUnitFrom, TUnitTo>> measure, TUnitTo unitTo)
        //    where TUnitFrom : Unit
        //    where TUnitTo : Unit
        //{
        //    return new Measure<TNumber, TUnitTo>(measure.Unit.ConvertTo(measure.Number));
        //}

        //public static Measure<TNumber, TUnitTo> ConvertToImplicit<TNumber, TUnitFrom, TUnitTo>(this IMeasure<TNumber, IConvertibleToImplicit<TUnitFrom, TUnitTo>> measure, TUnitTo unitTo)
        //    where TUnitFrom : Unit
        //    where TUnitTo : Unit
        //{
        //    return new Measure<TNumber, TUnitTo>(measure.Number);
        //}

        public static Measure<TNumber, TUnitTo> ConvertToMeasure<TNumber, TValue, TUnitTo>(TValue value)
            where TNumber : struct
            where TUnitTo : Unit, IConvertibleFromValue<TNumber, TValue>, new()
        {
            return new Measure<TNumber, TUnitTo>(new TUnitTo().ConvertFrom(value));
        }

        public static TValue ConvertToValue<TNumber, TUnitFrom, TValue>(this Measure<TNumber, TUnitFrom> measure)
            where TNumber : struct
            where TUnitFrom : Unit, IConvertibleToValue<TNumber, TValue>, new()
        {
            return new TUnitFrom().ConvertTo(measure.Number);
        }

        // Add: TUnit + TUnit -> TUnit
        public static Measure<TNumber, TUnit> Add<TNumber, TUnit>(this Measure<TNumber, TUnit> measure1, Measure<TNumber, TUnit> measure2)
            where TNumber : struct
            where TUnit : Unit
        {
            return new Measure<TNumber, TUnit>(((dynamic)measure1.Number) + ((dynamic)measure2.Number));
        }

        // Sub: TUnit - TUnit -> TUnit
        public static Measure<TNumber, TUnit> Sub<TNumber, TUnit>(this Measure<TNumber, TUnit> measure1, Measure<TNumber, TUnit> measure2)
            where TNumber : struct
            where TUnit : Unit
        {
            return new Measure<TNumber, TUnit>(((dynamic)measure1.Number) - ((dynamic)measure2.Number));
        }

        #region Mul

        // Mul: TUnit * Ratio -> TUnit
        public static Measure<TNumber, TUnit> Mul<TNumber, TUnit>(this Measure<TNumber, TUnit> measure, Ratio<TNumber> ratio)
            where TNumber : struct
            where TUnit : Unit
        {
            return new Measure<TNumber, TUnit>(((dynamic)measure.Number) * ((dynamic)ratio));
        }

        // Mul: Ratio * TUnit -> TUnit
        public static Measure<TNumber, TUnit> Mul<TNumber, TUnit>(this Ratio<TNumber> ratio, Measure<TNumber, TUnit> measure)
            where TNumber : struct
            where TUnit : Unit
        {
            return new Measure<TNumber, TUnit>(((dynamic)ratio) * ((dynamic)measure.Number));
        }

        // Mul: TUnit1 * TUnit2 -> Prod(TUnit1, TUnit2)
        public static Measure<TNumber, UnitProduct<TUnit1, TUnit2>> Mul<TNumber, TUnit1, TUnit2>(this Measure<TNumber, TUnit1> measure1, Measure<TNumber, TUnit2> measure2)
            where TNumber : struct
            where TUnit1 : Unit
            where TUnit2 : Unit
        {
            return new Measure<TNumber, UnitProduct<TUnit1, TUnit2>>(((dynamic)measure1.Number) * ((dynamic)measure2.Number));
        }

        // Mul: Quot(TUnit1, TUnit2) * TUnit2 -> TUnit1
        public static Measure<TNumber, TUnit1> Mul<TNumber, TUnit1, TUnit2>(this Measure<TNumber, UnitQuotient<TUnit1, TUnit2>> measure1, Measure<TNumber, TUnit2> measure2)
            where TNumber : struct
            where TUnit1 : Unit
            where TUnit2 : Unit
        {
            return new Measure<TNumber, TUnit1>(((dynamic)measure1.Number) * ((dynamic)measure2.Number));
        }

        // Mul: Quot(TUnit1, TUnit2) / TUnit1 -> Recip(TUnit2)
        public static Measure<TNumber, UnitReciprocal<TUnit2>> Div<TNumber, TUnit1, TUnit2>(this Measure<TNumber, UnitQuotient<TUnit1, TUnit2>> measure1, Measure<TNumber, TUnit1> measure2)
            where TNumber : struct
            where TUnit1 : Unit
            where TUnit2 : Unit
        {
            return new Measure<TNumber, UnitReciprocal<TUnit2>>(((dynamic)measure1.Number) * ((dynamic)measure2.Number));
        }

#if true
        // Mul: Recip(TUnit) * TUnit -> Ratio
        public static Ratio<TNumber> Mul<TNumber, TUnit>(this Measure<TNumber, UnitReciprocal<TUnit>> measure1, Measure<TNumber, TUnit> measure2)
            where TNumber : struct
            where TUnit : Unit
        {
            return ((dynamic)measure1.Number) * ((dynamic)measure2.Number);
        }
#endif

#if false
        // Mul: TUnit * Recip(TUnit) -> Ratio
        public static Ratio<TNumber> Mul<TNumber, TUnit>(this Measure<TNumber, TUnit> measure1, Measure<TNumber, UnitReciprocal<TUnit>> measure2)
            where TNumber : struct
            where TUnit : Unit
        {
            return ((dynamic)measure1.Number) * ((dynamic)measure2.Number);
        }
#endif

        // Mul: TUnit1 * Quot(TUnit2, TUnit1) -> TUnit2
        public static Measure<TNumber, TUnit2> Mul<TNumber, TUnit1, TUnit2>(this Measure<TNumber, TUnit1> measure1, Measure<TNumber, UnitQuotient<TUnit2, TUnit1>> measure2)
            where TNumber : struct
            where TUnit1 : Unit
            where TUnit2 : Unit
        {
            return new Measure<TNumber, TUnit2>(((dynamic)measure1.Number) * ((dynamic)measure2.Number));
        }

        #endregion

        #region Div

        // Div: TUnit / Ratio -> TUnit
        public static Measure<TNumber, TUnit> Div<TNumber, TUnit>(this Measure<TNumber, TUnit> measure, Ratio<TNumber> ratio)
            where TNumber : struct
            where TUnit : Unit
        {
            return new Measure<TNumber, TUnit>(((dynamic)measure.Number) / ((dynamic)ratio));
        }

        // Div: Ratio / TUnit -> Recip(TUnit)
        public static Measure<TNumber, UnitReciprocal<TUnit>> Div<TNumber, TUnit>(this Ratio<TNumber> ratio, Measure<TNumber, TUnit> measure)
            where TNumber : struct
            where TUnit : Unit
        {
            return new Measure<TNumber, UnitReciprocal<TUnit>>(((dynamic)ratio) / ((dynamic)measure.Number));
        }

#if false
        // this method resides in Measure<TNumber, TUnit>, because here it would cause ambiguity
        // Div: TUnit1 / TUnit1 -> Ratio
        public static Ratio<TNumber> Div<TNumber, TUnit>(this Measure<TNumber, TUnit> measure1, Measure<TNumber, TUnit> measure2)
            where TUnit : Unit
        {
            return ((dynamic)measure1.Number) / ((dynamic)measure2.Number);
        }
#endif

        // Div: TUnit1 / TUnit2 -> Quot(TUnit1, TUnit2)
        public static Measure<TNumber, UnitQuotient<TUnit1, TUnit2>> Div<TNumber, TUnit1, TUnit2>(this Measure<TNumber, TUnit1> measure1, Measure<TNumber, TUnit2> measure2)
            where TNumber : struct
            where TUnit1 : Unit
            where TUnit2 : Unit
        {
            return new Measure<TNumber, UnitQuotient<TUnit1, TUnit2>>(((dynamic)measure1.Number) / ((dynamic)measure2.Number));
        }

        // Div: Prod(TUnit1, TUnit2) / TUnit1 -> TUnit2
        public static Measure<TNumber, TUnit2> Div<TNumber, TUnit1, TUnit2>(this Measure<TNumber, UnitProduct<TUnit1, TUnit2>> measure1, Measure<TNumber, TUnit1> measure2)
            where TNumber : struct
            where TUnit1 : Unit
            where TUnit2 : Unit
        {
            return new Measure<TNumber, TUnit2>(((dynamic)measure1.Number) / ((dynamic)measure2.Number));
        }

        // Div: Prod(TUnit1, TUnit2) / TUnit2 -> TUnit1
        public static Measure<TNumber, TUnit1> Div<TNumber, TUnit1, TUnit2>(this Measure<TNumber, UnitProduct<TUnit1, TUnit2>> measure1, Measure<TNumber, TUnit2> measure2)
            where TNumber : struct
            where TUnit1 : Unit
            where TUnit2 : Unit
        {
            return new Measure<TNumber, TUnit1>(((dynamic)measure1.Number) / ((dynamic)measure2.Number));
        }

        // Div: TUnit1 / Prod(TUnit1, TUnit2) -> Recip(TUnit2)
        public static Measure<TNumber, UnitReciprocal<TUnit2>> Div<TNumber, TUnit1, TUnit2>(this Measure<TNumber, TUnit1> measure1, Measure<TNumber, UnitProduct<TUnit1, TUnit2>> measure2)
            where TNumber : struct
            where TUnit1 : Unit
            where TUnit2 : Unit
        {
            return new Measure<TNumber, UnitReciprocal<TUnit2>>(((dynamic)measure1.Number) / ((dynamic)measure2.Number));
        }

        // Div: TUnit2 / Prod(TUnit1, TUnit2) -> Recip(TUnit1)
        public static Measure<TNumber, UnitReciprocal<TUnit1>> Div<TNumber, TUnit1, TUnit2>(this Measure<TNumber, TUnit2> measure1, Measure<TNumber, UnitProduct<TUnit1, TUnit2>> measure2)
            where TNumber : struct
            where TUnit1 : Unit
            where TUnit2 : Unit
        {
            return new Measure<TNumber, UnitReciprocal<TUnit1>>(((dynamic)measure1.Number) / ((dynamic)measure2.Number));
        }

        // Div: Ratio / Recip(TUnit) -> TUnit
        public static Measure<TNumber, TUnit> Div<TNumber, TUnit>(this Ratio<TNumber> ratio, Measure<TNumber, UnitReciprocal<TUnit>> measure)
            where TNumber : struct
            where TUnit : Unit
        {
            return new Measure<TNumber, TUnit>(((dynamic)ratio) / ((dynamic)measure.Number));
        }

        #endregion
    }
}

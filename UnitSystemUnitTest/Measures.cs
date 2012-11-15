using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ZEUS.Core;
//using ZEUS.Predefined;
//using ZEUS.Core.Prefixes;

namespace ZEUS.UnitTest
{
    public class Length : Quantity { }

    public class A : BaseUnit<A, Length>,
        IConvertibleFromImplicit<B, A>,
        IConvertibleToImplicit<A, B>,
        IConvertibleToImplicit<A, C>,
        IConvertibleFromExplicit<B, A>,
        IConvertibleToExplicit<A, B>,
        IConvertibleToExplicit<A, C>
    {
        //Func<ParameterExpression<TNumber, A>, Measure<TNumber, B>> Convert<TNumber, A, B>()
        //{
        //}

        public Measure<TNumber, A> ConvertFromImplicit<TNumber>(Measure<TNumber, B> measureFrom)
            where TNumber : struct
        {
            return new Measure<TNumber,A>(measureFrom.Number);
        }

        public Measure<TNumber, B> ConvertToImplicit<TNumber>(Measure<TNumber, A> measureFrom, B unitTo)
            where TNumber : struct
        {
            return Measure.Implicit(measureFrom);
        }

        public Measure<TNumber, C> ConvertToImplicit<TNumber>(Measure<TNumber, A> measureFrom, C unitTo)
            where TNumber : struct
        {
            throw new NotImplementedException();
        }

        public Measure<TNumber, A> ConvertFromExplicit<TNumber>(Measure<TNumber, B> measureFrom)
            where TNumber : struct
        {
            throw new NotImplementedException();
        }

        public Measure<TNumber, B> ConvertToExplicit<TNumber>(Measure<TNumber, A> measureFrom, B unitTo)
            where TNumber : struct
        {
            return Measure.Convert<TNumber, A, B>(measureFrom, param => param * 3 + 7);
        }

        public Measure<TNumber, C> ConvertToExplicit<TNumber>(Measure<TNumber, A> measureFrom, C unitTo)
            where TNumber : struct
        {
            throw new NotImplementedException();
        }
    }

    public class B : BaseUnit<B, Length>
    {
    }

    public class C : BaseUnit<C, Length>
    {
    }

    [TestClass]
    public class Measures
    {
        [TestMethod]
        public void TestMeasures()
        {
            var a = new Measure<double, A>(5);
            var b = new Measure<double, B>(5);
            var c = new Measure<double, C>(5);

            a = new A().ConvertFromImplicit(b);
            a = new A().ConvertFromExplicit(b);
            b = new B().ConvertFromImplicit(a);
            b = new B().ConvertFromExplicit(a);

//            var length = new Measure<double, Meter>(5);
//            Measure<double, Meter> length2 = new Measure<double, Meter>(8);
//            var length3 = new Measure<double, Centi<Meter>>(3);
//            var length4 = new Measure<double, Milli<Meter>>(2);
//            var weight = new Measure<double, Kilogram>(85);
//            var time = new Measure<double, Second>(4.2);
//            var length5 = new Measure<double, Foot>(7);
//            Measure<double, Inch> length6 = length5.ConvertToExplicit(new Inch());
//            Measure<double, Meter> length7 = length5.ConvertToExplicit(new Meter());

//            TimeSpan timespan = time.ConvertToValue<double, Second, TimeSpan>();
//            time = Measure.ConvertToMeasure<double, TimeSpan, Second>(timespan);

//            var forcex = weight.Mul(length).Div(time.Mul(time)).ConvertToImplicit(new Newton());
//            var force3 = weight.Mul(length.Div(time.Mul(time))).Normalize().ConvertToImplicit<Newton>();
//            var force4 = weight.Mul(length.Div(time.Mul(time))).ConvertToImplicit(new Newton());

//            length = length2;
//            //            length3 = length2;

//            length3 = length.ConvertToExplicit<Centi<Meter>>();
//            length = length3.ConvertToExplicit<Meter>();
//            length4 = length.ConvertToExplicit<Milli<Meter>>();
//            length = length4.ConvertToExplicit<Meter>();
//            //length4 = length3.ConvertToExplicit<Meter, Milli<Meter>>();
//            //length3 = length4.ConvertToExplicit<Meter, Centi<Meter>>();

//            length = length + length2;
//            length = length - length2;
//            length += length2;
//            //            length = length + length3;

//            var speed = length.Div(time);
//            speed = speed * 4;
//            speed = 5 * speed;
//            //            speed = 5.2.Mul(speed);
//            speed = Measure.Mul(5.2, speed);
//            speed *= 3;
//            var foo = 5 / speed;
//            speed = speed / 5;
//            length2 = speed.Mul(time);
//            length2 = length2.Div(time).Mul(time);
//            length2 = length2.Div(time).Div(time).Mul(time).Mul(time);
//            length2 = length2.Div(time).Mul(time).Div(time).Mul(time);
//            length2 = length2.Div(time).Div(length2).Mul(time).Mul(length2);
//            length2 = length2.Div(length2).Mul(length2);
//            length2 = length2.Div(length2).Div(time).Mul(time).Mul(length2);
////            length2 = length2.Div(length2).Div(time).Mul(length2).Mul(time);
//            var boo = length2.Div(length2).Div(time);
//            double ratio = length2.Div(length2);
        }
    }
}

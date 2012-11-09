using System;
using Utilities.Measures.Prefixes;

namespace Utilities.Measures.Predefined
{
    public class Mass : Quantity { }

    public class Kilogram : BaseUnit<Kilogram, Mass>,
        IConvertibleFromImplicit<Kilo<Gram>, Kilogram>,
        IConvertibleToImplicit<Kilogram, Kilo<Gram>>,
        IConvertibleFromExplicit<Gram, Kilogram>,
        IConvertibleFromExplicit<Pound, Kilogram>
//        IConvertibleFromExplicit<Ounce, Kilogram>
    {
        TNumber IConvertibleFromExplicit<Gram, Kilogram>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1000;
        }

        TNumber IConvertibleFromExplicit<Pound, Kilogram>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 0.45359237;
        }
    }

    public class Gram : BaseUnit<Gram, Mass>,
        IConvertibleFromExplicit<Kilogram, Gram>,
        IConvertibleFromExplicit<Pound, Gram>
//        IConvertibleFromExplicit<Ounce, Gram>
    {
        TNumber IConvertibleFromExplicit<Kilogram, Gram>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1000;
        }

        TNumber IConvertibleFromExplicit<Pound, Gram>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 0.00045359237;
        }
    }

    public class Pound : BaseUnit<Pound, Mass>,
        IConvertibleFromExplicit<Kilogram, Pound>,
        IConvertibleFromExplicit<Gram, Pound>
//        IConvertibleFromExplicit<Ounce, Pound>
    {
        TNumber IConvertibleFromExplicit<Kilogram, Pound>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 0.45359237;
        }

        TNumber IConvertibleFromExplicit<Gram, Pound>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 0.00045359237;
        }

        //TNumber IConvertibleFromExplicit<Ounce, Pound>.ConvertFrom<TNumber>(TNumber number)
        //{
        //    return ((dynamic)number) / 16;
        //}
    }

    //public class Ounce : BaseUnit<Ounce, Mass>,
    //    IConvertibleFromExplicit<Kilogram, Ounce>,
    //    IConvertibleFromExplicit<Gram, Ounce>,
    //    IConvertibleFromExplicit<Pound, Ounce>,
    //{
    //    TNumber IConvertibleFromExplicit<Kilogram, Ounce>.ConvertFrom<TNumber>(TNumber number)
    //    {
    //        return ((dynamic)number) / ;
    //    }

    //    TNumber IConvertibleFromExplicit<Gram, Ounce>.ConvertFrom<TNumber>(TNumber number)
    //    {
    //        return ((dynamic)number) / ;
    //    }

    //    TNumber IConvertibleFromExplicit<Pound, Ounce>.ConvertFrom<TNumber>(TNumber number)
    //    {
    //        return ((dynamic)number) * 16;
    //    }
    //}
}

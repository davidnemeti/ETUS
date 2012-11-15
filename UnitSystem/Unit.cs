using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Utilities.Measures.Prefixes;

#pragma warning disable 0618

namespace ZEUS.Core
{
    public abstract class Quantity
    {
    }

    public abstract class Unit
    {
        //public TNumber ConvertTo<TNumber, TUnitFrom, TUnitTo>(TNumber number, IConvertibleFromExplicit<TUnitFrom, TUnitTo> unitTo)
        //{
        //    throw new NotImplementedException();
        //}

        //public Measure<TNumber, TUnitTo> ConvertTo<TNumber, TUnitFrom, TUnitTo>(IConvertibleFromExplicit<TUnitFrom, TUnitTo> unitTo)
        //    where TUnitFrom : Unit, new()
        //    where TUnitTo : Unit, new()
        //{
        //    return new Measure<TNumber, TUnitTo>();
        //}
    }

    public abstract class BaseUnit<TBaseUnit, TQuantity> : Unit
#if false
        IConvertibleFromExplicit<Yotta<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Zetta<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Exa<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Peta<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Tera<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Giga<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Mega<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Kilo<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Hecto<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Deca<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Deci<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Centi<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Milli<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Micro<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Nano<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Pico<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Femto<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Atto<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Zepto<TBaseUnit>, TBaseUnit>,
        IConvertibleFromExplicit<Yocto<TBaseUnit>, TBaseUnit>
#endif
        where TBaseUnit : BaseUnit<TBaseUnit, TQuantity>, new()
        where TQuantity : Quantity, new()
    {
        #region Conversions
#if false
        TNumber IConvertibleFromExplicit<Yotta<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E+24;
        }

        TNumber IConvertibleFromExplicit<Zetta<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E+21;
        }

        TNumber IConvertibleFromExplicit<Exa<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E+18;
        }

        TNumber IConvertibleFromExplicit<Peta<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E+15;
        }

        TNumber IConvertibleFromExplicit<Tera<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E+12;
        }

        TNumber IConvertibleFromExplicit<Giga<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E+9;
        }

        TNumber IConvertibleFromExplicit<Mega<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E+6;
        }

        TNumber IConvertibleFromExplicit<Kilo<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E+3;
        }

        TNumber IConvertibleFromExplicit<Hecto<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E+2;
        }

        TNumber IConvertibleFromExplicit<Deca<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E+1;
        }

        TNumber IConvertibleFromExplicit<Deci<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E-1;
        }

        TNumber IConvertibleFromExplicit<Centi<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E-2;
        }

        TNumber IConvertibleFromExplicit<Milli<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E-3;
        }

        TNumber IConvertibleFromExplicit<Micro<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E-6;
        }

        TNumber IConvertibleFromExplicit<Nano<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E-9;
        }

        TNumber IConvertibleFromExplicit<Pico<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E-12;
        }

        TNumber IConvertibleFromExplicit<Femto<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E-15;
        }

        TNumber IConvertibleFromExplicit<Atto<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E-18;
        }

        TNumber IConvertibleFromExplicit<Zepto<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E-21;
        }

        TNumber IConvertibleFromExplicit<Yocto<TBaseUnit>, TBaseUnit>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1E-24;
        }
#endif
    	#endregion
    }

    public abstract class PrefixUnit<TUnitBase> : Unit
        where TUnitBase : Unit
    {
    }

    public sealed class UnitProduct<TUnit1, TUnit2> : Unit
        where TUnit1 : Unit
        where TUnit2 : Unit
    {
    }

    public sealed class UnitQuotient<TUnit1, TUnit2> : Unit
        where TUnit1 : Unit
        where TUnit2 : Unit
    {
    }

    public sealed class UnitReciprocal<TUnit> : Unit
        where TUnit : Unit
    {
    }
}

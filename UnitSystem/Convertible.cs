using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZEUS.Core
{
    public interface IConvertibleToImplicit<TUnitFrom, TUnitTo>
        where TUnitFrom : Unit
        where TUnitTo : Unit
    {
        Measure<TNumber, TUnitTo> ConvertToImplicit<TNumber>(Measure<TNumber, TUnitFrom> measureFrom, TUnitTo unitTo) where TNumber : struct;
    }

    public interface IConvertibleToExplicit<TUnitFrom, TUnitTo>
        where TUnitFrom : Unit
        where TUnitTo : Unit
    {
        Measure<TNumber, TUnitTo> ConvertToExplicit<TNumber>(Measure<TNumber, TUnitFrom> measureFrom, TUnitTo unitTo) where TNumber : struct;
    }

    public interface IConvertibleFromImplicit<TUnitFrom, TUnitTo>
        where TUnitFrom : Unit
        where TUnitTo : Unit
    {
        Measure<TNumber, TUnitTo> ConvertFromImplicit<TNumber>(Measure<TNumber, TUnitFrom> measureFrom) where TNumber : struct;
    }

    public interface IConvertibleFromExplicit<TUnitFrom, TUnitTo>
        where TUnitFrom : Unit
        where TUnitTo : Unit
    {
        Measure<TNumber, TUnitTo> ConvertFromExplicit<TNumber>(Measure<TNumber, TUnitFrom> measureFrom) where TNumber : struct;
    }

    public interface IConvertibleToValue<TNumber, TValueTo>
    {
        TValueTo ConvertTo(TNumber numberFrom);
    }

    public interface IConvertibleFromValue<TNumber, TValueFrom>
    {
        TNumber ConvertFrom(TValueFrom valueFrom);
    }

    public class ImplicitlyConvertible<TNumber>
        where TNumber : struct
    {
        private readonly TNumber number;

        public TNumber Number { get { return this.number; } }

        public ImplicitlyConvertible(TNumber number)
        {
            this.number = number;
        }
    }
}

using System;
#if false
using Utilities.Measures.Prefixes;

namespace Utilities.Measures.Predefined
{
    public class Length : Quantity { }

    public class Meter : BaseUnit<Meter, Length>,
        IConvertibleFromExplicit<Mile, Meter>,
        IConvertibleFromExplicit<Foot, Meter>,
        IConvertibleFromExplicit<Yard, Meter>,
        IConvertibleFromExplicit<Inch, Meter>
    {
        TNumber IConvertibleFromExplicit<Mile, Meter>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1609.344;
        }

        TNumber IConvertibleFromExplicit<Foot, Meter>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 0.3048;
        }

        TNumber IConvertibleFromExplicit<Inch, Meter>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 0.0254;
        }

        TNumber IConvertibleFromExplicit<Yard, Meter>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 0.9144;
        }
    }

    public class Foot : BaseUnit<Foot, Length>,
        IConvertibleFromExplicit<Meter, Foot>,
        IConvertibleFromExplicit<Mile, Foot>,
        IConvertibleFromExplicit<Yard, Foot>,
        IConvertibleFromExplicit<Inch, Foot>
    {
        TNumber IConvertibleFromExplicit<Meter, Foot>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 0.3048;
        }

        TNumber IConvertibleFromExplicit<Mile, Foot>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 5280;
        }

        TNumber IConvertibleFromExplicit<Yard, Foot>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 3;
        }

        TNumber IConvertibleFromExplicit<Inch, Foot>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 12;
        }
    }

    public class Yard : BaseUnit<Yard, Length>,
        IConvertibleFromExplicit<Meter, Yard>,
        IConvertibleFromExplicit<Mile, Yard>,
        IConvertibleFromExplicit<Foot, Yard>,
        IConvertibleFromExplicit<Inch, Yard>
    {
        TNumber IConvertibleFromExplicit<Meter, Yard>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 0.9144;
        }

        TNumber IConvertibleFromExplicit<Mile, Yard>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 1760;
        }

        TNumber IConvertibleFromExplicit<Foot, Yard>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 3;
        }

        TNumber IConvertibleFromExplicit<Inch, Yard>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 36;
        }
    }

    public class Inch : BaseUnit<Inch, Length>,
        IConvertibleFromExplicit<Mile, Inch>,
        IConvertibleFromExplicit<Foot, Inch>,
        IConvertibleFromExplicit<Yard, Inch>,
        IConvertibleFromExplicit<Meter, Inch>
    {
        TNumber IConvertibleFromExplicit<Mile, Inch>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 63360;
        }

        TNumber IConvertibleFromExplicit<Foot, Inch>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 12;
        }

        TNumber IConvertibleFromExplicit<Yard, Inch>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 36;
        }

        TNumber IConvertibleFromExplicit<Meter, Inch>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 0.0254;
        }
    }

    public class Mile : BaseUnit<Mile, Length>,
        IConvertibleFromExplicit<Meter, Mile>,
        IConvertibleFromExplicit<Yard, Mile>,
        IConvertibleFromExplicit<Foot, Mile>,
        IConvertibleFromExplicit<Inch, Mile>
    {
        TNumber IConvertibleFromExplicit<Meter, Mile>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1609.344;
        }

        TNumber IConvertibleFromExplicit<Yard, Mile>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1760;
        }

        TNumber IConvertibleFromExplicit<Foot, Mile>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 5280;
        }

        TNumber IConvertibleFromExplicit<Inch, Mile>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 63360;
        }
    }
}
#endif

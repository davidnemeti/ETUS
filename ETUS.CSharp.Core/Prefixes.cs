using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if false
namespace ETUS.Core.Prefixes
{
    public class Yotta<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Yotta<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Yotta<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E24;
        }
    }

    public class Zetta<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Zetta<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Zetta<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E21;
        }
    }

    public class Exa<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Exa<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Exa<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E18;
        }
    }

    public class Peta<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Peta<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Peta<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E15;
        }
    }

    public class Tera<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Tera<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Tera<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E12;
        }
    }

    public class Giga<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Giga<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Giga<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E9;
        }
    }

    public class Mega<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Mega<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Mega<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E6;
        }
    }

    public class Kilo<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Kilo<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Kilo<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E3;
        }
    }

    public class Hecto<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Hecto<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Hecto<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E2;
        }
    }

    public class Deca<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Deca<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Deca<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E1;
        }
    }

    public class Deci<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Deci<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Deci<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E-1;
        }
    }

    public class Centi<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Centi<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Centi<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E-2;
        }
    }

    public class Milli<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Milli<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Milli<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E-3;
        }
    }

    public class Micro<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Micro<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Micro<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E-6;
        }
    }

    public class Nano<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Nano<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Nano<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E-9;
        }
    }

    public class Pico<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Pico<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Pico<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E-12;
        }
    }

    public class Femto<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Femto<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Femto<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E-15;
        }
    }

    public class Atto<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Atto<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Atto<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E-18;
        }
    }

    public class Zepto<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Zepto<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Zepto<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E-21;
        }
    }

    public class Yocto<TUnit> : PrefixUnit<TUnit>, IConvertibleFromExplicit<TUnit, Yocto<TUnit>>
        where TUnit : Unit, new()
    {
        TNumber IConvertibleFromExplicit<TUnit, Yocto<TUnit>>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 1E-24;
        }
    }
}
#endif

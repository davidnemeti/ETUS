using System;
#if false
using Utilities.Measures.Prefixes;

namespace Utilities.Measures.Predefined
{
    public class Speed : Quantity { }

    public class Knot : BaseUnit<Knot, Speed>
    {
    }

    public class Kmph : BaseUnit<Kmph, Speed>,
        IConvertibleFromImplicit<UnitQuotient<Kilo<Meter>, Hour>, Kmph>
    {
    }

    public class Kmps : BaseUnit<Kmps, Speed>,
        IConvertibleFromImplicit<UnitQuotient<Kilo<Meter>, Second>, Kmps>
    {
    }

    public class Mps : BaseUnit<Mps, Speed>,
        IConvertibleFromImplicit<UnitQuotient<Mile, Second>, Mps>
    {
    }

    public class Mph : BaseUnit<Mph, Speed>,
        IConvertibleFromImplicit<UnitQuotient<Mile, Hour>, Mph>
    {
    }
}
#endif
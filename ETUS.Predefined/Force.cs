using System;
#if false
using Utilities.Measures.Prefixes;

namespace Utilities.Measures.Predefined
{
    public class Force : Quantity { }

    public class Newton : BaseUnit<Newton, Force>,
        IConvertibleFromImplicit<UnitProduct<Kilogram, UnitQuotient<Meter, UnitProduct<Second, Second>>>, Newton>,
        IConvertibleFromImplicit<UnitQuotient<UnitProduct<Kilogram, Meter>, UnitProduct<Second, Second>>, Newton>,
        IConvertibleFromImplicit<UnitQuotient<UnitProduct<Kilogram, UnitQuotient<Meter, Second>>, Second>, Newton>
    {
    }
}
#endif
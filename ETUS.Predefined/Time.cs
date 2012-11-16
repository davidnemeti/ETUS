using System;
#if false
using Utilities.Measures.Prefixes;

namespace Utilities.Measures.Predefined
{
    public class Time : Quantity { }

    public class Second : BaseUnit<Second, Time>,
        IConvertibleFromExplicit<Minute, Second>,
        IConvertibleFromExplicit<Hour, Second>,
        IConvertibleFromValue<double, TimeSpan>,
        IConvertibleToValue<double, TimeSpan>
    {
        double IConvertibleFromValue<double, TimeSpan>.ConvertFrom(TimeSpan timespan)
        {
            return timespan.TotalSeconds;
        }

        TimeSpan IConvertibleToValue<double, TimeSpan>.ConvertTo(double number)
        {
            return TimeSpan.FromSeconds(number);
        }

        TNumber IConvertibleFromExplicit<Minute, Second>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 60;
        }

        TNumber IConvertibleFromExplicit<Hour, Second>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 3600;
        }
    }

    public class Minute : BaseUnit<Minute, Time>,
        IConvertibleFromExplicit<Second, Minute>,
        IConvertibleFromExplicit<Hour, Minute>,
        IConvertibleFromValue<double, TimeSpan>,
        IConvertibleToValue<double, TimeSpan>
    {
        double IConvertibleFromValue<double, TimeSpan>.ConvertFrom(TimeSpan timespan)
        {
            return timespan.TotalMinutes;
        }

        TimeSpan IConvertibleToValue<double, TimeSpan>.ConvertTo(double number)
        {
            return TimeSpan.FromMinutes(number);
        }

        TNumber IConvertibleFromExplicit<Second, Minute>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 60;
        }

        TNumber IConvertibleFromExplicit<Hour, Minute>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) * 60;
        }
    }

    public class Hour : BaseUnit<Hour, Time>,
        IConvertibleFromExplicit<Second, Hour>,
        IConvertibleFromExplicit<Minute, Hour>,
        IConvertibleFromValue<double, TimeSpan>,
        IConvertibleToValue<double, TimeSpan>
    {
        double IConvertibleFromValue<double, TimeSpan>.ConvertFrom(TimeSpan timespan)
        {
            return timespan.TotalHours;
        }

        TimeSpan IConvertibleToValue<double, TimeSpan>.ConvertTo(double number)
        {
            return TimeSpan.FromHours(number);
        }

        TNumber IConvertibleFromExplicit<Second, Hour>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 3600;
        }

        TNumber IConvertibleFromExplicit<Minute, Hour>.ConvertFrom<TNumber>(TNumber number)
        {
            return ((dynamic)number) / 60;
        }
    }
}
#endif

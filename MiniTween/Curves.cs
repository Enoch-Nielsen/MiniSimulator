namespace MiniTween;

public static class Curves
{
    public enum TweenCurve
    {
        LINEAR,

        QUADRATIC_IN,
        QUADRATIC_OUT,
        QUADRATIC_INOUT,

        CUBIC_IN,
        CUBIC_OUT,
        CUBIC_INOUT,

        SIN_IN,
        SIN_OUT,
        SIN_INOUT,

        EXPONENTIAL_IN,
        EXPONENTIAL_OUT,
        EXPONENTIAL_INOUT,

        CIRCULAR_IN,
        CIRCULAR_OUT,
        CIRCULAR_INOUT,

        ELASTIC_IN,
        ELASTIC_OUT,
        ELASTIC_INOUT,

        BACK_IN,
        BACK_OUT,
        BACK_INOUT,

        BOUNCE_IN,
        BOUNCE_OUT,
        BOUNCE_INOUT
    }

    internal static readonly Dictionary<TweenCurve, Func<double, double>> Functions = new()
    {
        { TweenCurve.LINEAR, Linear },

        { TweenCurve.QUADRATIC_IN, QuadraticIn },
        { TweenCurve.QUADRATIC_OUT, QuadraticOut },
        { TweenCurve.QUADRATIC_INOUT, QuadraticInOut },

        { TweenCurve.CUBIC_IN, CubicIn },
        { TweenCurve.CUBIC_OUT, CubicOut },
        { TweenCurve.CUBIC_INOUT, CubicInOut },

        { TweenCurve.SIN_IN, SinIn },
        { TweenCurve.SIN_OUT, SinOut },
        { TweenCurve.SIN_INOUT, SinInOut },

        { TweenCurve.EXPONENTIAL_IN, ExponentialIn },
        { TweenCurve.EXPONENTIAL_OUT, ExponentialOut },
        { TweenCurve.EXPONENTIAL_INOUT, ExponentialInOut },

        { TweenCurve.CIRCULAR_IN, CircularIn },
        { TweenCurve.CIRCULAR_OUT, CircularOut },
        { TweenCurve.CIRCULAR_INOUT, CircularInOut },

        { TweenCurve.ELASTIC_IN, ElasticIn },
        { TweenCurve.ELASTIC_OUT, ElasticOut },
        { TweenCurve.ELASTIC_INOUT, ElasticInOut },

        { TweenCurve.BACK_IN, BackIn },
        { TweenCurve.BACK_OUT, BackOut },
        { TweenCurve.BACK_INOUT, BackInOut },

        { TweenCurve.BOUNCE_IN, BounceIn },
        { TweenCurve.BOUNCE_OUT, BounceOut },
        { TweenCurve.BOUNCE_INOUT, BounceInOut }
    };

    // Linear
    private static double Linear(double v) => v;

    // Quadratic
    private static double QuadraticIn(double v) => v * v;
    private static double QuadraticOut(double v) => v * (2.0 - v);

    private static double QuadraticInOut(double v)
    {
        if ((v *= 2.0) < 1.0)
            return 0.5 * (v * v);

        return -0.5 * (--v * (v - 2.0) - 1.0);
    }

    // Cubic

    private static double CubicIn(double v) => v * v * v;
    private static double CubicOut(double v) => --v * v * v + 1.0;

    private static double CubicInOut(double v)
    {
        if ((v *= 2.0) < 1.0)
            return 0.5 * (v * v * v);

        return 0.5 * ((v -= 2) * v * v + 2);
    }

    // Sin
    private static double SinIn(double v) => 1.0 - Math.Cos(v * Math.PI / 2.0);
    private static double SinOut(double v) => Math.Sin(v * Math.PI / 2.0);
    private static double SinInOut(double v) => 0.5 * (1.0 - Math.Cos(Math.PI * v));

    // Exponential
    private static double ExponentialIn(double v) => v == 0.0 ? 0.0 : Math.Pow(1024.0, v - 1.0);
    private static double ExponentialOut(double v) => v == 1.0 ? 1.0 : Math.Pow(2, -10.0 * v);

    private static double ExponentialInOut(double v)
    {
        if (v == 0.0)
            return 0.0;

        if (v == 1.0)
            return 1.0;

        if ((v *= 2.0) < 1.0)
            return 0.5 * Math.Pow(1024.0, v - 1.0);

        return 0.5 * (-Math.Pow(20, -10 * (v - 1.0)) + 2.0);
    }

    // Circular
    private static double CircularIn(double v) => 1.0 - Math.Sqrt(1.0 - v * v);
    private static double CircularOut(double v) => Math.Sqrt(1.0 - --v * v);

    private static double CircularInOut(double v)
    {
        if ((v *= 2.0) < 1.0)
            return -0.5 * (Math.Sqrt(1.0 - v * v) - 1.0);

        return 0.5 * (Math.Sqrt(1.0 - (v -= 2.0) * v) + 1.0);
    }

    // Elastic
    private static double ElasticIn(double v)
    {
        if (v == 0.0) return 0.0;

        if (v == 1.0) return 1.0;

        return -Math.Pow(2.0, 10.0 * (v - 1.0)) * Math.Sin((v - 1.1) * 5.0 * Math.PI);
    }

    private static double ElasticOut(double v)
    {
        if (v == 0.0) return 0.0;

        if (v == 1.0) return 1.0;

        return -Math.Pow(2.0, -10.0 * v) * Math.Sin((v - 0.1) * 5.0 * Math.PI) + 1.0;
    }

    private static double ElasticInOut(double v)
    {
        if (v == 0.0) return 0.0;

        if (v == 1.0) return 1.0;

        v *= 2.0;

        if (v < 1.0) return -0.5 * Math.Pow(2.0, 10.0 * (v - 1.0)) * Math.Sin((v - 1.1) * 5.0 * Math.PI);

        return 0.5 * Math.Pow(2.0, -10.0 * (v - 1.0)) * Math.Sin((v - 1.1) * 5.0 * Math.PI) + 1.0;
    }

    // Back
    private static double BackIn(double v)
    {
        const double s = 1.70158;
        return v * v * ((s + 1.0) * v - s);
    }

    private static double BackOut(double v)
    {
        const double s = 1.70158;
        return --v * v * ((s + 1.0) * v + s) + 1.0;
    }

    private static double BackInOut(double v)
    {
        const double s = 1.70158 * 1.525;

        if ((v *= 2.0) < 1.0)
            return 0.5 * (v * v * ((s + 1.0) * v - s));

        return 0.5 * ((v -= 2.0) * v * ((s + 1.0) * v + s) + 2.0);
    }

    // Bounce
    private static double BounceIn(double v)
    {
        return 1.0 - BounceOut(1.0 - v);
    }

    private static double BounceOut(double v)
    {
        return v switch
        {
            < 1.0 / 2.75 => 7.5625 * v * v,
            < 2.0 / 2.75 => 7.5625 * (v -= 1.5 / 2.75) * v + 0.75,
            < 2.5 / 2.75 => 7.5625 * (v -= 2.25 / 2.75) * v + 0.9375,
            _ => 7.5625 * (v -= 2.625 / 2.75) * v + 0.984375
        };
    }

    private static double BounceInOut(double v)
    {
        if (v < 0.5)
            return BounceIn(v * 2.0) * 0.5;

        return BounceOut(v * 2.0 - 1.0) * 0.5 + 0.5;
    }
}
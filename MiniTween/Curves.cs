namespace MiniTween;

public static class Curves
{
    public enum TweenCurve
    {
        LINEAR, 
        QUADRATIC_IN, QUADRATIC_OUT, QUADRATIC_INOUT,
    }
    
    // Linear
    public static double LinearEase(double v) => v;

    // Quadratic
    public static double QuadraticInEase(double v) => (v * v);
    public static double QuadraticOutEase(double v) => v * (2.0 - v);

    public static double QuadraticInOutEase(double v)
    {
        if ((v *= 2.0) < 1.0)
            return 0.5 * (v * v);

        return -0.5 * (--v * (v - 2.0) - 1.0);
    }
}
namespace LibMath;

public static class MathL
{
    /// <summary>
    /// Gets a random double value between "min" and "max"
    /// </summary>
    /// <param name="random"> The Random Instance. </param>
    /// <param name="min"> The Minimum Value </param>
    /// <param name="max"> The Maximum Value </param>
    /// <returns></returns>
    public static double RandomRange(Random random, double min, double max) => (random.NextDouble() * (max - min)) + min;

    /// <summary>
    /// Returns a linearly interpolated value, as a fraction of start towards end.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="fraction"></param>
    /// <returns></returns>
    public static double Lerp(double start, double end, double fraction) => (1.0 - fraction) * start + end * fraction;
    
    /// <summary>
    /// Remaps and returns the fraction of value between Min and Max. 0.0 - 1.0
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double InverseLerp(double min, double max, double value) => (value - min) / (max - min);
}
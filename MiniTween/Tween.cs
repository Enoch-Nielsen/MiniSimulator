using LibMath;

namespace MiniTween;

public static class Tween
{
    private const double TWEEN_THRESHOLD = 0.05;

    private static List<TweenInfo> _tweenValues = new();

    private static Dictionary<Curves.TweenCurve, Func<double, double>> Functions = new()
    {
        { Curves.TweenCurve.LINEAR, Curves.LinearEase },
        { Curves.TweenCurve.QUADRATIC_IN, Curves.QuadraticInEase },
        { Curves.TweenCurve.QUADRATIC_OUT, Curves.QuadraticOutEase },
        { Curves.TweenCurve.QUADRATIC_INOUT, Curves.QuadraticInOutEase }
    };

    /// <summary>
    ///     Subscribes the UpdateValues method to the given action delegate.
    /// </summary>
    /// <param name="action"></param>
    public static void Setup(Action<Action<double>> action) => action(UpdateValues);

    public static void TweenDouble(Func<double> getter, Action<double> setter,
        double tweenTarget, double tweenTime, Curves.TweenCurve tweenCurve = Curves.TweenCurve.LINEAR,
        bool overwrite = true)
    {
        VariableReference reference = new(getter, setter);

        if (_tweenValues.Any(t => t.VariableReference!.Equals(reference)))
        {
            if (overwrite)
                _tweenValues.RemoveAll(t => t.VariableReference!.Equals(reference));
            else
                return;
        }

        _tweenValues.Add(new TweenInfo
        {
            VariableReference = reference,
            OriginalValue = reference.Get(),
            TrueValue = reference.Get(),
            InterpolatedValue = reference.Get(),
            TweenTarget = tweenTarget,
            TweenTime = tweenTime,
            TweenCurve = tweenCurve
        });
    }

    private static void UpdateValues(double deltaTime)
    {
        List<TweenInfo> deque = [];

        foreach (TweenInfo tweenInfo in _tweenValues)
        {
            var increment = (tweenInfo.TweenTarget - tweenInfo.OriginalValue) * (deltaTime / tweenInfo.TweenTime);

            tweenInfo.TrueValue += increment;

            var dir = tweenInfo.TweenTarget - tweenInfo.OriginalValue < 0;
            if (Math.Abs(tweenInfo.TweenTarget - tweenInfo.TrueValue) < TWEEN_THRESHOLD
                || (dir && tweenInfo.TrueValue < tweenInfo.TweenTarget) ||
                (!dir && tweenInfo.TrueValue > tweenInfo.TweenTarget))
            {
                tweenInfo.VariableReference?.Set(tweenInfo.TweenTarget);
                deque.Add(tweenInfo);
                continue;
            }

            var inverse = dir
                ? MathL.InverseLerp(tweenInfo.TweenTarget, tweenInfo.OriginalValue, tweenInfo.TrueValue)
                : MathL.InverseLerp(tweenInfo.OriginalValue, tweenInfo.TweenTarget, tweenInfo.TrueValue);

            var funcValue = Functions[tweenInfo.TweenCurve](inverse);
            var interpolatedValue = dir
                ? MathL.Lerp(tweenInfo.TweenTarget, tweenInfo.OriginalValue, funcValue)
                : MathL.Lerp(tweenInfo.OriginalValue, tweenInfo.TweenTarget, funcValue);

            tweenInfo.InterpolatedValue = interpolatedValue;
            tweenInfo.VariableReference?.Set(tweenInfo.InterpolatedValue);
        }

        foreach (TweenInfo tweenInfo in deque)
            _tweenValues.Remove(tweenInfo);
    }
}

public class TweenInfo
{
    public double InterpolatedValue;
    public double OriginalValue;
    public double TrueValue;
    public Curves.TweenCurve TweenCurve;
    public double TweenTarget;
    public double TweenTime;
    internal VariableReference? VariableReference;
}

internal sealed class VariableReference
{
    public VariableReference(Func<double> getter, Action<double> setter)
    {
        Get = getter;
        Set = setter;
    }

    public Func<double> Get { get; }
    public Action<double> Set { get; }

    public override bool Equals(object? obj)
    {
        if (obj is not VariableReference other)
            return false;

        return Get == other.Get && Set == other.Set;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Get, Set);
    }
}
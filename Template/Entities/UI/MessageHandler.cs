using MiniTween;
using SkiaSharp;
using SkiaTemplate.Lib;
using SkiaTemplate.Objects;

namespace SkiaTemplate.Entities.UI;

public class MessageHandler : VisualEntity
{
    private static string _message = "";
    private static bool _isVisible;
    private static SKColor _currentColor = SKColors.Goldenrod;
    private static SKColor _defaultColor = SKColors.Goldenrod;
    private static SKFont _font = new();
    private static double _alpha = 255.0;

    public MessageHandler(Transform transform) : base(transform)
    {
        _font.Size = transform.Scale;
    }

    public override void Update(double deltaTime)
    {
    }

    protected override void Draw(SKCanvas canvas, SKPaint skPaint)
    {
        if (_alpha == 0.0) return;

        skPaint.Color = _currentColor.WithAlpha((byte)_alpha);
        canvas.DrawText(_message, Transform.Position.X, Transform.Position.Y, _font, skPaint);
    }

    /// <summary>
    /// Sends a message to the message bar on the screen for a set amount of time and color.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="seconds"></param>
    /// <param name="newColor"></param>
    public static void SendMessage(string message, double seconds = 3f, SKColor newColor = default)
    {
        _currentColor = newColor != default ? newColor : _defaultColor;
        
        _message = message;
        _isVisible = true;

        _alpha = 255.0;
        Tween.TweenDouble(() => _alpha, d => { _alpha = d; }, 0.0, seconds, Curves.TweenCurve.QUADRATIC_OUT);
    }
}
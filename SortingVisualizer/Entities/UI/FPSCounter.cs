using SkiaSharp;
using SkiaTemplate.Definitions;
using SkiaTemplate.Lib;
using SkiaTemplate.Objects;

namespace SkiaTemplate.Entities.UI;

public class FPSCounter : VisualEntity
{
    private const int FRAME_COUNT = 60;
    private SKColor _counterColor;
    private int _fps;
    private double _timer, _timerMax = 1.0;

    private List<double> _frames = [];

    private SKFont fpsFont = new();

    /// <summary>
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="counterColor"></param>
    public FPSCounter(Transform transform, SKColor counterColor) : base(transform)
    {
        _counterColor = counterColor;
        fpsFont.Size = transform.Scale;
    }

    public override void Update(double deltaTime)
    {
        TryAddFrame(deltaTime);

        if (_frames.Count > 0)
            _fps = (int)(1.0 / _frames.Average());

        if (_timer < _timerMax)
            _timer += deltaTime;
        else
            _timer = _timerMax;
    }

    protected override void Draw(SKCanvas canvas, SKPaint skPaint)
    {
        if (!SettingsDefinitions.ProgramSettings.ShowFps)
            return;

        skPaint.Color = _counterColor;
        canvas.DrawText(_fps.ToString(), Transform.Position.X, Transform.Position.Y, fpsFont, skPaint);
    }

    /// <summary>
    ///     Adds a frame to the fps counter
    /// </summary>
    /// <param name="deltaTime"></param>
    public void TryAddFrame(double deltaTime)
    {
        if (_frames.Count == FRAME_COUNT)
            _frames.RemoveAt(0);

        _frames.Add(deltaTime);
    }
}
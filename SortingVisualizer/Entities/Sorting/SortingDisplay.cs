using Silk.NET.SDL;
using SkiaSharp;
using SkiaTemplate.Lib;
using SkiaTemplate.Objects;
using Color = System.Drawing.Color;

namespace SkiaTemplate.Entities.Sorting;

public class SortingDisplay : VisualEntity
{
    // Screen Size / Val
    private const float VALUE_WIDTH = 12.5f, VALUE_HEIGHT = 800, VALUE_GAP = 5.5f;
    private const int VALUE_COUNT = 50;

    private TrackedIntArray _array;

    private SKColor normalColor;
    private SKColor viewColor;
    private SKColor modColor;

    public SortingDisplay(Transform transform) : base(transform)
    {
        int[] tempArray = new int[VALUE_COUNT];
        
        for (int i = 0; i < VALUE_COUNT; i++)
            tempArray[i] = i;
        
        _array = new TrackedIntArray(tempArray);
        
        SortingMethods.Scramble(_array, 300);
        _array.ResetTrackedValues();
        _array.SetNewBase(_array.Get());
        
        // Sort Array
        SortingMethods.BubbleSort(_array);
        _array.SetToBase();
        
        StepArray();
        
        normalColor = SKColors.WhiteSmoke;
        viewColor = SKColors.Yellow;
        modColor = SKColors.DarkRed;
    }

    public override void Update(double deltaTime)
    {
        
    }

    protected override void Draw(SKCanvas canvas, SKPaint skPaint)
    {
        TrackedIntArray.TrackingPoint[] changes = _array.GetChanges();
        int[] _tempArray = new int[_array.Get().Length];
        _array.Get().CopyTo(_tempArray, 0);
        
        for (int i = 0; i < _array.Get().Length; i++)
        {
            skPaint.Color = normalColor;
            
            if (changes.Length > 0)
                if (i == changes[_array.CurrentIndex].Index)
                {
                    skPaint.Color = changes[_array.CurrentIndex].TrackType == TrackedIntArray.TrackType.Modification ? modColor : modColor;
                }

            
            float height = VALUE_HEIGHT * ((float)_tempArray[i] / _tempArray.Length);
            canvas.DrawRoundRect((VALUE_GAP + VALUE_WIDTH) * (i+1) , WindowManager.HEIGHT - height, VALUE_WIDTH, height, 2f, 2f, skPaint);
        }
    }

    private async void StepArray()
    {
        for (int i = 0; i < _array.GetChanges().Length; i++)
        {
            _array.Step(true);
            await Task.Delay(100);
        }
    }
}
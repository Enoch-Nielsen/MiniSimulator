namespace SkiaTemplate.Entities.Sorting;

public class TrackedIntArray
{
    public List<TrackingPoint> TrackedChanges { get; private set; } = new();
    private int[] _values;
    private int[] _base;
    public int CurrentIndex { get; private set; }

    public enum TrackType
    {
        Modification,
        View
    }

    public TrackedIntArray(int[] values)
    { 
        _values = values;
        _base = new int[values.Length];
        values.CopyTo(_base,0);
    }

    public TrackingPoint[] GetChanges() => TrackedChanges.ToArray();
    public int[] Get() => _values;

    public int GetValue(int index)
    {
        TrackedChanges.Add(new TrackingPoint()
        {
            TrackType = TrackType.View,
            Index = index,
            Value = _values[index]
        });
        
        return _values[index];
    }

    public void ModifyValue(int index, int value)
    {
        TrackedChanges.Add(new TrackingPoint()
        {
            TrackType = TrackType.Modification,
            Index = index,
            Previous = _values[index],
            Value = value
        });
        
        _values[index] = value;
    }

    public TrackingPoint Step(bool direction)
    {
        CurrentIndex = Math.Clamp(CurrentIndex + (direction ? 1 : -1), 0, TrackedChanges.Count);
        
        TrackingPoint trackedIndex = TrackedChanges[CurrentIndex];
        _values[trackedIndex.Index] = direction ? trackedIndex.Value : trackedIndex.Previous;

        return trackedIndex;
    }

    public void SetToBase()
    {
        CurrentIndex = 0;
        _base.CopyTo(_values, 0);
    }

    public void SetNewBase(int[] array)
    {
        array.CopyTo(_base, 0);
    }

    public void ResetTrackedValues()
    {
        CurrentIndex = 0;
        TrackedChanges.Clear();
    }
    
    public class TrackingPoint
    {
        public TrackType TrackType;
        
        // If modification.
        public int Index;
        public int Previous;
        public int Value;
    }
}
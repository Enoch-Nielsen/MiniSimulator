namespace SkiaTemplate.Entities.Sorting;

public static class SortingMethods
{
    public static void Scramble(TrackedIntArray array, int times = 1)
    {
        Random random = new();
        
        for (int i = 0; i < times; i++)
        {
            foreach (var _ in array.Get())
            {
                int aIndex = random.Next(0, array.Get().Length-1);
                int bIndex = random.Next(0, array.Get().Length-1);

                int temp = array.GetValue(aIndex);
                array.ModifyValue(aIndex, array.GetValue(bIndex)); 
                array.ModifyValue(bIndex, temp); 
            }
        }
    }

    public static void BubbleSort(TrackedIntArray array)
    {
        for (int i = 0; i < array.Get().Length; i++)
        {
            for (int j = 0; j < (array.Get().Length-1) - i; j++)
            {
                if (array.GetValue(j) > array.GetValue(j + 1))
                {
                    int temp = array.GetValue(j);
                    array.ModifyValue(j, array.GetValue(j+1)); 
                    array.ModifyValue(j+1, temp);
                }
            }
        }
    }
}
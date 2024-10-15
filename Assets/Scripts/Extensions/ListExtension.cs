using System.Collections.Generic;
using Random = System.Random;

public static class ListExtension
{
    public static void GetRandomNumbersWithoutRepeat(this List<int> list, int count, int range)
    {
        Random random = new Random();
        
        for (int i = 0; i < count; i++)
        {
            int number = random.Next(0, range);
            while (list.Contains(number))
            {
                number = random.Next(0, range);
            }
            list.Add(number);
        }
    }
}
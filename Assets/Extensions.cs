using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public static class Extensions
{
    public static void Shuffle<T>(this IList<T> items)
    {
        for (var i = 0; i < items.Count - 1; ++i)
        {
            int random = Random.Range(i, items.Count);
            T item = items[random];

            items[random] = items[i];
            items[i] = item;
        }
    }

    public static int CountOf<T>(this IList<T> items, T item)
    {
        int count = 0; 

        for(int i = 0; i < items.Count; i++)
            if (items[i].Equals(item))
                count++;

        return count; 
    }
    
    public static void Process(this List<UnityAction<UnityAction>> phaseQueue, UnityAction callback, int i = 0)
    {
        if (phaseQueue.Count > i)
            phaseQueue[i].Invoke(() => Process(phaseQueue, callback, i + 1));
        else
            callback?.Invoke();
    }
}

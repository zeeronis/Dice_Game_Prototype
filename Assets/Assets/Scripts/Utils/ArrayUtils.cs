using System.Collections.Generic;
using UnityEngine;

public static class ArrayUtils
{
    #region Random sort 

    #region List

    public static void RandomSortWithCopy<T>(List<T> list, out List<T> sortedList)
    {
        sortedList = new List<T>(list);
        RandomSort(sortedList);
    }

    public static void RandomSortWithCopy<T>(List<T> list, out T[] sortedArray)
    {
        sortedArray = new T[list.Count];
        list.CopyTo(sortedArray, 0);

        RandomSort(sortedArray);
    }

    public static void RandomSort<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            var temp = list[i];

            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    #endregion

    #region Array

    public static void RandomSortWithCopy<T>(T[] array, out T[] sortedArray)
    {
        sortedArray = new T[array.Length];
        array.CopyTo(sortedArray, 0);

        RandomSort(sortedArray);
    }

    public static void RandomSortWithCopy<T>(T[] array, out List<T> sortedList)
    {
        sortedList = new List<T>(array);
        RandomSort(sortedList);
    }

    public static void RandomSort<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(0, array.Length);
            var temp = array[i];

            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    #endregion

    #endregion
}

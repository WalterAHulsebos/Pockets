using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Random = System.Random;

namespace Utilities.Extensions
{
    public static partial class ContainerExtensions
    {
        /// <summary>
        /// Get an item which returns true when overloaded in target function.
        /// </summary>
        public static T Get<T>(this List<T> list, Func<T, bool> func)
        {
            foreach (T t in list)
                if (func(t))
                    return t;
            return default;
        }

        /// <summary>
        /// Get index that returns true with with target function.
        /// </summary>
        public static int GetIndex<T>(this List<T> list, Func<T, bool> func)
        {
            var length = list.Count;
            for (var i = 0; i < length; i++)
                if (func(list[i]))
                    return i;
            return -1;
        }

        /// <summary>
        /// Get index that returns true with with target function.
        /// </summary>
        public static int GetIndex<T>(this T[] array, Func<T, bool> func)
        {
            var length = array.Length;
            for (var i = 0; i < length; i++)
                if (func(array[i]))
                    return i;
            return -1;
        }

        /// <summary>
        /// Get all items which returns true when overloaded in target function.
        /// </summary>
        public static List<T> GetMultiple<T>(this List<T> list, Func<T, bool> func, List<T> other = null)
        {
            if (other == null)
                other = new List<T>();
            foreach (T t in list)
                if (func(t))
                    other.Add(t);
            return other;
        }

        /// <summary>
        /// Get an item which returns true when overloaded in target function.
        /// </summary>
        public static T Get<T>(this T[] array, Func<T, bool> func)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
                if (func(array[i]))
                    return array[i];
            return default;
        }

        /// <summary>
        /// Get all items which returns true when overloaded in target function.
        /// </summary>
        public static List<T> GetMultiple<T>(this T[] array, Func<T, bool> func, List<T> other = null)
        {
            if (other == null)
                other = new List<T>();
            int length = array.Length;
            for (int i = 0; i < length; i++)
                if (func(array[i]))
                    other.Add(array[i]);
            return other;
        }

        public static List<T> GetMultiple<T>(this T[] array, Func<T, int, bool> func, List<T> other = null)
        {
            if (other == null)
                other = new List<T>();
            int length = array.Length;
            for (int i = 0; i < length; i++)
                if (func(array[i], i))
                    other.Add(array[i]);
            return other;
        }

        /// <summary>
        /// Order list by function.
        /// </summary>
        public static void EOrderBy<T>(this List<T> list, Func<T, int> func)
        {
            int listCount = list.Count, index;
            T current, other;
            int currentValue;

            for (int i = 1; i < listCount; i++)
            {
                index = i;
                current = list[index];
                currentValue = func(current);

                while (index > 0)
                {
                    other = list[index - 1];
                    if (func(other) > currentValue)
                        break;
                    list[index] = other;
                    index--;
                    list[index] = current;
                }
            }
        }

        /// <summary>
        /// First order the list by the speciate function, then order it based on the normal function.
        /// </summary>
        public static void EOrderBySpeciated<T>(this List<T> list, Func<T, int> func, Func<T, int> speciate)
        {
            int listCount = list.Count, index;
            T current, other;
            int currentValue;
            int currentSpeciateValue;

            for (int i = 1; i < listCount; i++)
            {
                index = i;
                current = list[index];
                currentValue = func(current);
                currentSpeciateValue = speciate(current);

                while (index > 0)
                {
                    other = list[index - 1];
                    if (currentSpeciateValue > speciate(other))
                        break;
                    if (currentSpeciateValue == speciate(other))
                        if (currentValue > func(other))
                            break;
                    list[index] = other;
                    index--;
                    list[index] = current;
                }
            }
        }

        /// <summary>
        /// Add values of other list to target list.
        /// </summary>
        public static void EAdd<T>(this List<T> list, List<T> other) => other.For(x => list.Add(x));

        /// <summary>
        /// Add values of other array to target list.
        /// </summary>
        public static void EAdd<T>(this List<T> list, T[] other) => other.For(x => list.Add(x));

        /// <summary>
        /// Add values of other list to target list that return true when overloaded with target function.
        /// </summary>
        public static void EAdd<T>(this List<T> list, List<T> other, Func<T, bool> func)
        {
            foreach (T t in other)
                if (func(t))
                    list.Add(t);
        }

        /// <summary>
        /// Add values of other list to target list that return true when overloaded with target function.
        /// </summary>
        public static void EAdd<T>(this List<T> list, List<T> other, Func<T, int, bool> func)
        {
            int length = other.Count;
            for (int i = 0; i < length; i++)
                if(func(other[i], i))
                    list.Add(other[i]);
        }

        /// <summary>
        /// Add values of other list to target list that return true when overloaded with target function.
        /// </summary>
        public static void EAdd<T, U>(this List<T> list, U[] other, Func<T, int, bool> func) where U : T
        {
            int length = other.Length;
            for (int i = 0; i < length; i++)
                if (func(other[i], i))
                    list.Add(other[i]);
        }

        /// <summary>
        /// Add values of other list to target array that return true when overloaded with target function.
        /// </summary>
        public static void EAdd<T>(this List<T> list, T[] other, Func<T, bool> func)
        {
            foreach (T t in other)
                if (func(t))
                    list.Add(t);
        }

        /// <summary>
        /// Check if target list contains a value that returns true when overloaded with target function.
        /// </summary>
        public static bool EContains<T>(this List<T> list, Func<T, bool> func)
        {
            foreach (T t in list)
                if (func(t))
                    return true;
            return false;
        }

        /// <summary>
        /// Get first value from list.
        /// </summary>
        public static T First<T>(this List<T> list) => list[0];

        /// <summary>
        /// Get last value from list.
        /// </summary>
        public static T Last<T>(this List<T> list) => list[list.Count - 1];

        /// <summary>
        /// Get middle value from list.
        /// </summary>
        public static T Middle<T>(this List<T> list) => list[Mathf.CeilToInt(list.Count / 2) - 1];

        /// <summary>
        /// Get middle value from list.
        /// </summary>
        public static T Middle<T>(this T[] array) => array[Mathf.CeilToInt(array.Length / 2) - 1];

        /// <summary>
        /// Get first value from list and remove it from said list.
        /// </summary>
        public static T Pop<T>(this List<T> list)
        {
            T item = list.First();
            list.RemoveAt(0);
            return item;
        }

        /// <summary>
        /// Execute a function for each item in target list.
        /// </summary>
        public static void For<T>(this List<T> list, Action<T> action)
        {
            foreach (T item in list)
                action(item);
        }

        /// <summary>
        /// Execute a function for each item in target array.
        /// </summary>
        public static void For<T>(this T[] array, Action<T, int> action)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
                action(array[i], i);
        }

        /// <summary>
        /// Execute a function for each item in target list.
        /// </summary>
        public static void For<T>(this List<T> list, Action<T, int> action)
        {
            int length = list.Count;
            for (int i = 0; i < length; i++)
                action(list[i], i);
        }

        /// <summary>
        /// Execute a function for each item in target array.
        /// </summary>
        public static void For<T>(this T[] array, Action<T> action)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
                action(array[i]);
        }

        public static void ERemoveAll<T>(this List<T> list, Func<T, bool> func)
        {
            int i = list.Count;
            while (i-- > 0)
                if (func(list[i]))
                    list.RemoveAt(i);
        }

        /// <summary>
        /// Check if target array contains a value that returns true when overloaded with target function.
        /// </summary>
        public static bool Contains<T>(this List<T> list, T value)
        {
            foreach (T item in list)
                if (list.Equals(value))
                    return true;
            return false;
        }

        /// <summary>
        /// Check if target array contains a value that returns true when overloaded with target function.
        /// </summary>
        public static bool Contains<T>(this T[] array, T value)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
                if (array[i].Equals(value))
                    return true;
            return false;
        }

        public static void For<T>(this T[,] grid, Action<T, int, int> action)
        {
            int lengthX = grid.GetLength(0),
                lengthY = grid.GetLength(1);
            for (int x = 0; x < lengthX; x++)
                for (int y = 0; y < lengthY; y++)
                    action(grid[x, y], x, y);
        }

        public static void For<T>(this T[,] grid, Action<T> action)
        {
            int lengthX = grid.GetLength(0),
                lengthY = grid.GetLength(1);
            for (int x = 0; x < lengthX; x++)
                for (int y = 0; y < lengthY; y++)
                    action(grid[x, y]);
        }

        /// <summary>
        /// Get the index of the highest returning value when overloaded in target function.
        /// </summary>
        public static int GetTopIndex<T>(this T[] array, Func<T, double> func)
        {
            int index = -1, length = array.Length;
            double score = double.MinValue;
            for (int i = 0; i < length; i++)
            {
                double currentScore = func(array[i]);
                if (currentScore < score)
                    continue;
                score = currentScore;
                index = i;
            }
            return index;
        }

        /// <summary>
        /// Get the index of the highest returning value when overloaded in target function.
        /// </summary>
        public static int GetTopIndex<T>(List<T> list, Func<T, double> func)
        {
            int index = -1, length = list.Count;
            double score = double.MinValue;
            for (int i = 0; i < length; i++)
            {
                double currentScore = func(list[i]);
                if (currentScore < score)
                    continue;
                score = currentScore;
                index = i;
            }
            return index;
        }

        /// <summary>
        /// Get a random item from target list.
        /// </summary>
        public static T GetRandom<T>(this List<T> list, Random random = null) => list[(random ?? GeneralExtensions.Random).Next(0, list.Count - 1)];

        /// <summary>
        /// Get a random item from target array.
        /// </summary>
        public static T GetRandom<T>(this T[] array, Random random = null) => array[(random ?? GeneralExtensions.Random).Next(0, array.Length - 1)];
    }
}
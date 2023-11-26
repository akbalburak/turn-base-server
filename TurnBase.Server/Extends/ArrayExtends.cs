﻿namespace TurnBase.Server.Extends
{
    public static class ArrayExtends
    {
        public static int IndexOf<T>(this T[] array, T item) where T : class
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == item)
                    return i;
            }

            return -1;
        }
    }
}

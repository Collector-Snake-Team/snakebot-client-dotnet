// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtentions.cs" company="Collector AB">
//   Copyright © Collector AB. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Cygni.Snake.Client
{
    using System;
    using System.Collections.Generic;

    public static class EnumerableExtentions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var value in enumerable)
            {
                action(value);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AzureConferenceLib.Helper
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source, ObservableCollection<T> collection)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            foreach (T item in source)
                collection.Add(item);

            return collection;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return new ObservableCollection<T>(source);
        }
    }
}

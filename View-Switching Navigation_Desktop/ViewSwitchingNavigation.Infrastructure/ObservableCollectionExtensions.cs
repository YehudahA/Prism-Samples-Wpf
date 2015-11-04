using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ViewSwitchingNavigation.Infrastructure
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T> (this ObservableCollection<T> targetCollection,IEnumerable<T> source)
        {
            foreach(T item in source)
            {
                targetCollection.Add(item);
            }
        }
    }
}

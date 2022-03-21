using System.Collections.Generic;

namespace PouleSimulator
{
    public static class IReadOnlyListExtension
    {
        /// <summary>
        /// As mentioned in the following stackoverflow post; IndexOf() is unavailable in <see cref="IReadOnlyList{T}"/> for no good reason whatsoever, so it's added as extension.
        /// https://stackoverflow.com/questions/37431844/why-ireadonlycollection-has-elementat-but-not-indexof#:~:text=ElementAt()%20iterates,an%20index%20does%20not%20apply.
        /// </summary>
        public static int IndexOf<T>(this IReadOnlyList<T> collection, T value) {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            for(int i = 0; i < collection.Count; i++) {
                if(comparer.Equals(collection[i], value))
                    return i;
            }

            return -1;
        }
    }
}

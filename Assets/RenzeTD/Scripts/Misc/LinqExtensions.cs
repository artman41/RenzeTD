using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq {
    public static class LinqExtensions {

        public static void ForEach<T1>(this IEnumerable<T1> collection, Action<T1> a) {
            foreach (var item in collection) {
                a(item);
            }
        }
        /// <summary>
        /// Gets the enum after the current
        /// </summary>
        /// <see cref="https://stackoverflow.com/questions/642542/how-to-get-next-or-previous-enum-value-in-c-sharp"/>
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length==j) ? Arr[0] : Arr[j];            
        }
    }
}

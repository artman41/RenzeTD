using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq {
    public static class LinqExtensions {

        /// <summary>
        /// An extension method that acts on all enumerable DataTypes the allows a foreach statement to occur within a lambda statement
        /// </summary>
        /// <param name="collection">the enumerable instance calling the method</param>
        /// <param name="a">the action to be performed</param>
        /// <typeparam name="T1">the type of each object</typeparam>
        public static void ForEach<T1>(this IEnumerable<T1> collection, Action<T1> a) {
            foreach (var item in collection) {
                a(item);
            }
        }
        /// <summary>
        /// Gets the enum after the current value
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCE561_jxl7581_FinalProject.Util
{
    public interface IHeap<T>
    {

        /// <summary>
        /// Removes the smallest element from the heap and returns it.
        /// </summary>
        /// <returns>
        /// The smallest element from the heap.
        /// </returns>
        T Extract();

        /// <summary>
        /// Returns the smallest element of the heap.
        /// </summary>
        /// <returns>
        /// The top element of the heap.
        /// </returns>
        T Top
        {
            get;
        }

        /// <summary>
        /// Adds the specified object to the heap.
        /// </summary>
        /// <param name="input">
        /// The object to add to the heap.
        /// </param>
        void Add(T input);

        /// <summary>
        /// Returns the size of the heap.
        /// </summary>
        /// <returns>
        /// The size of the heap.
        /// </returns>
        int Size
        {
            get;
        }

        /// <summary>
        /// Returns whether the heap is empty.
        /// </summary>
        /// <returns> 
        /// true if the heap is empty; false otherwise.
        ///</returns>
        bool IsEmpty
        {
            get;
        }

        /// <summary>
        /// Clears the contents of the heap.
        /// </summary>
        void Clear();
    }
}

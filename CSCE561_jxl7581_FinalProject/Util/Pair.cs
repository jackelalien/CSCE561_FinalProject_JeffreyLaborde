using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCE561_jxl7581_FinalProject.Util
{
    /// <summary>
	///  Dinky class to package pairs of things
	/// </summary>
	public sealed class Pair<A, B>
    {
        private A mFirst;
        private B mSecond;

        /// <summary>
        /// Value of the first object in the Pair.
        /// </summary>
        public A FirstValue
        {
            get
            {
                return mFirst;
            }
        }

        /// <summary>
        /// Value of the second object in the Pair.
        /// </summary>
        public B SecondValue
        {
            get
            {
                return mSecond;
            }
        }

        /// <summary>
        /// Constructor for the Pair object.
        /// </summary>
        /// <param name="first">
        /// First object to add to the Pair.
        /// </param>
        /// <param name="second">
        /// Second object to add to the Pair.
        /// </param>
        public Pair(A first, B second)
        {
            mFirst = first;
            mSecond = second;
        }

        /// <summary>
        /// Lists the values of the Pair object.
        /// </summary>
        /// <returns>
        /// String value.
        /// </returns>
        public override string ToString()
        {
            return "[" + mFirst.ToString() + "/" + mSecond.ToString() + "]";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCE561_jxl7581_FinalProject.Util
{
    public interface IBeamSearchContextGenerator : SharpEntropy.IContextGenerator
    {
        /// <summary>
        /// Returns the context for the specified position in the specified sequence (list).  </summary>
        /// <param name="index">
        /// The index of the sequence.
        /// </param>
        /// <param name="sequence">
        /// The sequence of items over which the beam search is performed.
        /// </param>
        /// <param name="priorDecisions">
        /// The sequence of decisions made prior to the context for which this decision is being made.
        /// </param>
        /// <param name="additionalContext">
        /// Any addition context specific to a class implementing this interface.
        /// </param>
        /// <returns>
        /// the context for the specified position in the specified sequence.
        /// </returns>
        string[] GetContext(int index, object[] sequence, string[] priorDecisions, object[] additionalContext);
    }
}

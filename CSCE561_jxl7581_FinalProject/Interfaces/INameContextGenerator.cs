using System;
using System.Collections;
using System.Collections.Generic;

namespace CSCE561_jxl7581_FinalProject.Interfaces
{
    public interface INameContextGenerator : Util.IBeamSearchContextGenerator
    {
        /// <summary>
        /// Returns the contexts for chunking of the specified index.
        /// </summary>
        /// <param name="tokenIndex">
        /// The index of the token in the specified tokens array for which the context should be constructed. 
        /// </param>
        /// <param name="tokens">
        /// The tokens of the sentence.
        /// </param>
        /// <param name="previousDecisions">
        /// The previous decisions made in the tagging of this sequence.  Only indices less than tokenIndex will be examined.
        /// </param>
        /// <param name="previousTags">
        /// A mapping between tokens and the previous outcome for these tokens. 
        /// </param>
        /// <returns>
        /// An array of predictive contexts on which a model basis its decisions.
        /// </returns>
        string[] GetContext(int tokenIndex, List<string> tokens, List<string> previousDecisions, IDictionary<string, string> previousTags);
    }

    public interface INameFinder
    {
        /// <summary>
        /// Generates name tags for the given sequence returning the result in a list.
        /// </summary>
        /// <param name="tokens">
        /// a list of the tokens or words of the sequence.
        /// </param>
        /// <param name="previousTags">
        /// a mapping between tokens and outcomes from previous sentences. 
        /// </param>
        /// <returns>
        /// a list of chunk tags for each token in the sequence.
        /// </returns>
        ArrayList Find(ArrayList tokens, IDictionary previousTags);

        /// <summary>
        /// Generates name tags for the given sequence returning the result in an array.
        /// </summary>
        /// <param name="tokens">
        /// an array of the tokens or words of the sequence.
        /// </param>
        /// <param name="previousTags">
        /// a mapping between tokens and outcomes from previous sentences. 
        /// </param>
        /// <returns>
        /// an array of chunk tags for each token in the sequence.
        /// </returns>
        string[] Find(object[] tokens, IDictionary previousTags);
    }
}

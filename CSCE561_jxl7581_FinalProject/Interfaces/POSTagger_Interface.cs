using System;
using System.Collections;


namespace CSCE561_jxl7581_FinalProject.Interfaces
{
    public interface POSTagger_Interface
    {
        /// <summary>
		/// Assigns the sentence of tokens pos tags.
		/// </summary>
		/// <param name="tokens">
		/// The sentence of tokens to be tagged.
		/// </param>
		/// <returns>
		/// a list of pos tags for each token provided in sentence.
		/// </returns>
		ArrayList Tag(ArrayList tokens);

        /// <summary>
        /// Assigns the sentence of tokens pos tags.</summary>
        /// <param name="tokens">
        /// The sentence of tokens to be tagged.
        /// </param>
        /// <returns>
        /// an array of pos tags for each token provided in sentence.
        /// </returns>
        string[] Tag(string[] tokens);

        /// <summary>
        /// Assigns pos tags to the sentence of space-delimited tokens.
        /// </summary>
        /// <param name="sentence">
        /// The sentence of space-delimited tokens to be tagged.
        /// </param>
        /// <returns>
        /// a string of space-delimited pos tags for each token provided in sentence.
        /// </returns>
        string TagSentence(string sentence);
    }
}

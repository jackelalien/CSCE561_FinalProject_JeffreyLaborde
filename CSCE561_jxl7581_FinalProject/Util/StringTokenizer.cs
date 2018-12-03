using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCE561_jxl7581_FinalProject.Util
{
    /// <summary>
	/// Class providing simple tokenization of a string, for manipulation.  
	/// For NLP tokenizing, see the OpenNLP.Tools.Tokenize namespace.
	/// </summary>
	public class StringTokenizer
    {
        private const string mDelimiters = " \t\n\r";   //The tokenizer uses the default delimiter set: the space character, the tab character, the newline character, and the carriage-return character	
        private string[] mTokens;
        int mPosition;

        /// <summary>
        /// Initializes a new class instance with a specified string to process
        /// </summary>
        /// <param name="input">
        /// String to tokenize
        /// </param>
        public StringTokenizer(string input) : this(input, mDelimiters.ToCharArray())
        {
        }

        public StringTokenizer(string input, string separators) : this(input, separators.ToCharArray())
        {
        }

        public StringTokenizer(string input, params char[] separators)
        {
            mTokens = input.Split(separators);
            mPosition = 0;
        }

        public string NextToken()
        {
            while (mPosition < mTokens.Length)
            {
                if ((mTokens[mPosition].Length > 0))
                {
                    return mTokens[mPosition++];
                }
                mPosition++;
            }
            return null;
        }

    }
}

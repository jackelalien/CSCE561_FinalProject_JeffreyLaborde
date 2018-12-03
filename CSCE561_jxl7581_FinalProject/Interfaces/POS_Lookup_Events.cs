using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSCE561_jxl7581_FinalProject.Interfaces
{
    class POS_Lookup_Events
    {
    }

    /// <summary>
	/// Provides a means of determining which tags are valid for a particular word based on a tag dictionary read from a file.
	/// </summary>
	public class PosLookupList
    {
        private Dictionary<string, string[]> mDictionary;
        private bool mIsCaseSensitive;

        public PosLookupList(string file) : this(file, true)
        {
        }

        /// <summary>
        /// Create tag dictionary object with contents of specified file and using specified case to determine how to access entries in the tag dictionary.
        /// </summary>
        /// <param name="file">
        /// The file name for the tag dictionary.
        /// </param>
        /// <param name="caseSensitive">
        /// Specifies whether the tag dictionary is case sensitive or not.
        /// </param>
        public PosLookupList(string file, bool caseSensitive) : this(new System.IO.StreamReader(file, System.Text.Encoding.UTF7), caseSensitive)
        {
        }

        /// <summary>
        /// Create tag dictionary object with contents of specified file and using specified case to determine how to access entries in the tag dictionary.
        /// </summary>
        /// <param name="reader">
        /// A reader for the tag dictionary.
        /// </param>
        /// <param name="caseSensitive">
        /// Specifies whether the tag dictionary is case sensitive or not.
        /// </param>
        public PosLookupList(System.IO.StreamReader reader, bool caseSensitive)
        {
            mDictionary = new Dictionary<string, string[]>();
            mIsCaseSensitive = caseSensitive;
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                string[] parts = line.Split(' ');
                string[] tags = new string[parts.Length - 1];
                for (int currentTag = 0, tagCount = parts.Length - 1; currentTag < tagCount; currentTag++)
                {
                    tags[currentTag] = parts[currentTag + 1];
                }
                mDictionary[parts[0]] = tags;
            }
        }

        /// <summary>
        /// Returns a list of valid tags for the specified word. </summary>
        /// <param name="word">
        /// The word.
        /// </param>
        /// <returns>
        /// A list of valid tags for the specified word or null if no information is available for that word.
        /// </returns>
        public virtual string[] GetTags(string word)
        {
            if (!mIsCaseSensitive)
            {
                word = word.ToLower(System.Globalization.CultureInfo.InvariantCulture);
            }

            if (mDictionary.ContainsKey(word))
            {
                return mDictionary[word];
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
	/// Class that helps generate part-of-speech lookup list files.
	/// </summary>
    public class PosLookupListWriter
    {
        private string mDictionaryFile;
        private Dictionary<string, Util.Set<string>> mDictionary;
        private Dictionary<string, int> mWordCounts;

        /// <summary>
        /// Creates a new part-of-speech lookup list, specifying the location to write it to.
        /// </summary>
        /// <param name="file">
        /// File to write the new list to.
        /// </param>
        public PosLookupListWriter(string file)
        {
            mDictionaryFile = file;
            mDictionary = new Dictionary<string, Util.Set<string>>();
            mWordCounts = new Dictionary<string, int>();
        }

        /// <summary>
        /// Adds an entry to the lookup list in memory, ready for writing to file.
        /// </summary>
        /// <param name="word">
        /// The word for which an entry should be added.
        /// </param>
        /// <param name="tag">
        /// The tag that should be marked as valid for this word.
        /// </param>
        public virtual void AddEntry(string word, string tag)
        {
            Util.Set<string> tags;
            if (mDictionary.ContainsKey(word))
            {
                tags = mDictionary[word];
            }
            else
            {
                tags = new Util.Set<string>();
                mDictionary.Add(word, tags);
            }
            tags.Add(tag);

            if (!(mWordCounts.ContainsKey(word)))
            {
                mWordCounts.Add(word, 1);
            }
            else
            {
                mWordCounts[word]++;
            }
        }

        /// <summary>
        /// Write the lookup list entries to file with a default cutoff of 5.
        /// </summary>
        public virtual void Write()
        {
            Write(5);
        }

        /// <summary>
        /// Write the lookup list entries to file.
        /// </summary>
        /// <param name="cutoff">
        /// The number of times a word must have been added to the lookup list for it to be considered important
        /// enough to write to file.
        /// </param>
        public virtual void Write(int cutoff)
        {
            using (StreamWriter writer = new StreamWriter(mDictionaryFile))
            {
                foreach (string word in mDictionary.Keys)
                {
                    if (mWordCounts[word] > cutoff)
                    {
                        writer.Write(word);
                        Util.Set<string> tags = mDictionary[word];
                        foreach (string tag in tags)
                        {
                            writer.Write(" ");
                            writer.Write(tag);
                        }
                        writer.Write(System.Environment.NewLine);
                    }
                }
                writer.Close();
            }
        }
    }

    public class PosEventReader : SharpEntropy.ITrainingEventReader
    {
        private System.IO.TextReader mTextReader;
        private IPosContextGenerator mContextGenerator;
        private List<SharpEntropy.TrainingEvent> mEventList = new List<SharpEntropy.TrainingEvent>();
        private int mCurrentEvent = 0;

        public PosEventReader(System.IO.TextReader data) : this(data, new POS_ContextGenerator())
        {
        }

        public PosEventReader(System.IO.TextReader data, IPosContextGenerator contextGenerator)
        {
            mContextGenerator = contextGenerator;
            mTextReader = data;
            string nextLine = mTextReader.ReadLine();
            if (nextLine != null)
            {
                AddEvents(nextLine);
            }
        }

        public virtual bool HasNext()
        {
            return (mCurrentEvent < mEventList.Count);
        }

        public virtual SharpEntropy.TrainingEvent ReadNextEvent()
        {
            SharpEntropy.TrainingEvent trainingEvent = mEventList[mCurrentEvent];
            mCurrentEvent++;
            if (mEventList.Count == mCurrentEvent)
            {
                mCurrentEvent = 0;
                mEventList.Clear();
                string nextLine = mTextReader.ReadLine();
                if (nextLine != null)
                {
                    AddEvents(nextLine);
                }
            }
            return trainingEvent;
        }

        private void AddEvents(string line)
        {
            Util.Pair<ArrayList, ArrayList> linePair = ConvertAnnotatedString(line);
            ArrayList tokens = linePair.FirstValue;
            ArrayList outcomes = linePair.SecondValue;
            List<string> tags = new List<string>();

            for (int currentToken = 0; currentToken < tokens.Count; currentToken++)
            {
                string[] context = mContextGenerator.GetContext(currentToken, tokens.ToArray(), tags.ToArray(), null);
                SharpEntropy.TrainingEvent posTrainingEvent = new SharpEntropy.TrainingEvent((string)outcomes[currentToken], context);
                tags.Add((string)outcomes[currentToken]);
                mEventList.Add(posTrainingEvent);
            }
        }

        private static Util.Pair<string, string> Split(string input)
        {
            int splitPosition = input.LastIndexOf("_");
            if (splitPosition == -1)
            {
                System.Console.Out.WriteLine("There is a problem in your training data: " + input + " does not conform to the format WORD_TAG.");
                return new Util.Pair<string, string>(input, "UNKNOWN");
            }
            return new Util.Pair<string, string>(input.Substring(0, (splitPosition) - (0)), input.Substring(splitPosition + 1));
        }

        public static Util.Pair<ArrayList, ArrayList> ConvertAnnotatedString(string input)
        {
            ArrayList tokens = new ArrayList();
            ArrayList outcomes = new ArrayList();
            Util.StringTokenizer tokenizer = new Util.StringTokenizer(input);
            string token = tokenizer.NextToken();
            while (token != null)
            {
                Util.Pair<string, string> linePair = Split(token);
                tokens.Add(linePair.FirstValue);
                outcomes.Add(linePair.SecondValue);
                token = tokenizer.NextToken();
            }
            return new Util.Pair<ArrayList, ArrayList>(tokens, outcomes);
        }

        //		[STAThread]
        //		public static void Main(string[] args)
        //		{
        //			string sData = "the_DT stories_NNS about_IN well-heeled_JJ communities_NNS and_CC developers_NNS";
        //			EventCollector oEventCollector = new PosEventCollector(new System.IO.StringReader(sData), new DefaultPosContextGenerator());
        //			Event[] aoEvents = oEventCollector.GetEvents();
        //			for (int iCurrentEvent = 0; iCurrentEvent < aoEvents.length; iCurrentEvent++)
        //			{
        //				System.Console.Out.WriteLine(aoEvents[iCurrentEvent].GetOutcome());
        //			}
        //		}
    }
}

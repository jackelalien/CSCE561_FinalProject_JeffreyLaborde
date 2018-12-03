using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CSCE561_jxl7581_FinalProject
{
    class POSTagger
    {
    }

    public interface IPosContextGenerator : Util.IBeamSearchContextGenerator
    {
        new string[] GetContext(int position, object[] tokens, string[] previousTags, object[] additionalContext);
    }

    public class POS_ContextGenerator : IPosContextGenerator
    {
        protected internal const string sentence_end = "*SE*";
        protected internal const string sentence_bgn = "*SB*";

        private const int mPrefix_Len = 4;
        private const int mSuffix_Len = 4;

        private static Regex mHasCapitalRegex = new Regex("[A-Z]");
        private static Regex mHasNumericRegex = new Regex("[0-9]");

        private Util.Cache mContextsCache;
        private object mWordsKey;

        public POS_ContextGenerator() : this(0)
        {
        }

        public POS_ContextGenerator(int cacheSize)
        {
            if (cacheSize > 0)
            {
                mContextsCache = new Util.Cache(cacheSize);
            }
        }

        public virtual string[] GetContext(object input)
        {
            object[] data = (object[])input;
            return GetContext(((int)data[0]), (object[])data[1], (string[])data[2], null);
        }

        protected internal static string[] GetPrefixes(string lex)
        {
            string[] prefixes = new string[mPrefix_Len];
            for (int currentPrefix = 0; currentPrefix < mPrefix_Len; currentPrefix++)
            {
                prefixes[currentPrefix] = lex.Substring(0, (System.Math.Min(currentPrefix + 1, lex.Length)) - (0));
            }
            return prefixes;
        }

        protected internal static string[] GetSuffixes(string lex)
        {
            string[] suffixes = new string[mSuffix_Len];
            for (int currentSuffix = 0; currentSuffix < mSuffix_Len; currentSuffix++)
            {
                suffixes[currentSuffix] = lex.Substring(System.Math.Max(lex.Length - currentSuffix - 1, 0));
            }
            return suffixes;
        }

        public virtual string[] GetContext(int index, object[] sequence, string[] priorDecisions, object[] additionalContext)
        {
            return GetContext(index, sequence, priorDecisions);
        }

        public virtual string[] GetContext(int index, object[] tokens, string[] tags)
        {
            string next, nextNext, lex, previous, previousPrevious;
            string tagPrevious, tagPreviousPrevious;
            tagPrevious = tagPreviousPrevious = null;
            next = nextNext = lex = previous = previousPrevious = null;

            lex = tokens[index].ToString();
            if (tokens.Length > index + 1)
            {
                next = tokens[index + 1].ToString();
                if (tokens.Length > index + 2)
                {
                    nextNext = tokens[index + 2].ToString();
                }
                else
                {
                    nextNext = sentence_end;
                }
            }
            else
            {
                next = sentence_end;
            }

            if (index - 1 >= 0)
            {
                previous = tokens[index - 1].ToString();
                tagPrevious = tags[index - 1].ToString();

                if (index - 2 >= 0)
                {
                    previousPrevious = tokens[index - 2].ToString();
                    tagPreviousPrevious = tags[index - 2].ToString();
                }
                else
                {
                    previousPrevious = sentence_bgn;
                }
            }
            else
            {
                previous = sentence_bgn;
            }

            string cacheKey = index.ToString(System.Globalization.CultureInfo.InvariantCulture) + tagPrevious + tagPreviousPrevious;
            if (!(mContextsCache == null))
            {
                if (mWordsKey == tokens)
                {
                    string[] cachedContexts = (string[])mContextsCache[cacheKey];
                    if (cachedContexts != null)
                    {
                        return cachedContexts;
                    }
                }
                else
                {
                    mContextsCache.Clear();
                    mWordsKey = tokens;
                }

            }

            List<string> eventList = new List<string>();

            // add the word itself
            eventList.Add("w=" + lex);

            // do some basic suffix analysis
            string[] suffixes = GetSuffixes(lex);
            for (int currentSuffix = 0; currentSuffix < suffixes.Length; currentSuffix++)
            {
                eventList.Add("suf=" + suffixes[currentSuffix]);
            }

            string[] prefixes = GetPrefixes(lex);
            for (int currentPrefix = 0; currentPrefix < prefixes.Length; currentPrefix++)
            {
                eventList.Add("pre=" + prefixes[currentPrefix]);
            }
            // see if the word has any special characters
            if (lex.IndexOf((char)'-') != -1)
            {
                eventList.Add("h");
            }

            if (mHasCapitalRegex.IsMatch(lex))
            {
                eventList.Add("c");
            }

            if (mHasNumericRegex.IsMatch(lex))
            {
                eventList.Add("d");
            }

            // add the words and positions of the surrounding context
            if ((object)previous != null)
            {
                eventList.Add("p=" + previous);
                if ((object)tagPrevious != null)
                {
                    eventList.Add("t=" + tagPrevious);
                }
                if ((object)previousPrevious != null)
                {
                    eventList.Add("pp=" + previousPrevious);
                    if ((object)tagPreviousPrevious != null)
                    {
                        eventList.Add("tt=" + tagPreviousPrevious);
                    }
                }
            }

            if ((object)next != null)
            {
                eventList.Add("n=" + next);
                if ((object)nextNext != null)
                {
                    eventList.Add("nn=" + nextNext);
                }
            }

            string[] contexts = eventList.ToArray();
            if (mContextsCache != null)
            {
                mContextsCache[cacheKey] = contexts;
            }
            return contexts;
        }
    }

    public class MaximumEntropyPOSTagger : Interfaces.POSTagger_Interface
    {
        public MaxEntropyModel_Interface mPosModel;
        public IPosContextGenerator mContextGenerator;
        private Interfaces.PosLookupList mDictionary;
        private bool mUseClosedClassTagsFilter = false;
        private const int mDefaultBeamSize = 3;
        private int mBeamSize;
        private Util.Sequence mBestSequence;

        public virtual string NegativeOutcome {  get { return ""; } }
        public virtual int NumTags { get { return mPosModel.OutcomeCount; } }
        protected MaxEntropyModel_Interface PosModel { get { return mPosModel; } set { mPosModel = value; } }
        protected IPosContextGenerator ContextGenerator { get { return mContextGenerator; } set { mContextGenerator = value; } }
        protected internal Interfaces.PosLookupList TagDictionary {  get { return mDictionary; } set { mDictionary = value; } }
        protected internal bool UseClosedClassTagsFilter {  get { return mUseClosedClassTagsFilter; } set { mUseClosedClassTagsFilter = value; } }
        protected internal int BeamSize { get { return mBeamSize; } set { mBeamSize = value; } }


        public virtual string[] AllTags()
        {
            string[] tags = new string[mPosModel.OutcomeCount];
            for (int currentTag = 0; currentTag < mPosModel.OutcomeCount; currentTag++)
            {
                tags[currentTag] = mPosModel.GetOutcomeName(currentTag);
            }
            return tags;
        }

        internal Util.BeamSearch Beam;

        // Constructors
        public MaximumEntropyPOSTagger(MaxEntropyModel_Interface model) : this(model, new POS_ContextGenerator())
        {
        }

        public MaximumEntropyPOSTagger(MaxEntropyModel_Interface model, Interfaces.PosLookupList dictionary) : this(mDefaultBeamSize, model, new POS_ContextGenerator(), dictionary)
        {
        }

        public MaximumEntropyPOSTagger(MaxEntropyModel_Interface model, IPosContextGenerator contextGenerator) : this(mDefaultBeamSize, model, contextGenerator, null)
        {
        }

        public MaximumEntropyPOSTagger(MaxEntropyModel_Interface model, IPosContextGenerator contextGenerator, Interfaces.PosLookupList dictionary) : this(mDefaultBeamSize, model, contextGenerator, dictionary)
        {
        }

        public MaximumEntropyPOSTagger(int beamSize, MaxEntropyModel_Interface model, IPosContextGenerator contextGenerator, Interfaces.PosLookupList dictionary)
        {
            mBeamSize = beamSize;
            mPosModel = model;
            mContextGenerator = contextGenerator;
            Beam = new PosBeamSearch(this, mBeamSize, contextGenerator, model);
            mDictionary = dictionary;
        }

        // End Constructors

        public virtual SharpEntropy.ITrainingEventReader GetEventReader(System.IO.TextReader reader)
        {
            return new Interfaces.PosEventReader(reader, mContextGenerator);
        }

        public virtual ArrayList Tag(ArrayList tokens)
        {
            mBestSequence = Beam.BestSequence(tokens, null);
            return new ArrayList(mBestSequence.Outcomes);
        }

        public virtual string[] Tag(string[] tokens)
        {
            mBestSequence = Beam.BestSequence(new ArrayList(tokens), null);
            return mBestSequence.Outcomes.ToArray();
        }

        public virtual void GetProbabilities(double[] probabilities)
        {
            mBestSequence.GetProbabilities(probabilities);
        }

        public virtual double[] GetProbabilities()
        {
            return mBestSequence.GetProbabilities();
        }

        public virtual string TagSentence(string sentence)
        {
            ArrayList tokens = new ArrayList(sentence.Split());
            ArrayList tags = Tag(tokens);
            System.Text.StringBuilder tagBuffer = new System.Text.StringBuilder();
            for (int currentTag = 0; currentTag < tags.Count; currentTag++)
            {
                tagBuffer.Append(tokens[currentTag] + "/" + tags[currentTag] + " ");
            }
            return tagBuffer.ToString().Trim();
        }

        public virtual void LocalEvaluate(MaxEntropyModel_Interface posModel, System.IO.StreamReader reader, out double accuracy, out double sentenceAccuracy)
        {
            mPosModel = posModel;
            float total = 0, correct = 0, sentences = 0, sentencesCorrect = 0;

            System.IO.StreamReader sentenceReader = new System.IO.StreamReader(reader.BaseStream, System.Text.Encoding.UTF7);
            string line;

            while ((object)(line = sentenceReader.ReadLine()) != null)
            {
                sentences++;
                Util.Pair<ArrayList, ArrayList> annotatedPair = Interfaces.PosEventReader.ConvertAnnotatedString(line);
                ArrayList words = annotatedPair.FirstValue;
                ArrayList outcomes = annotatedPair.SecondValue;
                ArrayList tags = new ArrayList(Beam.BestSequence(words, null).Outcomes);

                int count = 0;
                bool isSentenceOK = true;
                for (System.Collections.IEnumerator tagIndex = tags.GetEnumerator(); tagIndex.MoveNext(); count++)
                {
                    total++;
                    string tag = (string)tagIndex.Current;
                    if (tag == (string)outcomes[count])
                    {
                        correct++;
                    }
                    else
                    {
                        isSentenceOK = false;
                    }
                }
                if (isSentenceOK)
                {
                    sentencesCorrect++;
                }
            }

            accuracy = correct / total;
            sentenceAccuracy = sentencesCorrect / sentences;
        }

        private class PosBeamSearch : Util.BeamSearch
        {
            private MaximumEntropyPOSTagger mMaxentPosTagger;

            public PosBeamSearch(MaximumEntropyPOSTagger posTagger, int size, Util.IBeamSearchContextGenerator contextGenerator, MaxEntropyModel_Interface model) : base(size, contextGenerator, model)
            {
                mMaxentPosTagger = posTagger;
            }

            public PosBeamSearch(MaximumEntropyPOSTagger posTagger, int size, Util.IBeamSearchContextGenerator contextGenerator, MaxEntropyModel_Interface model, int cacheSize) : base(size, contextGenerator, model, cacheSize)
            {
                mMaxentPosTagger = posTagger;
            }

            protected internal override bool ValidSequence(int index, object[] inputSequence, string[] outcomesSequence, string outcome)
            {
                if (mMaxentPosTagger.TagDictionary == null)
                {
                    return true;
                }
                else
                {
                    string[] tags = mMaxentPosTagger.TagDictionary.GetTags(inputSequence[index].ToString());
                    if (tags == null)
                    {
                        return true;
                    }
                    else
                    {
                        return new ArrayList(tags).Contains(outcome);
                    }
                }
            }
            protected internal override bool ValidSequence(int index, ArrayList inputSequence, Util.Sequence outcomesSequence, string outcome)
            {
                if (mMaxentPosTagger.mDictionary == null)
                {
                    return true;
                }
                else
                {
                    string[] tags = mMaxentPosTagger.mDictionary.GetTags(inputSequence[index].ToString());
                    if (tags == null)
                    {
                        return true;
                    }
                    else
                    {
                        return new ArrayList(tags).Contains(outcome);
                    }
                }
            }
        }

        public virtual string[] GetOrderedTags(ArrayList words, ArrayList tags, int index)
        {
            return GetOrderedTags(words, tags, index, null);
        }

        public virtual string[] GetOrderedTags(ArrayList words, ArrayList tags, int index, double[] tagProbabilities)
        {
            double[] probabilities = mPosModel.Evaluate(mContextGenerator.GetContext(index, words.ToArray(), (string[])tags.ToArray(typeof(string)), null));
            string[] orderedTags = new string[probabilities.Length];
            for (int currentProbability = 0; currentProbability < probabilities.Length; currentProbability++)
            {
                int max = 0;
                for (int tagIndex = 1; tagIndex < probabilities.Length; tagIndex++)
                {
                    if (probabilities[tagIndex] > probabilities[max])
                    {
                        max = tagIndex;
                    }
                }
                orderedTags[currentProbability] = mPosModel.GetOutcomeName(max);
                if (tagProbabilities != null)
                {
                    tagProbabilities[currentProbability] = probabilities[max];
                }
                probabilities[max] = 0;
            }
            return orderedTags;
        }

        /// <summary>
		/// Trains a POS tag maximum entropy model.
		/// </summary>
		/// <param name="eventStream">
		/// Stream of training events
		/// </param>
		/// <param name="iterations">
		/// number of training iterations to perform.
		/// </param>
		/// <param name="cut">
		/// cutoff value to use for the data indexer.
		/// </param>
		/// <returns>
		/// Trained GIS model.
		/// </returns>
		public static SharpEntropy.GisModel Train(SharpEntropy.ITrainingEventReader eventStream, int iterations, int cut)
        {
            SharpEntropy.GisTrainer trainer = new SharpEntropy.GisTrainer();
            trainer.TrainModel(iterations, new SharpEntropy.TwoPassDataIndexer(eventStream, cut));
            return new SharpEntropy.GisModel(trainer);
        }

        /// <summary>
        /// Trains a POS tag maximum entropy model.
        /// </summary>
        /// <param name="trainingFile">
        /// filepath to the training data.
        /// </param>
        /// <returns>
        /// Trained GIS model.
        /// </returns>
        public static SharpEntropy.GisModel TrainModel(string trainingFile)
        {
            return TrainModel(trainingFile, 100, 5);
        }

        /// <summary>
        /// Trains a POS tag maximum entropy model.
        /// </summary>
        /// <param name="trainingFile">
        /// filepath to the training data.
        /// </param>
        /// <param name="iterations">
        /// number of training iterations to perform.
        /// </param>
        /// <param name="cutoff">
        /// Cutoff value to use for the data indexer.
        /// </param>
        /// <returns>
        /// Trained GIS model.
        /// </returns>
        public static SharpEntropy.GisModel TrainModel(string trainingFile, int iterations, int cutoff)
        {
            SharpEntropy.ITrainingEventReader eventReader = new Interfaces.PosEventReader(new System.IO.StreamReader(trainingFile));
            return Train(eventReader, iterations, cutoff);
        }


    }

    public class EnglishMaximumEntropyPosTagger : MaximumEntropyPOSTagger
    {

        public EnglishMaximumEntropyPosTagger(string modelFile, Interfaces.PosLookupList dictionary) : base(GetModel(modelFile), new POS_ContextGenerator(), dictionary)
        {
        }

        public EnglishMaximumEntropyPosTagger(string modelFile, string dictionary) : base(GetModel(modelFile), new POS_ContextGenerator(), new Interfaces.PosLookupList(dictionary))
        {
        }

        public EnglishMaximumEntropyPosTagger(string modelFile) : base(GetModel(modelFile), new POS_ContextGenerator())
        {
        }

        private static MaxEntropyModel_Interface GetModel(string name)
        {
            return new SharpEntropy.GisModel(new SharpEntropy.IO.BinaryGisModelReader(name));
        }
    }
}

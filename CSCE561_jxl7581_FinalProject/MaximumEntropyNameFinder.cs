using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCE561_jxl7581_FinalProject
{
    public class MaximumEntropyNameFinder : Interfaces.INameFinder
    {
        private MaxEntropyModel_Interface mModel;
        private Interfaces.INameContextGenerator mContextGenerator;
        private Util.Sequence mBestSequence;
        private Util.BeamSearch mBeam;

        public const string Start = "start";
        public const string Continue = "cont";
        public const string Other = "other";

        /// <summary>
        /// Creates a new name finder with the specified model.
        /// </summary>
        /// <param name="model">
        /// The model to be used to find names.
        /// </param>
        public MaximumEntropyNameFinder(MaxEntropyModel_Interface model) : this(model, new Interfaces.DefaultNameContextGenerator(10), 10)
        {
        }

        /// <summary>
        /// Creates a new name finder with the specified model and context generator.
        /// </summary>
        /// <param name="model">
        /// The model to be used to find names.
        /// </param>
        /// <param name="contextGenerator">
        /// The context generator to be used with this name finder.
        /// </param>
        public MaximumEntropyNameFinder(MaxEntropyModel_Interface model, Interfaces.INameContextGenerator contextGenerator) : this(model, contextGenerator, 10)
        {
        }

        /// <summary>
        /// Creates a new name finder with the specified model and context generator.
        /// </summary>
        /// <param name="model">
        /// The model to be used to find names.
        /// </param>
        /// <param name="contextGenerator">
        /// The context generator to be used with this name finder.
        /// </param>
        /// <param name="beamSize">
        /// The size of the beam to be used in decoding this model.
        /// </param>
        public MaximumEntropyNameFinder(MaxEntropyModel_Interface model, Interfaces.INameContextGenerator contextGenerator, int beamSize)
        {
            mModel = model;
            mContextGenerator = contextGenerator;
            mBeam = new NameBeamSearch(this, beamSize, contextGenerator, model, beamSize);
        }

        public virtual ArrayList Find(ArrayList tokens, IDictionary previousTags)
        {
            mBestSequence = mBeam.BestSequence(tokens, new object[] { previousTags });
            return new ArrayList(mBestSequence.Outcomes);
        }

        public virtual string[] Find(object[] tokens, IDictionary previousTags)
        {
            mBestSequence = mBeam.BestSequence(tokens, new object[] { previousTags });
            ArrayList outcomes = new ArrayList(mBestSequence.Outcomes);
            return (string[])outcomes.ToArray(typeof(string));
        }

        /// <summary>
        /// This method determines wheter the outcome is valid for the preceding sequence.  
        /// This can be used to implement constraints on what sequences are valid.  
        /// </summary>
        /// <param name="outcome">
        /// The outcome.
        /// </param>
        /// <param name="sequence">
        /// The preceding sequence of outcome assignments. 
        /// </param>
        /// <returns>
        /// true is the outcome is valid for the sequence, false otherwise.
        /// </returns>
        protected internal virtual bool ValidOutcome(string outcome, Util.Sequence sequence)
        {
            if (outcome == Continue)
            {
                string[] tags = sequence.Outcomes.ToArray();
                int lastTag = tags.Length - 1;
                if (lastTag == -1)
                {
                    return false;
                }
                else if (tags[lastTag] == Other)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Implementation of the abstract beam search to allow the name finder to use the common beam search code. 
        /// </summary>
        private class NameBeamSearch : Util.BeamSearch
        {
            private MaximumEntropyNameFinder mNameFinder;

            /// <summary>
            /// Creates a beam seach of the specified size using the specified model with the specified context generator.
            /// </summary>
            /// <param name="nameFinder">
            /// The associated MaximumEntropyNameFinder instance.
            /// </param>
            /// <param name="size">
            /// The size of the beam.
            /// </param>
            /// <param name="contextGenerator">
            /// The context generator used with the specified model.
            /// </param>
            /// <param name="model">
            /// The model used to determine names.
            /// </param>
            /// <param name="beamSize">
            /// The size of the beam to use in searching.
            /// </param>
            public NameBeamSearch(MaximumEntropyNameFinder nameFinder, int size, Interfaces.INameContextGenerator contextGenerator, MaxEntropyModel_Interface model, int beamSize) : base(size, contextGenerator, model, beamSize)
            {
                mNameFinder = nameFinder;
            }

            protected internal override bool ValidSequence(int index, ArrayList sequence, Util.Sequence outcomeSequence, string outcome)
            {
                return mNameFinder.ValidOutcome(outcome, outcomeSequence);
            }
        }

        /// <summary>
        /// Populates the specified array with the probabilities of the last decoded sequence.  The
        /// sequence was determined based on the previous call to <code>chunk</code>.  The 
        /// specified array should be at least as large as the numbe of tokens in the previous call to <code>chunk</code>.
        /// </summary>
        /// <param name="probabilities">
        /// An array used to hold the probabilities of the last decoded sequence.
        /// </param>
        public virtual void GetProbabilities(double[] probabilities)
        {
            mBestSequence.GetProbabilities(probabilities);
        }

        /// <summary>
        /// Returns an array with the probabilities of the last decoded sequence.  The
        /// sequence was determined based on the previous call to <code>chunk</code>.
        /// </summary>
        /// <returns>
        /// An array with the same number of probabilities as tokens were sent to <code>chunk</code>
        /// when it was last called.   
        /// </returns>
        public virtual double[] GetProbabilities()
        {
            return mBestSequence.GetProbabilities();
        }

        private static SharpEntropy.GisModel Train(SharpEntropy.ITrainingEventReader eventReader, int iterations, int cutoff)
        {
            SharpEntropy.GisTrainer trainer = new SharpEntropy.GisTrainer();
            trainer.TrainModel(iterations, new SharpEntropy.TwoPassDataIndexer(eventReader, cutoff));
            return new SharpEntropy.GisModel(trainer);
        }

        public static SharpEntropy.GisModel TrainModel(string trainingFile)
        {
            return TrainModel(trainingFile, 100, 5);
        }

        public static SharpEntropy.GisModel TrainModel(string trainingFile, int iterations, int cutoff)
        {
            SharpEntropy.ITrainingEventReader eventReader = new Interfaces.NameFinderEventReader(new SharpEntropy.PlainTextByLineDataReader(new System.IO.StreamReader(trainingFile)));
            return Train(eventReader, iterations, cutoff);
        }
    }
}

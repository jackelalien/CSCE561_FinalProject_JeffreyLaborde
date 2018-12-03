using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using java.io;
using java.util;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;

namespace CSCE561_jxl7581_FinalProject
{
    class DataRetrieval
    {
        public PowerPointManager pptManager;
        public List<string> ppts;
        public Dictionary<string, string> posDictionary;

        public Dictionary<string, string> termDictionary;
        public Dictionary<string, int> nameFreqDictionary;
        public Dictionary<string, int> locFreqDictionary;
        public Dictionary<string, int> ideaFreqDictionary;
        public Dictionary<string, int> eventFreqDictionary;
        public List<string> questions;

        private EnglishMaximumEntropyPosTagger mPosTagger;
        private EnglishNameFinder mNameFinder;
        public string mModelPath;



        public DataRetrieval(string modelPath)
        {
            ppts = new List<string>();
            posDictionary = new Dictionary<string, string>();
            termDictionary = new Dictionary<string, string>();
            nameFreqDictionary = new Dictionary<string, int>();
            locFreqDictionary = new Dictionary<string, int>();
            ideaFreqDictionary = new Dictionary<string, int>();
            eventFreqDictionary = new Dictionary<string, int>();
            questions = new List<string>();

            mModelPath = modelPath;

        }
        // Retrieve all the named entities from the powerpoint
        public void GetNamedEntities()
        {



        }

        // Get all terms from the powerpoint (by themselves, having : or  - after.... )
        public void GetTerms()
        {
            // If by themselves, look across or down. Look for > 2 words. 
        }

        public void GetTermImportance()
        {

        }

        public string[] PosTagTokens(string[] tokens)
        {
            if (mPosTagger == null)
            {
                mPosTagger = new EnglishMaximumEntropyPosTagger(mModelPath + "EnglishPOS.nbin", mModelPath + @"\Parser\tagdict.txt");
            }

            return mPosTagger.Tag(tokens);
        }

        public string FindNames(string sentence)
        {
            if (mNameFinder == null)
            {
                mNameFinder = new EnglishNameFinder(mModelPath + @"\NameFind\");
            }

            string[] models = new string[] { "date", "date_year", "location", "money", "organization", "percentage", "person", "time" };
            return mNameFinder.GetNames(models, sentence);
        }



    }


    public class ConditionalRandomField
    {
        // This was meant to hold the class for CRF.


        // 
    }

    public class TermFinder
    {

    }
}

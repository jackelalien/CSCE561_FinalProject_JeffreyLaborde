using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions;

namespace CSCE561_jxl7581_FinalProject
{
    public class Preprocessor
    {
        /// Preprocessing Steps:
        /// 1 - Strip unwanted charcters/markup (e.g. HTML tags, punctuation, numbers, etc)
        /// 2 - Stem tokens to root words
        /// 3 - Remove common stopwords
        /// 4 - Detect common phrases (maybe use a domain specific dictionary)
        /// 5 - Build inverted index (keyboard -> list of docs containing it)
        /// 
        List<string> stopwords = new List<string>();

        public void GetStopwordList(string stopWordPath)
        {
            StreamReader file = new StreamReader(stopWordPath);
            string line;

            while ((line = file.ReadLine()) != null)
            {
                //Ignore blanks
                if (line == null || line == "")
                {
                    continue;
                }
                else
                {
                    stopwords.Add(line);
                }
            }
        }

        // Step 1 - Strip Unwanted Characters/Markup
        public string StripCharacters(string line)
        {
            string ln;

            if (line.Length == 1 || line.Length == 2)
            {
                ln = "";
            }
            else
            {
                Regex reg = new Regex("[^a-zA-Z\\s -]");
                line = reg.Replace(line, "");
                ln = line;
            }

            return ln;
        }

        // Step 2 - Stem Tokens to Root Words
        public string StemLine(string line)
        {
            string[] ln = line.Split(' ');
            string currWord, newLine;
            newLine = "";

            PorterStemmer stemmer = new PorterStemmer();

            foreach (string word in ln)
            {
                currWord = stemmer.StemWord(word);
                newLine += currWord + " ";
            }

            return newLine;
        }

        // Step 3
        public string RemoveStopwords(string line)
        {
            string[] splitLine = line.Split(' ');
            string newLine = "";

            foreach (string word in splitLine)
            {
                if (stopwords.Contains(word))
                {
                    continue;
                }
                else
                {
                    newLine += word + " ";
                }
            }

            return newLine;

        }

        // Step 4
        public void getCommonPhrases()
        {

        }
    }
}

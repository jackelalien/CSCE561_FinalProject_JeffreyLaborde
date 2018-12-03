using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCE561_jxl7581_FinalProject
{
    public interface MaxEntropyModel_Interface
    {
        // Get number of outcomes
        int OutcomeCount { get; }

        // A list of string names of the contextual predicates which are to be evaluated together.
        // Returns an array of the probabilities for each of the different outcomes, all of which sum to 1.
        double[] Evaluate(string[] context);

        // Same as above, but also takes in an array which is populated with the probabilities for each of different outcomes, sum to 1.
        double[] Evaluate(string[] context, double[] probabilities);

        // Returns the outcome associated with the index containing the highest probability in the double array.
        // Returns the string name of best outcome.
        string GetBestOutcome(double[] outcomes);

        // Reutrn a string matching all outcome names with all probabilities produce by evaluation.
        // String containing outcome names paired with the normalized probability for each one.
        string GetAllOutcomes(double[] outcomes);

        // Gets the string name of the outcome associated with the supplied index.
        // Returns the string name of outcome.
        string GetOutcomeName(int index);

        // Gets index associated with string name of given outcome.
        int GetOutcomeIndex(string outcome);
    }
}

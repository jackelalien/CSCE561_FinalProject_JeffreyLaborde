using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSCE561_jxl7581_FinalProject.SharpEntropy
{
    class CustomModelTraining
    {
        private string modelPath;
        private string testPath;

        public string ModelFilePath { get { return modelPath; } set { modelPath = value;  } }
        public string TestFilePath { get { return testPath; } set { testPath = value; } }

        public CustomModelTraining(string model, string testFile)
        {
            modelPath = model;
            testPath = testFile;
        }

        public void TrainModel()
        {
            StreamReader trainingReader = new StreamReader(testPath);
            GisModel model;

            ITrainingEventReader eventReader = new BasicEventReader(new PlainTextByLineDataReader(trainingReader));
            GisTrainer trainer = new GisTrainer();
            trainer.TrainModel(eventReader);
            model = new GisModel(trainer);

            string modelDataFile = testPath.Replace(".txt", ".nbin"); // + ".nbin";
            IO.BinaryGisModelWriter writer = new IO.BinaryGisModelWriter();
            writer.Persist(model, modelDataFile);
            
        }

        public void SampleTest()
        {
            modelPath = @"D:\CSCE561\SharpNLP\sourceCode\sharp\sharpnlp\OpenNLP\OpenNLP\Models\NameFind\date_year.txt";
            testPath = @"D:\CSCE561\SharpNLP\sourceCode\sharp\sharpnlp\OpenNLP\OpenNLP\Models\NameFind\date_year.txt";

            TrainModel();
            IO.BinaryGisModelReader g = new IO.BinaryGisModelReader(@"D:\CSCE561\SharpNLP\sourceCode\sharp\sharpnlp\OpenNLP\OpenNLP\Models\NameFind\date_year.nbin");

        }
    }
}

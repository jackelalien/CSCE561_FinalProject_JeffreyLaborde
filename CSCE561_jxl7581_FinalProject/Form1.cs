using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;

using CSCE561_jxl7581_FinalProject.SharpEntropy;

namespace CSCE561_jxl7581_FinalProject
{
    public partial class Form1 : Form
    {
        List<GMarkerGoogle> locMarkers = new List<GMarkerGoogle>();
        List<PointLatLng> markerPoints = new List<PointLatLng>();
        GMapOverlay markerOverlay;

        private string mModelPath = @"D:\CSCE561\SharpNLP\sourceCode\sharp\sharpnlp\OpenNLP\OpenNLP\Models\";
        private string testCorpusPath = @"D:\CSCE561\SharpNLP\sourceCode\sharp\sharpnlp\OpenNLP\OpenNLP\Models\NameFind\Corpus.txt";
        DataRetrieval dR;
        CustomModelTraining cmt;
        

        PorterStemmer ps;

        double LatInit = 0.0;
        double LngInit = 0.0;


        public Form1()
        {
            InitializeComponent();

            StringBuilder output = new StringBuilder();
            dR = new DataRetrieval(mModelPath);

            cmt = new CustomModelTraining(mModelPath, mModelPath);

            cmt.SampleTest();

            //string[] sentences = SplitSentences(txtIn.Text);
            //string[] sentences = { "On the Indian subcontinent, the Delhi Sultanate and the Deccan sultanates would give way, beginning in 1450, to the Mughal Empire.", "In 6000 BCE the first farmers settled Crete.",
            //    "Large ships for fighting were called triremes",
            //    "The culture of Greece was influenced by the sea.",
            //    "The sea allowed travel and connected Greeks to the outside world.",
            //    "Many Greeks became fishermen and traders",
            //    "The greeks conquered the Aegean Sea",
            //    "They also had many scholars like Plato and Aristotle, and great kings like Leonidas.", "They fought great empires like Persia in the Peloponessian Wars"};

            string[] sentences = System.IO.File.ReadAllLines(testCorpusPath);

            foreach (string sentence in sentences)
            {
                string[] tokens = sentence.Split(' ');

                string[] tags = dR.PosTagTokens(tokens);

                

                for (int currentTag = 0; currentTag < tags.Length; currentTag++)
                {
                    output.Append(tokens[currentTag]).Append("/").Append(tags[currentTag]).Append(" ");
                }

                output.Append("\r\n\r\n");
            }

            output.Append("Name Finding...");

            foreach (string sentence in sentences)
            {
                output.Append(dR.FindNames(sentence)).Append("\r\n");
            }

            Console.WriteLine(output.ToString());
            //txtOut.Text = output.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.CanDragMap = true;
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(LatInit, LngInit);
            gMapControl1.MinZoom = 2;
            gMapControl1.MaxZoom = 24;
            gMapControl1.AutoScroll = true;
            gMapControl1.Zoom = 2;
        }

        private void BuildLocationMarkers()
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        // Add Presentations to the system.
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Browse for Presentation Files";
            openFileDialog1.Filter = "Powerpoint Presentations (*.pptx, *.ppt)|*.pptx;*.ppt";

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.Multiselect = true;

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] files = openFileDialog1.FileNames;

                foreach(string file in files)
                {
                    if(!pptListBox.Items.Contains(file))
                        pptListBox.Items.Add(file);
                }
            }  
            
        }


        // Remove Presentations from the selected ListBox
        private void button2_Click(object sender, EventArgs e)
        {
            if(pptListBox.SelectedIndex != -1)
            {
                pptListBox.Items.RemoveAt(pptListBox.SelectedIndex);
            }
        }

        // Retrieve Data
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void pptListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Browse for Path for the Trained Models";
            
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if(folderBrowserDialog1.SelectedPath != "")
                {
                    mModelPath = folderBrowserDialog1.SelectedPath;
                    textBox2.Text = mModelPath;
                }
            }


        }

        private void overviewTab_Click(object sender, EventArgs e)
        {

        }

        private void Test_Modified_TF_IDF()
        {

        }


        // NLP methods
        // private OpenNLP.Tools.SentenceDetect.MaximumEntropySentenceDetector mSentenceDetector;
        //private OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer mTokenizer;

        //private OpenNLP.Tools.NameFind.EnglishNameFinder mNameFinder;
        // private OpenNLP.Tools.Lang.English.TreebankLinker mCoreferenceFinder;

        /*        private string[] SplitSentences(string paragraph)
                {
                    if (mSentenceDetector == null)
                    {
                        mSentenceDetector = new EnglishMaximumEntropySentenceDetector(mModelPath + "EnglishSD.nbin");
                    }

                    return mSentenceDetector.SentenceDetect(paragraph);
                }

                private string[] TokenizeSentence(string sentence)
                {
                    if (mTokenizer == null)
                    {
                        mTokenizer = new EnglishMaximumEntropyTokenizer(mModelPath + "EnglishTok.nbin");
                    }

                    return mTokenizer.Tokenize(sentence);
                }*/


    }
}

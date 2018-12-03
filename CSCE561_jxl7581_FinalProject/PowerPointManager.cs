using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace CSCE561_jxl7581_FinalProject
{
    class PowerPointManager
    {
        public PowerPoint.Application pptApplication;
        public PowerPoint.Presentations multi_p;
        public PowerPoint.Presentation presentation;

        public PowerPointManager()
        {
            
        }

        public void OpenPPT(string filename)
        {
            pptApplication = new PowerPoint.Application();
            multi_p = pptApplication.Presentations;
            presentation = multi_p.Open(filename);
        }

        public void ClosePPT()
        {
            pptApplication.Quit();
        }

        public string GetSlideText()
        {
            string p_text = "";

            for(int i = 0; i < presentation.Slides.Count; i++)
            {
                // Get the title first
                var shapes = presentation.Slides[i + 1].Shapes;
                object oslideHasTitle = shapes.HasTitle;
                bool slidehasTitle = Convert.ToBoolean(oslideHasTitle);

                foreach (var item in presentation.Slides[i + 1].Shapes)
                {
                    var shape = (PowerPoint.Shape)item;

                    if (slidehasTitle)
                    {
                        if (shape == presentation.Slides[i + 1].Shapes.Title)
                        {
                            continue;
                        }
                    }

                    if (shape.HasTextFrame == MsoTriState.msoTrue)
                    {
                        
                        if (shape.TextFrame.HasText == MsoTriState.msoTrue)
                        {
                            var textRange = shape.TextFrame.TextRange;
                            var text = textRange.Text;
                            p_text += text + " ";
                        }
                    }

                    if (shape.HasTable == MsoTriState.msoTrue)
                    {
                        for (int a = 0; a < shape.Table.Rows.Count; a++)
                        {
                            for (int b = 0; b < shape.Table.Columns.Count; b++)
                            {
                                if (shape.Table.Cell(a, b).Shape.HasTextFrame == MsoTriState.msoTrue)
                                {
                                    if (shape.TextFrame.HasText == MsoTriState.msoTrue)
                                    {
                                        var textRange = shape.TextFrame.TextRange;
                                        var text = textRange.Text;
                                        p_text += text + " ";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return p_text;
        }



    }
}

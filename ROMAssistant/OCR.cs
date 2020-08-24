using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using System.Text.RegularExpressions;

namespace ROMAssistant
{
    public class OCR
    {
        private TesseractEngine Engine;
        public OCR()
        {
            Engine = new TesseractEngine("./tessdata", "eng", EngineMode.CubeOnly);
        }
        public string RawOCR(Bitmap Image)
        {
           var page = Engine.Process(Image);
            string text = page.GetText();
            page.Dispose();
            return text;
        }
        public static int ExtractTime(String text)
        {
            Regex regex = new Regex(@"(((\d+) min)|Appeared)");
            Match match = regex.Match(text);
            if (match.Value == "Appeared")
            {
                return 0;
            }
            else
            {
                if (match.Groups.Count >= 4)
                {
                    return int.Parse(match.Groups[3].Value);
                }
                return 30; // Return 30 minutes instead
            }

        }

        public List<int> QuickExtract(Bitmap Image)
        {
            String text = RawOCR(Image);
            System.Diagnostics.Debug.WriteLine(text);
            Regex regex = new Regex(@"(((?<time>\d+) min)|Appeared)");
            MatchCollection match = regex.Matches(text);
            List<int> timer = new List<int>();
            System.Diagnostics.Debug.WriteLine(match.Count.ToString());
            for (int i = 0; i < match.Count; i++)
            {
                if (match[i].Groups.Count >= 1)
                {
                    timer.Add(int.Parse(match[i].Groups["time"].Value));
                }
                else
                {
                    if (match[i].Value == "Appeared")
                        timer.Add(0);
                    else
                        timer.Add(30);
                }
            }
            return timer;
        }

    }
}

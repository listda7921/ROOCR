using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace ROMAssistant
{
    public static class StringProcessing
    {
        public static string OCR(Bitmap Image)
        {
            var ocr = new TesseractEngine("/dataset", "eng", EngineMode.TesseractAndCube);
            var page = ocr.Process(Image);
            return page.ToString();
        }
    }
}

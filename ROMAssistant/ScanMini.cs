using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant
{
    public class ScanMini
    {
        public AI _ai;
        public OCR _OCR;

        public ScanMini(AI ai, OCR ocr) {
            _ai = ai;
            _OCR = ocr;
        }

        //public Point ScanMini(string mini)
        //{
        //    Point MonsterImage;
        //    await this.Action.OpenMVP();

        //    return MonsterImage;
        //}

        public async Task ScanAllMonsters(List<Monster> monsterList)
        {
            await _ai.Action.OpenMVP();
            
            for (int i = 0; i < 10; i++)
            {
                ScanImage(monsterList);
                await _ai.Action.ClickScript();
            }

            await _ai.Action.CloseMVP();
        }

        private void ScanImage(List<Monster> monsterList)
        {
            foreach (var monster in monsterList)
            {
                Bitmap bmp = ImageSearch.PrintWindow((IntPtr)_ai.screenHandle);
                Point location = Scan(bmp, monster.GetImagePath());
                if (location.X == -1 && location.Y == -1) continue;
                location.X += 360;
                Bitmap crop = ImageSearch.CropImage(bmp, location, 180, 50);
                monster.MinutesToSpawn = OCR.ExtractTime(_OCR.RawOCR(crop));
            }
        }

        public Point Scan(Bitmap bmp, string imagePath)
        {
            var imagePoint = ImageSearch.SearchFromImage(bmp, imagePath);
            return imagePoint;
        }

    }
}

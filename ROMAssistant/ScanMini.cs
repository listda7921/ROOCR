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
        public Log _log;

        public ScanMini(AI ai, OCR ocr, Log log) {
            _ai = ai;
            _OCR = ocr;
            _log = log;
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
                //await _ai.Action.ScrollDown();
            }
            PostTimes(monsterList);
            await Task.Delay(2000);
            await _ai.Action.CloseMVP();
        }

        private void ScanImage(List<Monster> monsterList)
        {
            foreach (var monster in monsterList)
            {
                Bitmap bmp = ImageSearch.PrintWindow((IntPtr)_ai.screenHandle);
                var leftBmp = ImageSearch.CropImage(bmp, new Point(0,0), 720, 670);
                Point location = Scan(leftBmp, monster.GetImagePath());
                if (location.X == -1 && location.Y == -1) continue;
                location.X += 360;
                Bitmap crop = ImageSearch.CropImage(bmp, location, 185, 60);
                monster.MinutesToSpawn = OCR.ExtractTime(_OCR.RawOCR(crop));
                //_log.Info($"{monster.GetName()}: {monster.MinutesToSpawn} minutes");
            }
        }

        public Point Scan(Bitmap bmp, string imagePath)
        {
            var imagePoint = ImageSearch.SearchFromImage(bmp, imagePath);
            return imagePoint;
        }

        public void PostTimes(List<Monster> monsters)
        {
            foreach (var monster in monsters)
            {
                _log.Info($"{monster.GetName()}: {monster.MinutesToSpawn} minutes");
            }
        }

    }
}

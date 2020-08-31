using ROMAssistant.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant
{
    public class Bot
    {
        public async Task RunBot(Log log)
        {
            var active = true;
            AI ai = new AI(log);
            RotarZairo rotoZairo = new RotarZairo(ai);
            Smokie smokie = new Smokie(ai);
            EclipseSouthGate eclipseSG = new EclipseSouthGate(ai);
            EclipseLabyrinth eclipseL = new EclipseLabyrinth(ai);
            Mastering mastering = new Mastering(ai);
            VocalWestGate vocalWG = new VocalWestGate(ai);
            //toad
            //vagabond-wolf
            //dragon-fly
            //MonsterList monsterList = new MonsterList(
            //    //rotoZairo 
            //    smokie
            //    );

            var monsterList = new List<Monster>()
            {
                smokie,
                rotoZairo,
                eclipseSG,
                eclipseL,
                mastering,
                vocalWG
            };
            
            ScanMini scanMini = new ScanMini(ai, new OCR(), ai.Log);
            var _hunt = new Hunt(scanMini);
            var mobs = monsterList;//monsterList.GetMonsterList();
            
            while (active)
            {
                var nextMonster = await GetNextMonster(_hunt, mobs);
                await nextMonster.GoToLocation();
                await nextMonster.Hunt();
               //await ScheduleHunt(_hunt, mobs);
            }
        }

        private static async Task ScheduleHunt(Hunt _hunt, List<Monster> mobs)
        {
            await _hunt.GatherMobTimes(mobs);
            await _hunt.HuntMonsters(mobs);
        }

        public async Task<Monster> GetNextMonster(Hunt _hunt, List<Monster> mobs)
        {
            await _hunt.GatherMobTimes(mobs);
            return mobs.OrderBy(m => m.MinutesToSpawn).First();
        }
    }
}

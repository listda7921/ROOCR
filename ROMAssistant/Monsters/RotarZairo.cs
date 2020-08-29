using ROM.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant.Monsters
{
    public class RotarZairo : Monster
    {
        public string ImagePath = "resources/rotar-zairo.png";
        public AI _ai;
        public MonsterType Type = MonsterType.RotorZario;

        public RotarZairo(AI ai) {
            _ai = ai;
        }

        public override string GetImagePath()
        {
            return ImagePath;
        }

        public override async Task<int> GetSecondsToSpawn()
        {
            //await _mini.OpenMVP();
            await _ai.Action.OpenMVP();
            return 1;
        }

        public override MonsterType GetMonsterType()
        {
            return Type;
        }

        public override async Task GoToLocation()
        {
            //throw new NotImplementedException();
            await _ai.Action.teleportToGoblinForest();
            //await atLocation == true
            await _ai.Action.DelayOnLocation(MonsterType.RotorZario);
        }

        public override async Task Hunt()
        {
            //throw new NotImplementedException();
            await _ai.waitForSpawn(this.MinutesToSpawn * 1000);
        }
    }
}

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
        public string Name = "Rotor Zairo";
        public string Location = "Goblin Forest";
        public new int FightTime = 60;

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

        public override async Task<bool> GoToLocation()
        {
            var currentLocation = await _ai.Action.GetCurrentLocation();
            if (currentLocation == GetSpawnLocation()) return true;
            await _ai.Action.teleportToGoblinForest();
            return await _ai.Action.DelayOnLocation(MonsterType.RotorZario);
        }

        public override async Task Hunt()
        {
            //throw new NotImplementedException();
            await _ai.waitForSpawn(this.MinutesToSpawn * 60 * 1000, FightTime * 1000);
        }

        public override string GetName()
        {
            return Name;
        }

        public override string GetSpawnLocation()
        {
            return Location;
        }

        public override int GetFightTime()
        {
            return FightTime;
        }
    }
}

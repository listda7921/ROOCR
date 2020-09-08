using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant.Monsters
{
    public class Toad : Monster
    {
        public string ImagePath = "resources/Toad.png";
        public MonsterType Type = MonsterType.Toad;
        public string Name = "Toad";
        public string Location = "Underwater Cave";
        public new int FightTime = 60;

        public AI _ai;
        public Toad(AI ai)
        {
            _ai = ai;
        }
        public override int GetFightTime()
        {
            return FightTime;
        }

        public override string GetImagePath()
        {
            return ImagePath;
        }

        public override MonsterType GetMonsterType()
        {
            return Type;
        }

        public override string GetName()
        {
            return Name;
        }

        public override Task<int> GetSecondsToSpawn()
        {
            throw new NotImplementedException();
        }

        public override string GetSpawnLocation()
        {
            return Location;
        }

        public override async Task GoToLocation()
        {
            var currentLocation = await _ai.Action.GetCurrentLocation();
            if (currentLocation == GetSpawnLocation()) return;
            await _ai.Action.teleportToUnderWaterCave();
            await _ai.Action.DelayOnLocation(Type);
        }

        public override async Task Hunt()
        {
            await _ai.waitForSpawn(this.MinutesToSpawn * 60 * 1000, FightTime * 1000);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant.Monsters
{
    public class WoodGoblin : Monster
    {
        public string ImagePath = "resources/wood-goblin.png";
        public MonsterType Type = MonsterType.WoodGoblin;
        public string Name = "Wood Goblin";
        public string Location = "Payon South";
        public new int FightTime = 90;
        public AI _ai;

        public WoodGoblin(AI ai)
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

        public override async Task<bool> GoToLocation()
        {
            var currentLocation = await _ai.Action.GetCurrentLocation();
            if (currentLocation == GetSpawnLocation()) return true;
            await _ai.Action.teleportToPayonSouth();
            return await _ai.Action.DelayOnLocation(Type);
        }

        public override async Task Hunt()
        {
            await _ai.waitForSpawn(this.MinutesToSpawn * 60 * 1000, FightTime * 1000);
        }
    }
}

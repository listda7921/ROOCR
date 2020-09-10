using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant.Monsters
{
    public class DragonFly: Monster
    {
        public string ImagePath = "resources/dragon-fly.png";
        public MonsterType Type = MonsterType.DragonFly;
        public string Name = "Dragon Fly";
        public string Location = "Mjolnir Mountains";
        public new int FightTime = 40;

        public AI _ai;
        public DragonFly(AI ai)
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
            await _ai.Action.teleportToMjolnirMountains();
            return await _ai.Action.DelayOnLocation(MonsterType.DragonFly);
        }

        public override async Task Hunt()
        {
            await _ai.waitForSpawn(this.MinutesToSpawn * 60 * 1000, FightTime * 1000);
        }
    }
}

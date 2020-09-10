using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant.Monsters
{
    public class VagabondWolf : Monster
    {
        public string ImagePath = "resources/vagabond-wolf.png";
        public MonsterType Type = MonsterType.VagabondWolf;
        public string Name = "Vagabond Wolf";
        public string Location = "Sograt Desert";
        public new int FightTime = 70;

        public AI _ai;
        public VagabondWolf(AI ai)
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
            return MonsterType.VagabondWolf;
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
            await _ai.Action.teleportToDesert();
            return await _ai.Action.DelayOnLocation(Type);
        }

        public override async Task Hunt()
        {
            await _ai.waitForSpawn(this.MinutesToSpawn * 60 * 1000, FightTime * 1000);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant.Monsters
{
    public class Smokie : Monster
    {
        public string ImagePath = "resources/smokie2.png";
        public MonsterType Type = MonsterType.Smokie;
        public string Name = "Smokie";
        public string Location = "Prontera South Gate";
        public AI _ai;
        public Smokie(AI ai) {
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
            await _ai.Action.UseButterFlyWing();
            return await _ai.Action.RouteToMob(0, Type);//smokie
        }

        public override async Task Hunt()
        {
            await _ai.waitForSpawn(this.MinutesToSpawn * 60 * 1000, FightTime * 1000);
        }
    }
}

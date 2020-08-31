using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant.Monsters
{
    public class EclipseLabyrinth : Monster
    {
        public string ImagePath = "resources/Eclipse-Labyrinth.png";
        public MonsterType Type = MonsterType.EclipseL;
        public string Name = "Eclipse (Labyrinth)";
        public string Location = "Labyrinth Forest";

        public AI _ai;
        public EclipseLabyrinth(AI ai)
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
            await _ai.Action.UseButterFlyWing();
            await _ai.Action.RouteToMob(1, Type);//eclipse SG
        }

        public override async Task Hunt()
        {
            await _ai.waitForSpawn(this.MinutesToSpawn * 60 * 1000, FightTime * 1000);
        }
    }
}

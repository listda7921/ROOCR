using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace ROMAssistant.Monsters
{
    public class VocalLabyrinth : Monster
    {
        public string ImagePath = "resources/Basilisk.png";
        public MonsterType Type = MonsterType.VocalL;
        public string Name = "Vocal (Labyrinth)";
        public string Location = "Labyrinth Forest";
        public new int MonsterImagePositionOffsetY = -110;

        public AI _ai;
        public VocalLabyrinth(AI ai)
        {
            _ai = ai;
        }
        public override int GetFightTime()
        {
            return FightTime;
        }

        public override int GetMonsterImagePositionOffsetY()
        {
            return MonsterImagePositionOffsetY;
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
            await _ai.Action.RouteToMob(3, Type);//eclipse SG
        }

        public override async Task Hunt()
        {
            await _ai.waitForSpawn(this.MinutesToSpawn * 60 * 1000, FightTime * 1000);
        }
    }
}

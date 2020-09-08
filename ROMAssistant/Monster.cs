using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROMAssistant
{
    public abstract class Monster
    {
        public int FightTime { get; set; } = 30;
        public int MinutesToSpawn { get; set; } = 30;
        //public string ImagePath { get; set; }
        public int MonsterImagePositionOffsetY { get; set; } = 0;

        public abstract Task<int> GetSecondsToSpawn();
        public abstract Task GoToLocation();

        public abstract Task Hunt();

        public abstract string GetImagePath();

        public abstract MonsterType GetMonsterType();
        public abstract string GetName();
        public abstract string GetSpawnLocation();
        public abstract int GetFightTime();
        public virtual int GetMonsterImagePositionOffsetY()
        {
            return MonsterImagePositionOffsetY;
        }

    }

    public enum MonsterType
    {
        RotorZario,
        Smokie,
        EclipseS,
        EclipseL,
        Mastering,
        VocalWG,
        VocalL,
        VagabondWolf,
        Toad
    }
}

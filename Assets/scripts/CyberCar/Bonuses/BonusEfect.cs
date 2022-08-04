using System;

namespace CyberCar.Bonuses
{
    [Serializable]
    public class BonusEfect
    {
        public bool AddScore;
        public int ScoreCount;
        public bool SpeedBonus;
        public int Speed;
        public int SpeedTime;
        public bool FlyBonus;
        public int FlyTime;
        
    }
}
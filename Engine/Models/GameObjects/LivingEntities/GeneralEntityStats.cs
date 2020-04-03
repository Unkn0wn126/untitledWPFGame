using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.GameObjects.LivingEntities
{
    public class GeneralEntityStats : IEntityStats
    {
        private float _hp;
        // Stuff for checking entity alive state
        public float MaxHP { get; set; }
        public float HP { get { return _hp; } set { _hp = value <= MaxHP ? value : MaxHP; } }
        // How quickly it will move on the map... possible use for combat too
        public float Speed { get; set; }
        // How hard it hits
        public int Strength { get; set; }
        // Determines chance of missing the target
        public int Accuracy { get; set; }

        public GeneralEntityStats(float maxHp, float speed, int strength, int accuracy)
        {
            MaxHP = maxHp;
            HP = MaxHP;
            Speed = speed;
            Strength = strength;
            Accuracy = accuracy;
        }
    }
}

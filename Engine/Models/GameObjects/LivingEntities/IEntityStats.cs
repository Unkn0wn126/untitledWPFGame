using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.GameObjects
{
    public interface IEntityStats
    {
        public float MaxHP { get; set; }
        public float HP { get; set; }
        public float Speed { get; set; }
        public int Strength { get; set; }
        public int Accuracy { get; set; }
    }
}

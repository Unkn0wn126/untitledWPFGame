using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.GameObjects
{
    public interface ILivingEntity : IGameObject
    {
        public IEntityStats Stats { get; set; }
    }
}

using Engine.Models.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Factories
{
    public interface IGameObjectFactory
    {
        public IGameObject CreateObject();
    }
}

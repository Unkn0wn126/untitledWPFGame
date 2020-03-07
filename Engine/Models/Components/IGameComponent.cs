using Engine.Models.GameObjects;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components
{
    public interface IGameComponent
    {
        public void Update(IGameObject entity, IScene logicContext);
    }
}

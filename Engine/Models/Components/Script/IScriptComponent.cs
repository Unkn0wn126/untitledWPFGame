using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Script
{
    public interface IScriptComponent : IGameComponent
    {
        void Update();
    }
}

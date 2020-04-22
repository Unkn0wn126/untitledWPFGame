using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Script
{
    public enum ScriptType
    {
        None = 0,
        AiMovement = 1 << 0,
        FollowPlayer = 1 << 1,
        PlayerMovement = 1 << 2
    }
    public interface IScriptComponent : IGameComponent
    {
        void Update();
    }
}

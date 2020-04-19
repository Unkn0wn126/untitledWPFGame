using Engine.Models.Scenes;
using GameInputHandler;
using System;
using System.Collections.Generic;
using System.Text;
using TimeUtils;

namespace Engine.Models.Factories
{
    public interface ISceneFactory
    {
        IScene CreateScene(float xRes, float yRes, GameTime gameTime, GameInput gameInputHandler);
    }
}

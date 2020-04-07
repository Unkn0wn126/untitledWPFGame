using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Factories
{
    public interface ISceneFactory
    {
        IScene CreateScene();
    }
}

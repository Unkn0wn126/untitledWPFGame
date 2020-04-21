using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Scenes
{
    public interface ISceneManager
    {
        IScene CurrentScene { get; set; }
        IScene LoadBattleScene();
        IScene LoadNextScene();
    }
}

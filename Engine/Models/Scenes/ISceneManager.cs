using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Scenes
{
    public interface ISceneManager
    {
        IScene CurrentScene { get; set; }
        void LoadBattleScene();
        void LoadNextScene();
    }
}

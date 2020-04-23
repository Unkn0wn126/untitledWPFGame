using Engine.Models.Factories.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Scenes
{
    public interface ISceneManager
    {
        List<byte[]> MetaScenes { get; set; }
        IScene CurrentScene { get; set; }
        List<byte[]> GetScenesToSave();
        void UpdateScenes(List<byte[]> newScenes);
        IScene LoadBattleScene();
        IScene LoadNextScene();
    }
}

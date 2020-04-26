using Engine.Models.Factories.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Scenes
{
    public delegate void SceneChangeStarted();
    public delegate void SceneChangeFinished();
    public interface ISceneManager
    {
        public event SceneChangeStarted SceneChangeStarted;
        public event SceneChangeFinished SceneChangeFinished;
        List<byte[]> MetaScenes { get; set; }
        IScene CurrentScene { get; set; }
        int CurrentIndex { get; set; }
        List<byte[]> GetScenesToSave();
        void UpdateScenes(List<byte[]> newScenes);
        IScene LoadBattleScene();
        void LoadNextScene();
    }
}

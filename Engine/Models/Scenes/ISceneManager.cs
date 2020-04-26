using System.Collections.Generic;

namespace Engine.Models.Scenes
{
    public delegate void SceneChangeStarted();
    public delegate void SceneChangeFinished();
    public interface ISceneManager
    {
        event SceneChangeStarted SceneChangeStarted;
        event SceneChangeFinished SceneChangeFinished;
        List<byte[]> MetaScenes { get; set; }
        IScene CurrentScene { get; set; }
        int CurrentIndex { get; set; }
        List<byte[]> GetScenesToSave();
        void UpdateScenes(List<byte[]> newScenes);
        IScene LoadBattleScene();
        void LoadNextScene();
    }
}

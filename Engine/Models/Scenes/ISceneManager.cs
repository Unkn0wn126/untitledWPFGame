using System.Collections.Generic;

namespace Engine.Models.Scenes
{
    public delegate void SceneChangeStarted();
    public delegate void SceneChangeFinished();
    public delegate void GameWon();
    public interface ISceneManager
    {
        event SceneChangeStarted SceneChangeStarted;
        event SceneChangeFinished SceneChangeFinished;
        event GameWon GameWon;
        List<byte[]> MetaScenes { get; set; }
        IScene CurrentScene { get; set; }
        int CurrentIndex { get; set; }
        List<byte[]> GetScenesToSave();
        void UpdateScenes(List<byte[]> newScenes, int currentIndex);
        IScene LoadBattleScene();
        void LoadNextScene();
    }
}

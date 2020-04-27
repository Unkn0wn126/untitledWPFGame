using Engine.Models.Components.Life;
using System.Collections.Generic;

namespace Engine.Models.Scenes
{
    public delegate void SceneChangeStarted();
    public delegate void SceneChangeFinished();
    public delegate void GameEnd();
    public delegate void BattleInitialization(uint enemy);
    public interface ISceneManager
    {
        event SceneChangeStarted SceneChangeStarted;
        event SceneChangeFinished SceneChangeFinished;
        event GameEnd GameWon;
        event GameEnd GameLost;
        BattleSceneMediator BattleSceneMediator { get; set; }
        List<byte[]> MetaScenes { get; set; }
        IScene CurrentScene { get; set; }
        int CurrentIndex { get; set; }
        List<byte[]> GetScenesToSave();
        void UpdateScenes(List<byte[]> newScenes, int currentIndex);
        void LoadBattleScene(ILifeComponent player, ILifeComponent enemy);
        void LoadNextScene();
    }
}

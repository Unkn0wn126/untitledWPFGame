using Engine.Models.Components.Life;
using System.Collections.Generic;

namespace Engine.Models.Scenes
{
    public delegate void SceneChangeStarted();
    public delegate void SceneChangeFinished();
    public delegate void GameEnd();
    public delegate void BattleInitialization(uint enemy);

    /// <summary>
    /// A manager for the scenes.
    /// Takes care of loading the scenes
    /// as well as notifing other game
    /// systems of such change.
    /// Also can invoke victory or defeat
    /// state when asked to do so.
    /// </summary>
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

        /// <summary>
        /// Serializes the current state
        /// of the game
        /// </summary>
        /// <returns></returns>
        List<byte[]> GetScenesToSave();

        /// <summary>
        /// Basically loads a new game
        /// </summary>
        /// <param name="newScenes"></param>
        void UpdateScenes(List<byte[]> newScenes, int currentIndex);

        /// <summary>
        /// Loads a battle scene
        /// based on the provided life components
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        void LoadBattleScene(ILifeComponent player, ILifeComponent enemy);

        /// <summary>
        /// Loads the next scene in the list
        /// </summary>
        void LoadNextScene();
    }
}

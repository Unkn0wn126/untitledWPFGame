using Engine.Models.GameStateMachine;
using Engine.Models.Scenes;
using System.Collections.Generic;

namespace Engine.ViewModels
{
    /// <summary>
    /// Container of the current game context
    /// Keeps track of the context as a whole
    /// </summary>
    public interface IGame
    {
        GameStateMachine State { get; set; }
        ISceneManager SceneManager { get; set; }

        /// <summary>
        /// Initializes a new game instance
        /// </summary>
        /// <param name="metaScenes"></param>
        void InitializeGame(List<byte[]> metaScenes, int currentIndex);

        /// <summary>
        /// Updates the game logic.
        /// Should be called on every tick.
        /// </summary>
        void Update();

        /// <summary>
        /// Updates which entities should be visible
        /// </summary>
        void UpdateGraphics();

        /// <summary>
        /// Updates the processor context
        /// </summary>
        void UpdateProcessorContext();
    }
}

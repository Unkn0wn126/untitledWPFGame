using Engine.Models.Scenes;

namespace Engine.Processors
{
    /// <summary>
    /// Interface for logic processing
    /// </summary>
    public interface IProcessor
    {
        /// <summary>
        /// Should be called on every game update.
        /// Processes all the desired behaviour
        /// of the given type of game component.
        /// </summary>
        /// <param name="lastFrameTime"></param>
        void ProcessOneGameTick(float lastFrameTime);

        /// <summary>
        /// Allows to change the scene
        /// whose entities should be processed
        /// </summary>
        /// <param name="context"></param>
        void ChangeContext(IScene context);
    }
}

using System;

namespace Engine.Models.Components.Script
{
    /// <summary>
    /// Used to represent
    /// the combination
    /// of scripts an entity has
    /// </summary>
    [Flags]
    public enum ScriptType
    {
        None = 0,
        AiMovement = 1 << 0,
        SceneChanger = 1 << 1,
        PlayerMovement = 1 << 2
    }

    /// <summary>
    /// A game component used to
    /// create specific entity
    /// behavior.
    /// Its main purpose is
    /// to help implement
    /// game logic.
    /// </summary>
    public interface IScriptComponent : IGameComponent
    {
        /// <summary>
        /// Executes the commands
        /// saved in the implementation
        /// </summary>
        void Update();
    }
}

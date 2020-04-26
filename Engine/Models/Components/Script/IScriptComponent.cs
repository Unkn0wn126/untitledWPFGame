namespace Engine.Models.Components.Script
{
    public enum ScriptType
    {
        None = 0,
        AiMovement = 1 << 0,
        SceneChanger = 1 << 1,
        PlayerMovement = 1 << 2
    }
    public interface IScriptComponent : IGameComponent
    {
        void Update();
    }
}

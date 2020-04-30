namespace Engine.Models.Components.RigidBody
{
    /// <summary>
    /// Component used to represent the force
    /// applied to an entity from a given direction.
    /// Example usage would be moving an entity around
    /// or making a base for a platformer where
    /// the base force on Y axis pushes entities down.
    /// </summary>
    public interface IRigidBodyComponent : IGameComponent
    {
        float ForceX { get; set; }
        float ForceY { get; set; }
    }
}

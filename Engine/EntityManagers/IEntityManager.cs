using Engine.Coordinates;
using Engine.Models;
using Engine.Models.Components;
using Engine.Models.Components.Navmesh;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.EntityManagers
{
    /// <summary>
    /// This should hold all the entities and their components
    /// </summary>
    public interface IEntityManager
    {
        ISpatialIndex Coordinates { get; set; }
        List<uint> GetAllEntities();
        uint AddEntity();
        uint AddEntity(ITransformComponent transform);
        void RemoveEntity(uint id);
        void AddComponentToEntity(uint entityID, IGameComponent component);
        void UpdateActiveEntities(ITransformComponent focusPoint);

        // Transform
        ITransformComponent GetTransformComponent(uint entity);
        List<ITransformComponent> GetAllTransformComponents();
        List<ITransformComponent> GetAllActiveTransformComponents();
        
        // Graphics
        IGraphicsComponent GetGraphicsComponent(uint entity);
        List<IGraphicsComponent> GetAllGraphicsComponents();
        List<IGraphicsComponent> GetAllActiveGraphicsComponents();
        
        // Collision
        ICollisionComponent GetCollisionComponent(uint entity);
        List<ICollisionComponent> GetAllCollicionComponents();
        List<ICollisionComponent> GetAllActiveCollicionComponents();
        
        // Sound
        ISoundComponent GetSoundComponent(uint entity);
        List<ISoundComponent> GetAllSoundComponents();
        List<ISoundComponent> GetAllActiveSoundComponents();
        
        // Rigid Body
        IRigidBodyComponent GetRigidBodyComponent(uint entity);
        List<IRigidBodyComponent> GetAllRigidBodyComponents();
        List<IRigidBodyComponent> GetAllActiveRigidBodyComponents();

        // Scripts
        List<IScriptComponent> GetEntityScriptComponents(uint entity);
        List<List<IScriptComponent>> GetAllScriptComponents();
        List<List<IScriptComponent>> GetAllActiveScriptComponents();

        // Navmesh
        INavmeshComponent GetNavmeshComponent(uint entity);
        List<INavmeshComponent> GetAllNavmeshComponents();
        List<INavmeshComponent> GetAllActiveNavmeshComponents();

        List<uint> GetAllEntitiesPossessingComponent(Type componentType);
        List<uint> GetAllActiveEntities();
        bool EntityHasComponent(uint id, Type componentType);
    }
}

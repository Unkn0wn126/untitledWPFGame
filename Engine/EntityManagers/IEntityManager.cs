using Engine.Coordinates;
using Engine.Models;
using Engine.Models.Components;
using Engine.Models.Components.RigidBody;
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
        public ISpatialIndex Coordinates { get; set; }
        public List<uint> GetAllEntities();
        public uint AddEntity();
        public uint AddEntity(ITransformComponent transform);
        public void RemoveEntity(uint id);
        public void AddComponentToEntity(uint entityID, IGameComponent component);
        public void UpdateActiveEntities(ITransformComponent focusPoint);

        // Transform
        public ITransformComponent GetTransformComponent(uint entity);
        public List<ITransformComponent> GetAllTransformComponents();
        public List<ITransformComponent> GetAllActiveTransformComponents();
        
        // Graphics
        public IGraphicsComponent GetGraphicsComponent(uint entity);
        public List<IGraphicsComponent> GetAllGraphicsComponents();
        public List<IGraphicsComponent> GetAllActiveGraphicsComponents();
        
        // Collision
        public ICollisionComponent GetCollisionComponent(uint entity);
        public List<ICollisionComponent> GetAllCollicionComponents();
        public List<ICollisionComponent> GetAllActiveCollicionComponents();
        
        // Sound
        public ISoundComponent GetSoundComponent(uint entity);
        public List<ISoundComponent> GetAllSoundComponents();
        public List<ISoundComponent> GetAllActiveSoundComponents();
        
        // Rigid Body
        public IRigidBodyComponent GetRigidBodyComponent(uint entity);
        public List<IRigidBodyComponent> GetAllRigidBodyComponents();
        public List<IRigidBodyComponent> GetAllActiveRigidBodyComponents();

        public List<uint> GetAllEntitiesPossessingComponent(Type componentType);
        public List<uint> GetAllActiveEntities();
        public bool EntityHasComponent(uint id, Type componentType);
    }
}

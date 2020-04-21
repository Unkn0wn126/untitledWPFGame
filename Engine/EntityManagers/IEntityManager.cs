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
        void AddComponentToEntity<T>(uint entityID, T component) where T : IGameComponent;
        void UpdateActiveEntities(ITransformComponent focusPoint);

        T GetComponentOfType<T>(uint entity) where T : IGameComponent;
        List<T> GetAllComponentsOfType<T>() where T : IGameComponent;
        List<T> GetAllActiveComponentsOfType<T>() where T : IGameComponent;

        // Scripts
        List<IScriptComponent> GetEntityScriptComponents(uint entity);
        List<List<IScriptComponent>> GetAllScriptComponents();
        List<List<IScriptComponent>> GetAllActiveScriptComponents();

        List<uint> GetAllEntitiesPossessingComponent(Type componentType);
        List<uint> GetAllActiveEntities();
        bool EntityHasComponent<T>(uint id) where T : IGameComponent;
    }
}

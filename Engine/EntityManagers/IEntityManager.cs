using Engine.Coordinates;
using Engine.Models.Components;
using Engine.Models.Components.Script;
using System;
using System.Collections.Generic;

namespace Engine.EntityManagers
{
    /// <summary>
    /// Holds all entities and their respective
    /// components.
    /// </summary>
    public interface IEntityManager
    {
        ISpatialIndex Coordinates { get; set; }

        /// <summary>
        /// Gets all entitie present
        /// in this entity manager
        /// </summary>
        /// <returns></returns>
        List<uint> GetAllEntities();

        /// <summary>
        /// Adds a new entity
        /// to this manager
        /// </summary>
        /// <returns></returns>
        uint AddEntity();

        /// <summary>
        /// Adds a new entity
        /// to this manager
        /// and assigns it
        /// a position
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        uint AddEntity(ITransformComponent transform);

        /// <summary>
        /// Removes an entity
        /// with a given id
        /// </summary>
        /// <param name="id"></param>
        void RemoveEntity(uint id);

        /// <summary>
        /// Adds a provided component
        /// to an entity with a given id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityID"></param>
        /// <param name="component"></param>
        void AddComponentToEntity<T>(uint entityID, T component) where T : IGameComponent;

        /// <summary>
        /// Updates the list of active entities
        /// </summary>
        /// <param name="focusPoint"></param>
        void UpdateActiveEntities(ITransformComponent focusPoint);

        /// <summary>
        /// Gets a component of given type
        /// belonging to a given entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        T GetComponentOfType<T>(uint entity) where T : IGameComponent;

        /// <summary>
        /// Gets all of the components
        /// of the given type regardless
        /// of the owner entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetAllComponentsOfType<T>() where T : IGameComponent;

        /// <summary>
        /// Gets all of the components
        /// of the given type regardless
        /// of the owner entity, except
        /// for its membership in active
        /// entities list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetAllActiveComponentsOfType<T>() where T : IGameComponent;

        /// <summary>
        /// Gets all script components
        /// assigned to a given entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        List<IScriptComponent> GetEntityScriptComponents(uint entity);

        /// <summary>
        /// Gets all script components
        /// regardless of the owner entity
        /// </summary>
        /// <returns></returns>
        List<List<IScriptComponent>> GetAllScriptComponents();

        /// <summary>
        /// Gets all script components
        /// regardless of the owner entity
        /// except for its membership in
        /// active entities
        /// </summary>
        /// <returns></returns>
        List<List<IScriptComponent>> GetAllActiveScriptComponents();

        /// <summary>
        /// Gets a list of all entities
        /// that have the given component
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        List<uint> GetAllEntitiesPossessingComponent(Type componentType);

        /// <summary>
        /// Gets a list of all
        /// active entities
        /// </summary>
        /// <returns></returns>
        List<uint> GetAllActiveEntities();

        /// <summary>
        /// Checks if the given entity
        /// has the given component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        bool EntityHasComponent<T>(uint id) where T : IGameComponent;
    }
}

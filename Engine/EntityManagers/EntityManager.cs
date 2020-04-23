using Engine.Coordinates;
using Engine.Models.Components;
using Engine.Models.Components.Life;
using Engine.Models.Components.Navmesh;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script;
using System;
using System.Collections.Generic;

namespace Engine.EntityManagers
{
    public class EntityManager : IEntityManager
    {
        private uint _maxValue;
        private List<uint> _activeEntities;
        private readonly List<uint> _entities;
        private readonly Dictionary<Type, Dictionary<uint, IGameComponent>> _gameComponents;
        private readonly Dictionary<uint, List<IScriptComponent>> _scriptComponents;

        public ISpatialIndex Coordinates { get; set; }

        public EntityManager(ISpatialIndex grid)
        {
            Coordinates = grid;
            _maxValue = uint.MinValue;
            _entities = new List<uint>();
            _activeEntities = new List<uint>();
            _gameComponents = new Dictionary<Type, Dictionary<uint, IGameComponent>>();
            InitComponentHolders();
            _scriptComponents = new Dictionary<uint, List<IScriptComponent>>();
        }

        private void InitComponentHolders()
        {
            _gameComponents.Add(typeof(ITransformComponent), new Dictionary<uint, IGameComponent>());
            _gameComponents.Add(typeof(IGraphicsComponent), new Dictionary<uint, IGameComponent>());
            _gameComponents.Add(typeof(ICollisionComponent), new Dictionary<uint, IGameComponent>());
            _gameComponents.Add(typeof(ISoundComponent), new Dictionary<uint, IGameComponent>());
            _gameComponents.Add(typeof(IRigidBodyComponent), new Dictionary<uint, IGameComponent>());
            _gameComponents.Add(typeof(INavmeshComponent), new Dictionary<uint, IGameComponent>());
            _gameComponents.Add(typeof(ILifeComponent), new Dictionary<uint, IGameComponent>());
        }

        public List<uint> GetAllEntities()
        {
            return _entities;
        }

        public void UpdateActiveEntities(ITransformComponent focusPoint)
        {
            _activeEntities = Coordinates.GetObjectsInRadius(focusPoint, 3);
            foreach (var item in _entities)
            {
                // assuming they have no transform for this reason
                if (!EntityHasComponent<ITransformComponent>(item))
                    _activeEntities.Add(item);
            }
        }

        public void AddComponentToEntity<T>(uint entityID, T component) where T : IGameComponent
        {
            if (component is IScriptComponent)
            {
                AddScriptComponent(entityID, (IScriptComponent)component);
                return;
            }

            if (!_gameComponents.ContainsKey(typeof(T)))
                throw new Exception("Invalid component type");

            _gameComponents[typeof(T)].Add(entityID, component);

            if (component is ITransformComponent)
                Coordinates.Add(entityID, ((ITransformComponent)component).Position);
        }

        private void AddScriptComponent(uint entityID, IScriptComponent sc)
        {
            if (!_scriptComponents.ContainsKey(entityID))
                _scriptComponents.Add(entityID, new List<IScriptComponent>());

            _scriptComponents[entityID].Add(sc);
        }

        public uint AddEntity()
        {
            uint temp = _maxValue;
            _entities.Add(_maxValue++);
            return temp;
        }

        private void RemoveEntityComponents(uint id)
        {
            foreach (var item in _gameComponents.Keys)
            {
                if (_gameComponents[item].ContainsKey(id))
                    _gameComponents[item].Remove(id);
            }
            
            if (_scriptComponents.ContainsKey(id))
                _scriptComponents.Remove(id); 
        }

        public void RemoveEntity(uint id)
        {
            RemoveEntityComponents(id);

            _entities.Remove(id);
        }

        public List<uint> GetAllEntitiesPossessingComponent(Type componentType)
        {
            if (!_gameComponents.ContainsKey(componentType))
                throw new Exception("Invalid component type");

            return new List<uint>(_gameComponents[componentType].Keys);
        }

        public uint AddEntity(ITransformComponent transform)
        {
            uint temp = AddEntity();

            AddComponentToEntity(temp, transform);
            return temp;
        }

        public T GetComponentOfType<T> (uint entity) where T : IGameComponent
        {
            Type t = typeof(T);
            if (!_gameComponents.ContainsKey(t))
                throw new Exception("Invalid component");

            if (!_gameComponents[t].ContainsKey(entity))
                throw new Exception("Entity has no such component");

            return (T)_gameComponents[t][entity];
        }

        public List<T> GetAllComponentsOfType<T> () where T : IGameComponent
        {
            if (!_gameComponents.ContainsKey(typeof(T)))
                throw new Exception("Invalid component");

            Dictionary<uint, IGameComponent> temp = _gameComponents[typeof(T)];
            List<T> output = new List<T>();
            foreach (var item in temp.Values)
            {
                output.Add((T)item);
            }

            return output;
        }


        public List<T> GetAllActiveComponentsOfType<T> () where T : IGameComponent
        {
            if (!_gameComponents.ContainsKey(typeof(T)))
                throw new Exception("Invalid component");

            Dictionary<uint, IGameComponent> temp = _gameComponents[typeof(T)];
            List<T> output = new List<T>();
            foreach (var item in _activeEntities)
            {
                if (temp.ContainsKey(item))
                {
                    output.Add((T)temp[item]);
                }
            }

            return output;
        }

        public bool EntityHasComponent<T>(uint id) where T : IGameComponent
        {
            if (typeof(T) == typeof(IScriptComponent))
            {
                return _scriptComponents.ContainsKey(id);
            }
            if (!_gameComponents.ContainsKey(typeof(T)))
                throw new Exception("Invalid component type");

            return _gameComponents[typeof(T)].ContainsKey(id);
        }

        public List<uint> GetAllActiveEntities()
        {
            return _activeEntities;
        }

        public List<IScriptComponent> GetEntityScriptComponents(uint entity)
        {
            return _scriptComponents[entity];
        }

        public List<List<IScriptComponent>> GetAllScriptComponents()
        {
            return new List<List<IScriptComponent>>(_scriptComponents.Values);
        }

        public List<List<IScriptComponent>> GetAllActiveScriptComponents()
        {
            List<List<IScriptComponent>> active = new List<List<IScriptComponent>>();
            _activeEntities.ForEach(x => { active.Add(_scriptComponents[x]); });
            return active;
        }
    }
}

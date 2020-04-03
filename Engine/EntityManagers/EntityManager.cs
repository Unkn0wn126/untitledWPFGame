using Engine.Coordinates;
using Engine.Models;
using Engine.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.EntityManagers
{
    public class EntityManager : IEntityManager
    {
        private uint _maxValue;
        private List<uint> _activeEntities;
        private List<uint> _entities;
        private Dictionary<uint, ITransformComponent> _transformComponents;
        private Dictionary<uint, IGraphicsComponent> _graphicsComponents;
        private Dictionary<uint, ICollisionComponent> _collisionComponents;
        private Dictionary<uint, ISoundComponent> _soundComponents;
        private ISpatialIndex _grid;

        public EntityManager(ISpatialIndex grid)
        {
            _grid = grid;
            _maxValue = uint.MinValue;
            _entities = new List<uint>();
            _activeEntities = new List<uint>();
            _transformComponents = new Dictionary<uint, ITransformComponent>();
            _graphicsComponents = new Dictionary<uint, IGraphicsComponent>();
            _collisionComponents = new Dictionary<uint, ICollisionComponent>();
            _soundComponents = new Dictionary<uint, ISoundComponent>();
        }

        public void UpdateActiveEntities(ITransformComponent focusPoint)
        {
            _activeEntities = _grid.GetObjectsInRadius(focusPoint, 3);
        }

        public void AddComponentToEntity(uint entityID, IGameComponent component)
        {
            switch (component)
            {
                case IGraphicsComponent g:
                    _graphicsComponents.Add(entityID, g);
                    break;
                case ITransformComponent t:
                    _transformComponents.Add(entityID, t);
                    break;
                case ICollisionComponent c:
                    _collisionComponents.Add(entityID, c);
                    break;
                case ISoundComponent s:
                    _soundComponents.Add(entityID, s);
                    break;
            }
        }

        public uint AddEntity()
        {
            uint temp = _maxValue;
            _entities.Add(_maxValue++);
            return temp;
        }

        private void RemoveEntityComponents(uint id)
        {
            if (_transformComponents.ContainsKey(id))
                _transformComponents.Remove(id);            
            
            if (_graphicsComponents.ContainsKey(id))
                _graphicsComponents.Remove(id);     
            
            if (_soundComponents.ContainsKey(id))
                _soundComponents.Remove(id);  
            
            if (_collisionComponents.ContainsKey(id))
                _collisionComponents.Remove(id);
        }

        public void RemoveEntity(uint id)
        {
            RemoveEntityComponents(id);

            _entities.Remove(id);
        }

        public List<uint> GetAllEntitiesPossessingComponent(Type componentType)
        {
            if (componentType is IGraphicsComponent)
                return new List<uint>(_graphicsComponents.Keys);

            if (componentType is ITransformComponent)
                return new List<uint>(_transformComponents.Keys);

            if (componentType is ICollisionComponent)
                return new List<uint>(_collisionComponents.Keys);

            if (componentType is ISoundComponent)
                return new List<uint>(_soundComponents.Keys);

            return new List<uint>();
        }

        public uint AddEntity(ITransformComponent transform)
        {
            uint temp = AddEntity();

            _grid.Add(temp, transform);
            AddComponentToEntity(temp, transform);
            return temp;
        }

        public ITransformComponent GetTransformComponent(uint entity)
        {
            return _transformComponents[entity];
        }

        public List<ITransformComponent> GetAllTransformComponents()
        {
            return new List<ITransformComponent>(_transformComponents.Values);
        }

        public List<ITransformComponent> GetAllActiveTransformComponents()
        {
            List<ITransformComponent> active = new List<ITransformComponent>();
            _activeEntities.ForEach(x => { if (_transformComponents.ContainsKey(x)) active.Add(_transformComponents[x]); });
            return active;
        }

        public IGraphicsComponent GetGraphicsComponent(uint entity)
        {
            return _graphicsComponents[entity];
        }

        public List<IGraphicsComponent> GetAllGraphicsComponents()
        {
            return new List<IGraphicsComponent>(_graphicsComponents.Values);
        }

        public List<IGraphicsComponent> GetAllActiveGraphicsComponents()
        {
            List<IGraphicsComponent> active = new List<IGraphicsComponent>();
            _activeEntities.ForEach(x => { if (_transformComponents.ContainsKey(x)) active.Add(_graphicsComponents[x]); });
            return active;
        }

        public ICollisionComponent GetCollisionComponent(uint entity)
        {
            return _collisionComponents[entity];
        }

        public List<ICollisionComponent> GetAllCollicionComponents()
        {
            return new List<ICollisionComponent>(_collisionComponents.Values);
        }

        public List<ICollisionComponent> GetAllActiveCollicionComponents()
        {
            List<ICollisionComponent> active = new List<ICollisionComponent>();
            _activeEntities.ForEach(x => { if (_transformComponents.ContainsKey(x)) active.Add(_collisionComponents[x]); });
            return active;
        }

        public ISoundComponent GetSoundComponents(uint entity)
        {
            return _soundComponents[entity];
        }

        public List<ISoundComponent> GetAllSoundComponents()
        {
            return new List<ISoundComponent>(_soundComponents.Values);
        }

        public List<ISoundComponent> GetAllActiveSoundComponents()
        {
            List<ISoundComponent> active = new List<ISoundComponent>();
            _activeEntities.ForEach(x => { if (_transformComponents.ContainsKey(x)) active.Add(_soundComponents[x]); });
            return active;
        }

        public bool EntityHasComponent(uint id, Type componentType)
        {
            if (componentType is IGraphicsComponent)
                return _graphicsComponents.ContainsKey(id);

            if (componentType is ITransformComponent)
                return _transformComponents.ContainsKey(id);

            if (componentType is ICollisionComponent)
                return _collisionComponents.ContainsKey(id);

            if (componentType is ISoundComponent)
                return _soundComponents.ContainsKey(id);

            return false;
        }
    }
}

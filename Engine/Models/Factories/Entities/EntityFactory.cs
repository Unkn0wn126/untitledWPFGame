using Engine.EntityManagers;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.Navmesh;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Sound;
using ResourceManagers.Images;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using TimeUtils;

namespace Engine.Models.Factories.Entities
{
    [Flags]
    public enum ComponentState
    {
        None = 0,
        TransformComponent = 1 << 0,
        CollisionComponent = 1 << 1,
        GraphicsComponent = 1 << 2,
        RigidBodyComponent = 1 << 3,
        SoundComponent = 1 << 4,
        NavMeshComponent = 1 << 5,
    }
    public static class EntityFactory
    {
        public static uint GenerateEntity(IEntityManager manager, ComponentState requiredComponents, ImgName imgName, CollisionType collisionType, Vector2 size, Vector2 pos, int zIndex)
        {
            uint entity = manager.AddEntity();
            if (IsComponentRequired(requiredComponents, ComponentState.TransformComponent))
            {
                manager.AddComponentToEntity<ITransformComponent>(entity, new TransformComponent(pos, size.X, size.Y, new Vector2(0, 0), zIndex));
            }
            if (IsComponentRequired(requiredComponents, ComponentState.CollisionComponent))
            {
                manager.AddComponentToEntity<ICollisionComponent>(entity, new CollisionComponent(IsCollisionType(collisionType, CollisionType.Solid), IsCollisionType(collisionType, CollisionType.Dynamic)));
            }            
            if (IsComponentRequired(requiredComponents, ComponentState.GraphicsComponent))
            {
                manager.AddComponentToEntity<IGraphicsComponent>(entity, new GraphicsComponent(imgName));
            }            
            if (IsComponentRequired(requiredComponents, ComponentState.RigidBodyComponent))
            {
                manager.AddComponentToEntity<IRigidBodyComponent>(entity, new RigidBodyComponent());
            }            
            if (IsComponentRequired(requiredComponents, ComponentState.SoundComponent))
            {
                manager.AddComponentToEntity<ISoundComponent>(entity, new SoundComponent());
            }            
            if (IsComponentRequired(requiredComponents, ComponentState.NavMeshComponent))
            {
                manager.AddComponentToEntity<INavmeshComponent>(entity, new NavmeshComponent());
            }           

            return entity;
        }

        private static bool IsCollisionType(CollisionType requiredValue, CollisionType askedValue)
        {
            return (requiredValue & askedValue) == askedValue;
        }

        private static bool IsComponentRequired(ComponentState requiredValue, ComponentState askedValue)
        {
            return (requiredValue & askedValue) == askedValue;
        }
    }
}

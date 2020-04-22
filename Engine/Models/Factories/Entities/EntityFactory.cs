using Engine.EntityManagers;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.Navmesh;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Sound;
using Engine.Models.Factories.Scenes;
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
        LifeComponent = 1 << 6
    }
    public static class EntityFactory
    {
        public static uint GenerateEntity(MetaMapEntity metaEntity, IEntityManager manager)
        {
            uint entity = manager.AddEntity();
            if (IsComponentRequired(metaEntity.Components, ComponentState.TransformComponent))
            {
                manager.AddComponentToEntity<ITransformComponent>(entity, new TransformComponent(new Vector2(metaEntity.PosX, metaEntity.PosY), metaEntity.SizeX, metaEntity.SizeY, new Vector2(0, 0), metaEntity.ZIndex));
            }
            if (IsComponentRequired(metaEntity.Components, ComponentState.CollisionComponent))
            {
                manager.AddComponentToEntity<ICollisionComponent>(entity, new CollisionComponent(IsCollisionType(metaEntity.CollisionType, CollisionType.Solid), IsCollisionType(metaEntity.CollisionType, CollisionType.Dynamic)));
            }            
            if (IsComponentRequired(metaEntity.Components, ComponentState.GraphicsComponent))
            {
                manager.AddComponentToEntity<IGraphicsComponent>(entity, new GraphicsComponent(metaEntity.Graphics));
            }            
            if (IsComponentRequired(metaEntity.Components, ComponentState.RigidBodyComponent))
            {
                manager.AddComponentToEntity<IRigidBodyComponent>(entity, new RigidBodyComponent());
            }            
            if (IsComponentRequired(metaEntity.Components, ComponentState.SoundComponent))
            {
                manager.AddComponentToEntity<ISoundComponent>(entity, new SoundComponent());
            }            
            if (IsComponentRequired(metaEntity.Components, ComponentState.NavMeshComponent))
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

using Engine.EntityManagers;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.RigidBody;
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
        SolidCollision = 1 << 6,
        DynamicCollision = 1 << 7
    }
    public static class EntityFactory
    {
        public static uint GenerateEntity(IEntityManager manager, ComponentState requiredComponents, ImgName imgName, Vector2 size, Vector2 pos, int zIndex)
        {
            uint entity = manager.AddEntity();
            if (IsComponentRequired(requiredComponents, ComponentState.TransformComponent))
            {
                manager.AddComponentToEntity(entity, new TransformComponent(pos, size.X, size.Y, new Vector2(0, 0), zIndex));
            }
            if (IsComponentRequired(requiredComponents, ComponentState.CollisionComponent))
            {
                manager.AddComponentToEntity(entity, new CollisionComponent(IsComponentRequired(requiredComponents, ComponentState.SolidCollision), IsComponentRequired(requiredComponents, ComponentState.DynamicCollision)));
            }            
            if (IsComponentRequired(requiredComponents, ComponentState.GraphicsComponent))
            {
                manager.AddComponentToEntity(entity, new GraphicsComponent(imgName));
            }            
            if (IsComponentRequired(requiredComponents, ComponentState.RigidBodyComponent))
            {
                manager.AddComponentToEntity(entity, new RigidBodyComponent());
            }            
            if (IsComponentRequired(requiredComponents, ComponentState.SoundComponent))
            {

            }            
            if (IsComponentRequired(requiredComponents, ComponentState.NavMeshComponent))
            {

            }           

            return entity;
        }

        private static bool IsComponentRequired(ComponentState requiredValue, ComponentState askedValue)
        {
            return (requiredValue & askedValue) == askedValue;
        }
    }
}

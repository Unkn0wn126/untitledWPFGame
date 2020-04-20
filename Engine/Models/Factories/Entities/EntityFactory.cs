using Engine.EntityManagers;
using System;
using System.Collections.Generic;
using System.Text;

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
        LifeComponent = 1 << 6,
        ScriptComponent = 1 << 7
    }
    public class EntityFactory
    {
        public static uint GenerateEntity(IEntityManager manager, ComponentState requiredComponents)
        {
            uint entity = manager.AddEntity();
            if (IsComponentRequired(requiredComponents, ComponentState.TransformComponent))
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

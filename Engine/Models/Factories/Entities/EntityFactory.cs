using Engine.EntityManagers;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.Life;
using Engine.Models.Components.Navmesh;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script;
using Engine.Models.Components.Sound;
using Engine.Models.Factories.Scenes;
using Engine.Models.Scenes;
using GameInputHandler;
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
        public static uint GenerateEntity(MetaEntity metaEntity, IScene scene, IEntityManager manager, GameTime gameTime, GameInput gameInput)
        {
            uint entity = manager.AddEntity();
            DetermineComponents(entity, metaEntity, manager);
            DetermineScripts(entity, metaEntity, scene, manager, gameTime, gameInput);
            return entity;
        }

        private static void DetermineScripts(uint entity, MetaEntity metaEntity, IScene scene, IEntityManager manager, GameTime gameTime, GameInput gameInput)
        {
            if (IsScriptRequired(metaEntity.Scripts, ScriptType.AiMovement))
            {
                // TODO: get the speed from LifeComponent
                manager.AddComponentToEntity<IScriptComponent>(entity, new AiMovementScript(gameTime, scene, entity, 1 * scene.BaseObjectSize));
            }
            if (IsScriptRequired(metaEntity.Scripts, ScriptType.PlayerMovement))
            {
                manager.AddComponentToEntity<IScriptComponent>(entity, new PlayerMovementScript(gameTime, gameInput, scene, entity, 2 * scene.BaseObjectSize));
            }
        }

        private static void DetermineComponents(uint entity, MetaEntity metaEntity, IEntityManager manager)
        {
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
                manager.AddComponentToEntity<INavmeshComponent>(entity, 
                    new NavmeshComponent 
                    { 
                        LeadsUp = NavmeshLeadsInDirection(metaEntity.NavmeshContinuation, NavmeshContinues.Up),
                        LeadsDown = NavmeshLeadsInDirection(metaEntity.NavmeshContinuation, NavmeshContinues.Down),
                        LeadsLeft = NavmeshLeadsInDirection(metaEntity.NavmeshContinuation, NavmeshContinues.Left),
                        LeadsRight = NavmeshLeadsInDirection(metaEntity.NavmeshContinuation, NavmeshContinues.Right),
                    });
            }
            if (IsComponentRequired(metaEntity.Components, ComponentState.LifeComponent))
            {
                manager.AddComponentToEntity(entity, metaEntity.LifeComponent);
            }
        }

        private static bool NavmeshLeadsInDirection(NavmeshContinues input, NavmeshContinues searched)
        {
            return (input & searched) == searched;
        }

        public static MetaEntity GenerateMetaEntityFromEntity(IEntityManager manager, uint item)
        {
            ComponentState required = 0;
            MetaEntity currentEntity = new MetaEntity();
            if (manager.EntityHasComponent<ITransformComponent>(item))
            {
                required |= ComponentState.TransformComponent;
                ITransformComponent currTransform = manager.GetComponentOfType<ITransformComponent>(item);
                currentEntity.PosX = currTransform.Position.X;
                currentEntity.PosY = currTransform.Position.Y;
                currentEntity.SizeX = currTransform.ScaleX;
                currentEntity.SizeY = currTransform.ScaleY;
                currentEntity.ZIndex = currTransform.ZIndex;
            }
            if (manager.EntityHasComponent<ILifeComponent>(item))
            {
                required |= ComponentState.LifeComponent;
                currentEntity.LifeComponent = manager.GetComponentOfType<ILifeComponent>(item);
            }
            if (manager.EntityHasComponent<IGraphicsComponent>(item))
            {
                required |= ComponentState.GraphicsComponent;
                currentEntity.Graphics = manager.GetComponentOfType<IGraphicsComponent>(item).CurrentImageName;
            }
            if (manager.EntityHasComponent<ICollisionComponent>(item))
            {
                required |= ComponentState.CollisionComponent;
                ICollisionComponent currCollision = manager.GetComponentOfType<ICollisionComponent>(item);
                currentEntity.CollisionType = 0;
                if (currCollision.IsDynamic)
                {
                    currentEntity.CollisionType |= CollisionType.Dynamic;
                }
                if (currCollision.IsSolid)
                {
                    currentEntity.CollisionType |= CollisionType.Solid;

                }
            }
            if (manager.EntityHasComponent<IRigidBodyComponent>(item))
            {
                required |= ComponentState.RigidBodyComponent;
            }
            if (manager.EntityHasComponent<IScriptComponent>(item))
            {
                var scripts = manager.GetEntityScriptComponents(item);
                currentEntity.Scripts = 0;
                foreach (var script in scripts)
                {
                    if (script.GetType() == typeof(AiMovementScript))
                    {
                        currentEntity.Scripts |= ScriptType.AiMovement;
                    }
                    if (script.GetType() == typeof(PlayerMovementScript))
                    {
                        currentEntity.Scripts |= ScriptType.PlayerMovement;
                    }
                }
            }

            currentEntity.Components = required;

            return currentEntity;
        }

        private static bool IsCollisionType(CollisionType requiredValue, CollisionType askedValue)
        {
            return (requiredValue & askedValue) == askedValue;
        }

        private static bool IsComponentRequired(ComponentState requiredValue, ComponentState askedValue)
        {
            return (requiredValue & askedValue) == askedValue;
        }

        private static bool IsScriptRequired(ScriptType requiredValue, ScriptType askedValue)
        {
            return (requiredValue & askedValue) == askedValue;
        }
    }
}

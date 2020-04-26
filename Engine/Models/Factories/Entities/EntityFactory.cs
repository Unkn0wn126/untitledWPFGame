using Engine.EntityManagers;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.Life;
using Engine.Models.Components.Navmesh;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script;
using Engine.Models.Components.Sound;
using Engine.Models.Scenes;
using GameInputHandler;
using System;
using System.Numerics;
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
        /// <summary>
        /// Generates new entity based
        /// on the provided meta entity
        /// and inserts it into the
        /// scene entity manager
        /// </summary>
        /// <param name="metaEntity"></param>
        /// <param name="scene"></param>
        /// <param name="manager"></param>
        /// <param name="gameTime"></param>
        /// <param name="gameInput"></param>
        /// <returns></returns>
        public static uint GenerateEntity(MetaEntity metaEntity, IScene scene, IEntityManager manager, GameTime gameTime, GameInput gameInput)
        {
            uint entity = manager.AddEntity();
            DetermineComponents(entity, metaEntity, manager);
            DetermineScripts(entity, metaEntity, scene, manager, gameTime, gameInput);
            return entity;
        }

        /// <summary>
        /// Determines what type of scripts
        /// will the entity need
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="metaEntity"></param>
        /// <param name="scene"></param>
        /// <param name="manager"></param>
        /// <param name="gameTime"></param>
        /// <param name="gameInput"></param>
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

        /// <summary>
        /// Determines what type of components
        /// will the entity need
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="metaEntity"></param>
        /// <param name="manager"></param>
        private static void DetermineComponents(uint entity, MetaEntity metaEntity, IEntityManager manager)
        {
            SetUpTransform(entity, metaEntity, manager);
            SetUpCollision(entity, metaEntity, manager);
            SetUpGraphics(entity, metaEntity, manager);
            SetUpRigidBody(entity, metaEntity, manager);
            SetUpSound(entity, metaEntity, manager);
            SetUpNavmesh(entity, metaEntity, manager);
            SetUpLife(entity, metaEntity, manager);
        }

        /// <summary>
        /// Based on the value from
        /// the meta entity determines
        /// if the entity should have
        /// a transform component
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="metaEntity"></param>
        /// <param name="manager"></param>
        private static void SetUpTransform(uint entity, MetaEntity metaEntity, IEntityManager manager)
        {
            if (IsComponentRequired(metaEntity.Components, ComponentState.TransformComponent))
            {
                manager.AddComponentToEntity<ITransformComponent>(entity,
                    new TransformComponent(
                        new Vector2(metaEntity.PosX, metaEntity.PosY),
                        metaEntity.SizeX, metaEntity.SizeY,
                        new Vector2(0, 0), metaEntity.ZIndex
                        ));
            }
        }

        /// <summary>
        /// Based on the value from
        /// the meta entity determines
        /// if the entity should have
        /// a collision component
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="metaEntity"></param>
        /// <param name="manager"></param>
        private static void SetUpCollision(uint entity, MetaEntity metaEntity, IEntityManager manager)
        {
            if (IsComponentRequired(metaEntity.Components, ComponentState.CollisionComponent))
            {
                manager.AddComponentToEntity<ICollisionComponent>(entity,
                    new CollisionComponent(
                        IsCollisionType(metaEntity.CollisionType, CollisionType.Solid),
                        IsCollisionType(metaEntity.CollisionType, CollisionType.Dynamic)
                        ));
            }
        }

        /// <summary>
        /// Based on the value from
        /// the meta entity determines
        /// if the entity should have
        /// a graphics component
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="metaEntity"></param>
        /// <param name="manager"></param>
        private static void SetUpGraphics(uint entity, MetaEntity metaEntity, IEntityManager manager)
        {
            if (IsComponentRequired(metaEntity.Components, ComponentState.GraphicsComponent))
            {
                manager.AddComponentToEntity<IGraphicsComponent>(entity,
                    new GraphicsComponent(metaEntity.Graphics));
            }
        }

        /// <summary>
        /// Based on the value from
        /// the meta entity determines
        /// if the entity should have
        /// a rigid body component
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="metaEntity"></param>
        /// <param name="manager"></param>
        private static void SetUpRigidBody(uint entity, MetaEntity metaEntity, IEntityManager manager)
        {
            if (IsComponentRequired(metaEntity.Components, ComponentState.RigidBodyComponent))
            {
                manager.AddComponentToEntity<IRigidBodyComponent>(entity, new RigidBodyComponent());
            }
        }

        /// <summary>
        /// Based on the value from
        /// the meta entity determines
        /// if the entity should have
        /// a sound component
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="metaEntity"></param>
        /// <param name="manager"></param>
        private static void SetUpSound(uint entity, MetaEntity metaEntity, IEntityManager manager)
        {
            if (IsComponentRequired(metaEntity.Components, ComponentState.SoundComponent))
            {
                manager.AddComponentToEntity<ISoundComponent>(entity, new SoundComponent());
            }
        }

        /// <summary>
        /// Based on the value from
        /// the meta entity determines
        /// if the entity should have
        /// a navmesh component
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="metaEntity"></param>
        /// <param name="manager"></param>
        private static void SetUpNavmesh(uint entity, MetaEntity metaEntity, IEntityManager manager)
        {
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
        }

        /// <summary>
        /// Based on the value from
        /// the meta entity determines
        /// if the entity should have
        /// a life component
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="metaEntity"></param>
        /// <param name="manager"></param>
        private static void SetUpLife(uint entity, MetaEntity metaEntity, IEntityManager manager)
        {
            if (IsComponentRequired(metaEntity.Components, ComponentState.LifeComponent))
            {
                manager.AddComponentToEntity(entity, metaEntity.LifeComponent);
            }
        }

        /// <summary>
        /// Determines if the navmesh
        /// continues in a given direction
        /// </summary>
        /// <param name="input"></param>
        /// <param name="searched"></param>
        /// <returns></returns>
        private static bool NavmeshLeadsInDirection(NavmeshContinues input, NavmeshContinues searched)
        {
            return (input & searched) == searched;
        }

        /// <summary>
        /// Generates a meta entity
        /// from an entity
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static MetaEntity GenerateMetaEntityFromEntity(IEntityManager manager, uint item)
        {
            MetaEntity currentEntity = new MetaEntity();

            SetUpMetaTransform(manager, item, currentEntity);
            SetUpMetaLife(manager, item, currentEntity);
            SetUpMetaGraphics(manager, item, currentEntity);
            SetUpMetaCollision(manager, item, currentEntity);
            SetUpMetaRigidBody(manager, item, currentEntity);
            SetUpMetaSound(manager, item, currentEntity);
            SetUpMetaNavmesh(manager, item, currentEntity);
            SetUpMetaScripts(manager, item, currentEntity);

            return currentEntity;
        }

        /// <summary>
        /// Determines if the entity
        /// has transform component
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="item"></param>
        /// <param name="currentEntity"></param>
        private static void SetUpMetaTransform(IEntityManager manager, uint item, MetaEntity currentEntity)
        {
            if (manager.EntityHasComponent<ITransformComponent>(item))
            {
                currentEntity.Components |= ComponentState.TransformComponent;
                ITransformComponent currTransform = manager.GetComponentOfType<ITransformComponent>(item);
                currentEntity.PosX = currTransform.Position.X;
                currentEntity.PosY = currTransform.Position.Y;
                currentEntity.SizeX = currTransform.ScaleX;
                currentEntity.SizeY = currTransform.ScaleY;
                currentEntity.ZIndex = currTransform.ZIndex;
            }
        }

        /// <summary>
        /// Determines if the entity
        /// has sound component
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="item"></param>
        /// <param name="currentEntity"></param>
        private static void SetUpMetaSound(IEntityManager manager, uint item, MetaEntity currentEntity)
        {
            if (manager.EntityHasComponent<ISoundComponent>(item))
            {
                currentEntity.Components |= ComponentState.SoundComponent;
            }
        }

        /// <summary>
        /// Determines if the entity
        /// has navmesh component
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="item"></param>
        /// <param name="currentEntity"></param>
        private static void SetUpMetaNavmesh(IEntityManager manager, uint item, MetaEntity currentEntity)
        {
            if (manager.EntityHasComponent<INavmeshComponent>(item))
            {
                currentEntity.Components |= ComponentState.NavMeshComponent;
                INavmeshComponent navmeshComponent = manager.GetComponentOfType<INavmeshComponent>(item);
                if (navmeshComponent.LeadsDown)
                    currentEntity.NavmeshContinuation |= NavmeshContinues.Down;

                if (navmeshComponent.LeadsUp)
                    currentEntity.NavmeshContinuation |= NavmeshContinues.Up;

                if (navmeshComponent.LeadsLeft)
                    currentEntity.NavmeshContinuation |= NavmeshContinues.Left;

                if (navmeshComponent.LeadsRight)
                    currentEntity.NavmeshContinuation |= NavmeshContinues.Right;
            }
        }

        /// <summary>
        /// Determines if the entity
        /// has life component
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="item"></param>
        /// <param name="currentEntity"></param>
        private static void SetUpMetaLife(IEntityManager manager, uint item, MetaEntity currentEntity)
        {
            if (manager.EntityHasComponent<ILifeComponent>(item))
            {
                currentEntity.Components |= ComponentState.LifeComponent;
                currentEntity.LifeComponent = manager.GetComponentOfType<ILifeComponent>(item);
            }
        }

        /// <summary>
        /// Determines if the entity
        /// has graphics component
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="item"></param>
        /// <param name="currentEntity"></param>
        private static void SetUpMetaGraphics(IEntityManager manager, uint item, MetaEntity currentEntity)
        {
            if (manager.EntityHasComponent<IGraphicsComponent>(item))
            {
                currentEntity.Components |= ComponentState.GraphicsComponent;
                currentEntity.Graphics = manager.GetComponentOfType<IGraphicsComponent>(item).CurrentImageName;
            }
        }

        /// <summary>
        /// Determines if the entity
        /// has collision component
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="item"></param>
        /// <param name="currentEntity"></param>
        private static void SetUpMetaCollision(IEntityManager manager, uint item, MetaEntity currentEntity)
        {
            if (manager.EntityHasComponent<ICollisionComponent>(item))
            {
                currentEntity.Components |= ComponentState.CollisionComponent;
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
        }

        /// <summary>
        /// Determines if the entity
        /// has rigid body component
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="item"></param>
        /// <param name="currentEntity"></param>
        private static void SetUpMetaRigidBody(IEntityManager manager, uint item, MetaEntity currentEntity)
        {
            if (manager.EntityHasComponent<IRigidBodyComponent>(item))
            {
                currentEntity.Components |= ComponentState.RigidBodyComponent;
            }
        }

        /// <summary>
        /// Determines if the entity
        /// has script components
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="item"></param>
        /// <param name="currentEntity"></param>
        private static void SetUpMetaScripts(IEntityManager manager, uint item, MetaEntity currentEntity)
        {
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
        }

        /// <summary>
        /// Checks the value of the flags
        /// to determine if given type
        /// of collision is required
        /// </summary>
        /// <param name="requiredValue"></param>
        /// <param name="askedValue"></param>
        /// <returns></returns>
        private static bool IsCollisionType(CollisionType requiredValue, CollisionType askedValue)
        {
            return (requiredValue & askedValue) == askedValue;
        }

        /// <summary>
        /// Checks the value of the flags
        /// to determine if given type
        /// of component is required
        /// </summary>
        /// <param name="requiredValue"></param>
        /// <param name="askedValue"></param>
        /// <returns></returns>
        private static bool IsComponentRequired(ComponentState requiredValue, ComponentState askedValue)
        {
            return (requiredValue & askedValue) == askedValue;
        }

        /// <summary>
        /// Checks the value of the flags
        /// to determine if given type
        /// of script is required
        /// </summary>
        /// <param name="requiredValue"></param>
        /// <param name="askedValue"></param>
        /// <returns></returns>
        private static bool IsScriptRequired(ScriptType requiredValue, ScriptType askedValue)
        {
            return (requiredValue & askedValue) == askedValue;
        }
    }
}

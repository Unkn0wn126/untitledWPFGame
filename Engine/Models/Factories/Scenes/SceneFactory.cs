using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.Life;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script;
using Engine.Models.Factories.Entities;
using Engine.Models.Factories.Scenes;
using Engine.Models.Scenes;
using GameInputHandler;
using ResourceManagers.Images;
using System;
using System.Collections.Generic;
using System.Numerics;
using TimeUtils;

namespace Engine.Models.Factories
{
    public static class SceneFactory
    {
        private static Random _rnd = new Random();
        public static IScene CreateBattleScene(float xRes, float yRes, GameTime gameTime, GameInput gameInputHandler)
        {
            int objectSize = 5;
            int numOfObjectsInCell = 3;
            int cellSize = objectSize * numOfObjectsInCell;
            float baseCellXValue = (2 * objectSize) / (float)cellSize;
            float baseCellYValue = (2 * objectSize) / (float)cellSize;
            int numCellsX = (int)Math.Ceiling(baseCellXValue);
            int numCellsY = (int)Math.Ceiling(baseCellYValue);

            ISpatialIndex grid = new Grid(numCellsX, numCellsY, cellSize);

            IEntityManager manager = new EntityManager(grid);

            ITransformComponent currTransform = new TransformComponent(new Vector2(1 * objectSize, 1 * objectSize), objectSize, objectSize, new Vector2(0, 0), 0);
            IGraphicsComponent current = new GraphicsComponent(ImgName.Player);

            uint currEntity = manager.AddEntity(currTransform);
            manager.AddComponentToEntity<IGraphicsComponent>(currEntity, current);            
            
            ITransformComponent currTransform2 = new TransformComponent(new Vector2(2 * objectSize, 1 * objectSize), objectSize, objectSize, new Vector2(0, 0), 1);
            IGraphicsComponent current2 = new GraphicsComponent(ImgName.Enemy);

            uint currEntity2 = manager.AddEntity(currTransform2);
            manager.AddComponentToEntity<IGraphicsComponent>(currEntity2, current2);

            IScene scene = new GeneralScene(new Camera(xRes, yRes), manager, grid);

            scene.PlayerEntity = currEntity;

            return scene;
        }

        private static List<MetaMapEntity> GenerateGround(int numOfObjectsOnX, int numOfObjectsOnY, int baseObjectSize)
        {
            MetaMapEntity[,] metaMap = new MetaMapEntity[numOfObjectsOnX, numOfObjectsOnY];
            ComponentState current;
            for (int i = 0; i < metaMap.GetLength(0); i++)
            {
                for (int j = 0; j < metaMap.GetLength(1); j++)
                {
                    ImgName currImg = (ImgName)_rnd.Next(1, 4);
                    current = ComponentState.GraphicsComponent | ComponentState.TransformComponent;
                    metaMap[i, j] = new MetaMapEntity { CollisionType = CollisionType.None, Graphics = currImg, Components = current, ZIndex = 0, PosX = i * baseObjectSize, PosY = j * baseObjectSize, SizeX = baseObjectSize, SizeY = baseObjectSize };
                }
            }

            List<MetaMapEntity> output = new List<MetaMapEntity>();
            foreach (var item in metaMap)
            {
                if (item != null)
                {
                    output.Add(item);
                }

            }

            return output;
        }

        private static List<MetaMapEntity> GenerateStaticBlocks(int numOfObjectsOnX, int numOfObjectsOnY, int baseObjectSize)
        {
            MetaMapEntity[,] metaMap = new MetaMapEntity[numOfObjectsOnX, numOfObjectsOnY];
            ComponentState current;
            for (int i = 0; i < metaMap.GetLength(0); i++)
            {
                for (int j = 0; j < metaMap.GetLength(1); j++)
                {
                    if ((i == 0 || i == metaMap.GetLength(0) - 1) || (j == 0 || j == metaMap.GetLength(1) - 1))  // generate edges of the map
                    {
                        current = ComponentState.GraphicsComponent | ComponentState.TransformComponent | ComponentState.CollisionComponent;
                        metaMap[i, j] = new MetaMapEntity { CollisionType = CollisionType.Solid, Graphics = ImgName.Rock, Components = current, ZIndex = 1, PosX = i * baseObjectSize, PosY = j * baseObjectSize, SizeX = baseObjectSize, SizeY = baseObjectSize };
                    }
                }
            }

            List<MetaMapEntity> output = new List<MetaMapEntity>();
            foreach (var item in metaMap)
            {
                if (item != null)
                {
                    output.Add(item);
                }
                
            }

            return output;
        }

        public static MetaScene CreateMetaScene(ILifeComponent lifeComponent, int numOfObjectsOnX, int numOfObjectsOnY, int baseObjectSize, int numOfObjectsInCell)
        {
            MetaScene metaScene = new MetaScene(SceneType.General);
            metaScene.NumOfEntitiesOnX = numOfObjectsOnX;
            metaScene.NumOfEntitiesOnY = numOfObjectsOnY;
            metaScene.BaseObjectSize = baseObjectSize;
            metaScene.NumOfObjectsInCell = numOfObjectsInCell;
            metaScene.GroundEntities = GenerateGround(numOfObjectsOnX, numOfObjectsOnY, baseObjectSize);
            metaScene.StaticCollisionEntities = GenerateStaticBlocks(numOfObjectsOnX, numOfObjectsOnY, baseObjectSize);

            int numOfEnemies = numOfObjectsOnX / 2/*(int)((numOfObjectsOnX / 4f) * (numOfObjectsOnY / 4f))*/;

            for (int i = 0; i < numOfEnemies; i++)
            {
                int x = _rnd.Next(3, numOfObjectsOnX - 3);
                int y = _rnd.Next(3, numOfObjectsOnY - 3);

                metaScene.DynamicEntities.Add(GenerateDynamicEntities(numOfObjectsOnX, numOfObjectsOnY, baseObjectSize, x, y));
            }

            metaScene.DynamicEntities.Add(GenerateMetaPlayer(lifeComponent, baseObjectSize, 1, 1));

            return metaScene;
        }

        private static MetaMapEntity GenerateDynamicEntities(int numOfObjectsOnX, int numOfObjectsOnY, int baseObjectSize, int xPos, int yPos)
        {
            MetaMapEntity metaMapEntity = new MetaMapEntity();
            ComponentState current = ComponentState.GraphicsComponent | ComponentState.TransformComponent | ComponentState.CollisionComponent | ComponentState.RigidBodyComponent | ComponentState.LifeComponent;
            metaMapEntity = new MetaMapEntity { CollisionType = CollisionType.Solid | CollisionType.Dynamic, Graphics = ImgName.Enemy, Components = current, ZIndex = 3, PosX = xPos, PosY = yPos, SizeX = baseObjectSize, SizeY = baseObjectSize };
            metaMapEntity.Scripts = ScriptType.AiMovement;
            metaMapEntity.LifeComponent = new LifeComponent { Agility = 10, AttributePoints = 0, BattleClass = BattleClass.Swordsman, CurrentLevel = 1, CurrentXP = 0, Gender = Gender.Male, MaxHP = 100, HP = 100, Intelligence = 10, IsPlayer = false, MaxMP = 100, MaxStamina = 100, MP = 100, Name = "Prak", NextLevelXP = 100, Race = Race.Human, Stamina = 100, Strength = 10 };
            return metaMapEntity;
        }

        private static MetaMapEntity GenerateMetaPlayer(ILifeComponent lifeComponent, int baseObjectSize, int xPos, int yPos)
        {
            MetaMapEntity metaMapEntity = new MetaMapEntity();
            ComponentState current = ComponentState.GraphicsComponent | ComponentState.TransformComponent | ComponentState.CollisionComponent | ComponentState.RigidBodyComponent | ComponentState.LifeComponent;
            metaMapEntity = new MetaMapEntity { CollisionType = CollisionType.Solid | CollisionType.Dynamic, Graphics = ImgName.Player, Components = current, ZIndex = 2, PosX = xPos, PosY = yPos, SizeX = baseObjectSize, SizeY = baseObjectSize };
            metaMapEntity.Scripts = ScriptType.PlayerMovement;
            metaMapEntity.LifeComponent = lifeComponent == null ? new LifeComponent { IsPlayer = true} : lifeComponent;
            return metaMapEntity;
        }

        public static MetaScene GenerateMetaSceneFromScene(IScene scene)
        {
            MetaScene metaScene = new MetaScene(SceneType.General);
            IEntityManager manager = scene.EntityManager;
            foreach (var item in manager.GetAllEntities())
            {
                MetaMapEntity currentEntity = EntityFactory.GenerateMetaEntityFromEntity(manager, item);

                DetermineEntityType(metaScene, currentEntity);
            }

            metaScene.BaseObjectSize = 1;
            metaScene.NumOfEntitiesOnX = scene.NumOfEntitiesOnX;
            metaScene.NumOfEntitiesOnY = scene.NumOfEntitiesOnY;
            metaScene.NumOfObjectsInCell = scene.NumOfObjectsInCell;

            return metaScene;
        }

        private static void DetermineEntityType(MetaScene metaScene, MetaMapEntity currentEntity)
        {
            if (IsGroundEntity(currentEntity))
            {
                metaScene.GroundEntities.Add(currentEntity);
            }
            else if (IsStaticCollisionEntity(currentEntity))
            {
                metaScene.StaticCollisionEntities.Add(currentEntity);
            }
            else
            {
                metaScene.DynamicEntities.Add(currentEntity);
            }
        }
        private static bool IsCollisionType(CollisionType requiredValue, CollisionType askedValue)
        {
            return (requiredValue & askedValue) == askedValue;
        }

        private static bool IsComponentRequired(ComponentState requiredValue, ComponentState askedValue)
        {
            return (requiredValue & askedValue) == askedValue;
        }

        private static bool IsGroundEntity(MetaMapEntity currentEntity)
        {
            return !IsComponentRequired(currentEntity.Components, ComponentState.CollisionComponent);
        }

        private static bool IsStaticCollisionEntity(MetaMapEntity currentEntity)
        {
            return IsCollisionType(currentEntity.CollisionType, CollisionType.Solid) && !IsCollisionType(currentEntity.CollisionType, CollisionType.Dynamic) && currentEntity.LifeComponent == null;
        }

        public static IScene GenerateSceneFromMeta(MetaScene metaScene, ICamera camera, GameInput gameInput, GameTime gameTime)
        {
            ICamera oldCamera = camera;
            int cellSize = metaScene.BaseObjectSize * metaScene.NumOfObjectsInCell;
            ISpatialIndex grid = new Grid(metaScene.NumOfEntitiesOnX, metaScene.NumOfEntitiesOnY, cellSize);
            IScene scene = new GeneralScene(new Camera(oldCamera.Width, oldCamera.Height), new EntityManager(grid), grid);
            scene.NumOfEntitiesOnX = metaScene.NumOfEntitiesOnX;
            scene.NumOfEntitiesOnY = metaScene.NumOfEntitiesOnY;
            scene.BaseObjectSize = metaScene.BaseObjectSize;
            scene.NumOfObjectsInCell = metaScene.NumOfObjectsInCell;
            foreach (var item in metaScene.GroundEntities)
            {
                EntityFactory.GenerateEntity(item, scene, scene.EntityManager, gameTime, gameInput);
            }
            foreach (var item in metaScene.StaticCollisionEntities)
            {
                if (item != null)
                {
                    EntityFactory.GenerateEntity(item, scene, scene.EntityManager, gameTime, gameInput);
                }
            }
            foreach (var item in metaScene.DynamicEntities)
            {
                if (item != null)
                {
                    uint curr = EntityFactory.GenerateEntity(item, scene, scene.EntityManager, gameTime, gameInput);
                    if (item.LifeComponent != null && item.LifeComponent.IsPlayer)
                    {
                        scene.PlayerEntity = curr;
                    }
                }
            }

            return scene;
        }

        // replace this with meta entity generation so every meta entity has the information about player and this is no longer needed...
        private static void GeneratePlayer(IEntityManager manager, IScene scene, int objectSize, GameTime gameTime, GameInput gameInputHandler)
        {
            ITransformComponent playerTransform = new TransformComponent(new Vector2(objectSize, objectSize), objectSize, objectSize, new Vector2(0, 0), 2);
            IGraphicsComponent test = new GraphicsComponent(ImgName.Player);
            //_playerTransform = playerTransform;

            uint player = manager.AddEntity(playerTransform);
            //_player = player;
            manager.AddComponentToEntity<IGraphicsComponent>(player, test);

            ICollisionComponent collision = new CollisionComponent(true, true);
            manager.AddComponentToEntity<ICollisionComponent>(player, collision);

            IRigidBodyComponent rigidBody = new RigidBodyComponent();
            manager.AddComponentToEntity<IRigidBodyComponent>(player, rigidBody);

            scene.PlayerEntity = player;

            manager.AddComponentToEntity<IScriptComponent>(player, new PlayerMovementScript(gameTime, gameInputHandler, scene, player, 4 * objectSize));

            manager.AddComponentToEntity<ILifeComponent>(player, new LifeComponent { IsPlayer = true });
        }
    }
}

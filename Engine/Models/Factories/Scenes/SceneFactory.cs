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
using System.Text;
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
            scene.PlayerTransform = currTransform;

            return scene;
        }

        private static MetaMapEntity[,] GenerateMetaMap(int numOfObjectsOnX, int numOfObjectsOnY)
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
                        metaMap[i, j] = new MetaMapEntity { CollisionType = CollisionType.Solid, Graphics = ImgName.Rock, Components = current, ZIndex = 1};
                    }
                    else
                    {
                        ImgName currImg = (ImgName)_rnd.Next(1, 4);
                        current = ComponentState.GraphicsComponent | ComponentState.TransformComponent;
                        metaMap[i, j] = new MetaMapEntity { CollisionType = CollisionType.None, Graphics = currImg, Components = current, ZIndex = 0 };
                    }

                    
                }
            }

            return metaMap;
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

        public static MetaScene CreateMetaScene(int numOfObjectsOnX, int numOfObjectsOnY, int baseObjectSize, int numOfObjectsInCell)
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
                int x = _rnd.Next(10, numOfObjectsOnX - 10);
                int y = _rnd.Next(10, numOfObjectsOnY - 10);

                metaScene.DynamicEntities.Add(GenerateDynamicEntities(numOfObjectsOnX, numOfObjectsOnY, baseObjectSize, x, y));
            }

            return metaScene;
        }

        private static MetaMapEntity GenerateDynamicEntities(int numOfObjectsOnX, int numOfObjectsOnY, int baseObjectSize, int xPos, int yPos)
        {
            MetaMapEntity metaMapEntity = new MetaMapEntity();
            ComponentState current = ComponentState.GraphicsComponent | ComponentState.TransformComponent | ComponentState.CollisionComponent | ComponentState.RigidBodyComponent;
            metaMapEntity = new MetaMapEntity { CollisionType = CollisionType.Solid | CollisionType.Dynamic, Graphics = ImgName.Enemy, Components = current, ZIndex = 3, PosX = xPos, PosY = yPos, SizeX = baseObjectSize, SizeY = baseObjectSize };
            metaMapEntity.Scripts = ScriptType.AiMovement;
            return metaMapEntity;
        }

        public static IScene CreateScene(float xRes, float yRes, GameTime gameTime, GameInput gameInputHandler, bool generateTheGuy, int numOfObjectsOnX, int numOfObjectsOnY)
        {
            int objectSize = 1;
            int numOfObjectsInCell = 4;
            int cellSize = objectSize * numOfObjectsInCell;
            float baseCellXValue = (numOfObjectsOnX * objectSize) / (float)cellSize;
            float baseCellYValue = (numOfObjectsOnY * objectSize) / (float)cellSize;
            int numCellsX = (int)Math.Ceiling(baseCellXValue);
            int numCellsY = (int)Math.Ceiling(baseCellYValue);

            ISpatialIndex grid = new Grid(numCellsX, numCellsY, cellSize);

            IEntityManager manager = new EntityManager(grid);

            IScene scene = new GeneralScene(new Camera(xRes, yRes), manager, grid);

            MetaMapEntity[,] metaMap = GenerateMetaMap(numOfObjectsOnX, numOfObjectsOnY);

            for (int i = 0; i < metaMap.GetLength(0); i++)
            {
                for (int j = 0; j < metaMap.GetLength(1); j++)
                {
                    EntityFactory.GenerateEntity(metaMap[i, j], manager);
                }
            }

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
            scene.PlayerTransform = playerTransform;

            manager.AddComponentToEntity<IScriptComponent>(player, new PlayerMovementScript(gameTime, gameInputHandler, scene, player, 4 * objectSize));

            int numOfEnemies = (int)((numOfObjectsOnX / 2f) * (numOfObjectsOnY / 2f));

            if (generateTheGuy)
            {
                for (int i = 0; i < numOfEnemies; i++)
                {
                    int x = _rnd.Next(10, numOfObjectsOnX - 10);
                    int y = _rnd.Next(10, numOfObjectsOnY - 10);

                    SetupCharacter(scene, manager, objectSize, gameTime, new Vector2(x, y));
                }
            }

            return scene;
        }

        private static void SetupCharacter(IScene scene, IEntityManager manager, float objectSize, GameTime gameTime, Vector2 position)
        {
            ITransformComponent playerTransform = new TransformComponent(position, objectSize, objectSize, new Vector2(0, 0), 2);
            IGraphicsComponent test = new GraphicsComponent(ImgName.Enemy);

            uint player = manager.AddEntity(playerTransform);
            manager.AddComponentToEntity<IGraphicsComponent>(player, test);

            ICollisionComponent collision = new CollisionComponent(true, true);
            manager.AddComponentToEntity<ICollisionComponent>(player, collision);

            IRigidBodyComponent rigidBody = new RigidBodyComponent();
            manager.AddComponentToEntity<IRigidBodyComponent>(player, rigidBody);

            IScriptComponent movementStrategy = new AiMovementScript(gameTime, scene, player, 4 * objectSize);
            //IScriptComponent movementStrategy = new FollowPlayerScript(gameTime, scene, player, scene.PlayerEntity, 4 * objectSize);

            manager.AddComponentToEntity<IScriptComponent>(player, movementStrategy);
        }

        public static MetaScene GenerateMetaSceneFromScene(IScene scene)
        {
            MetaScene metaScene = new MetaScene(SceneType.General);
            IEntityManager manager = scene.EntityManager;
            foreach (var item in manager.GetAllEntities())
            {
                ComponentState required = 0;
                MetaMapEntity currentEntity = new MetaMapEntity();
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

                if ((currentEntity.Components & ComponentState.CollisionComponent) != ComponentState.CollisionComponent)
                {
                    metaScene.GroundEntities.Add(currentEntity);
                }
                else if ((currentEntity.CollisionType & CollisionType.Solid) == CollisionType.Solid && (currentEntity.CollisionType & CollisionType.Dynamic) != CollisionType.Dynamic && currentEntity.LifeComponent == null)
                {
                    metaScene.StaticCollisionEntities.Add(currentEntity);
                }
                else
                {
                    metaScene.DynamicEntities.Add(currentEntity);
                }
            }

            metaScene.BaseObjectSize = 1;
            metaScene.ID = scene.SceneID;
            metaScene.NextScene = scene.NextScene;
            metaScene.NumOfEntitiesOnX = scene.NumOfEntitiesOnX;
            metaScene.NumOfEntitiesOnY = scene.NumOfEntitiesOnY;
            metaScene.NumOfObjectsInCell = scene.NumOfObjectsInCell;

            return metaScene;
        }

        public static IScene GenerateSceneFromMeta(MetaScene metaScene, ICamera camera, GameInput gameInput, GameTime gameTime)
        {
            ICamera oldCamera = camera;
            int cellSize = metaScene.BaseObjectSize * metaScene.NumOfObjectsInCell;
            ISpatialIndex grid = new Grid(metaScene.NumOfEntitiesOnX, metaScene.NumOfEntitiesOnY, cellSize);
            IScene scene = new GeneralScene(new Camera(oldCamera.Width, oldCamera.Height), new EntityManager(grid), grid);
            scene.SceneID = metaScene.ID;
            scene.NumOfEntitiesOnX = metaScene.NumOfEntitiesOnX;
            scene.NumOfEntitiesOnY = metaScene.NumOfEntitiesOnY;
            scene.BaseObjectSize = metaScene.BaseObjectSize;
            scene.NumOfObjectsInCell = metaScene.NumOfObjectsInCell;
            foreach (var item in metaScene.GroundEntities)
            {
                EntityFactory.GenerateEntity(item, scene.EntityManager);
            }
            foreach (var item in metaScene.StaticCollisionEntities)
            {
                if (item != null)
                {
                    EntityFactory.GenerateEntity(item, scene.EntityManager);
                }
            }
            foreach (var item in metaScene.DynamicEntities)
            {
                if (item != null)
                {
                    uint curr = EntityFactory.GenerateEntity(item, scene.EntityManager);
                    if ((item.Scripts & ScriptType.AiMovement) == ScriptType.AiMovement)
                    {
                        scene.EntityManager.AddComponentToEntity<IScriptComponent>(curr, new AiMovementScript(gameTime, scene, curr, 2 * metaScene.BaseObjectSize));
                    }
                    if (item.LifeComponent != null && item.LifeComponent.IsPlayer)
                    {
                        scene.EntityManager.AddComponentToEntity<IScriptComponent>(curr, new PlayerMovementScript(gameTime, gameInput, scene, curr, 4 * metaScene.BaseObjectSize));
                        scene.EntityManager.AddComponentToEntity<ILifeComponent>(curr, item.LifeComponent);
                        scene.PlayerEntity = curr;
                        scene.PlayerTransform = scene.EntityManager.GetComponentOfType<ITransformComponent>(curr);
                    }
                }
            }

            if (scene.PlayerTransform == null)
            {
                GeneratePlayer(scene.EntityManager, scene, metaScene.BaseObjectSize, gameTime, gameInput);
            }

            scene.NextScene = metaScene.NextScene;

            return scene;
        }

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
            scene.PlayerTransform = playerTransform;

            manager.AddComponentToEntity<IScriptComponent>(player, new PlayerMovementScript(gameTime, gameInputHandler, scene, player, 4 * objectSize));

            manager.AddComponentToEntity<ILifeComponent>(player, new LifeComponent { IsPlayer = true });
        }
    }
}

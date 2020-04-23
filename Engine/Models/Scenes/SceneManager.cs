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
using GameInputHandler;
using ResourceManagers.Images;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TimeUtils;

namespace Engine.Models.Scenes
{
    public class SceneManager : ISceneManager
    {
        public IScene CurrentScene { get; set; }
        public List<byte[]> MetaScenes { get; set; }
        private GameInput _gameInput;
        private GameTime _gameTime;

        private int _currIndex;

        public SceneManager(GameInput gameInput, GameTime gameTime)
        {
            _gameInput = gameInput;
            _gameTime = gameTime;
            MetaScenes = new List<byte[]>();
            _currIndex = 0;
        }

        public SceneManager(List<MetaScene> metaScenes, GameInput gameInput, GameTime gameTime)
        {
            _gameInput = gameInput;
            _gameTime = gameTime;
            MetaScenes = new List<byte[]>();
            _currIndex = 0;

            using (MemoryStream stream = new MemoryStream())
            {
                byte[] current;
                var binaryFormatter = new BinaryFormatter();
                foreach (var item in metaScenes)
                {
                    binaryFormatter.Serialize(stream, item);
                    current = stream.ToArray();
                    MetaScenes.Add(current);
                    stream.SetLength(0);
                }
            }
        }

        public SceneManager(List<byte[]> metaScenes, GameInput gameInput, GameTime gameTime)
        {
            _gameInput = gameInput;
            _gameTime = gameTime;
            MetaScenes = metaScenes;
            _currIndex = 0;
        }

        public List<byte[]> GetScenesToSave()
        {
            byte[] currentScene = SerializeMetaScene(GenerateMetaScene());
            MetaScenes[_currIndex - 1] = currentScene;

            return MetaScenes;
        }

        public void UpdateScenes(List<byte[]> newScenes)
        {
            _currIndex = 0;
            MetaScenes = newScenes;
            CurrentScene = LoadNextScene();
        }

        private MetaScene DeserializeMetaScene(int index)
        {
            byte[] item = MetaScenes[index];
            MetaScene result;
            using (MemoryStream fs = new MemoryStream(item))
            {
                var binaryFormatter = new BinaryFormatter();
                result = binaryFormatter.Deserialize(fs) as MetaScene;
            }

            return result;
        }

        private byte[] SerializeMetaScene(MetaScene metaScene)
        {
            byte[] current;
            using (MemoryStream stream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, metaScene);
                current = stream.ToArray();
            }

            return current;
        }

        private MetaScene GenerateMetaScene()
        {
            MetaScene metaScene = new MetaScene(SceneType.General);
            IEntityManager manager = CurrentScene.EntityManager;
            foreach (var item in CurrentScene.EntityManager.GetAllEntities())
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
            metaScene.ID = CurrentScene.SceneID;
            metaScene.NextScene = CurrentScene.NextScene;
            metaScene.NumOfEntitiesOnX = CurrentScene.NumOfEntitiesOnX;
            metaScene.NumOfEntitiesOnY = CurrentScene.NumOfEntitiesOnY;
            metaScene.NumOfObjectsInCell = CurrentScene.NumOfObjectsInCell;

            return metaScene;
        }

        public IScene LoadBattleScene()
        {
            throw new NotImplementedException();
        }

        public IScene LoadNextScene()
        {
            MetaScene searched = DeserializeMetaScene(_currIndex);

            _currIndex++;

            return GenerateSceneFromMeta(searched);
        }

        private IScene GenerateSceneFromMeta(MetaScene metaScene)
        {
            ICamera oldCamera = CurrentScene == null ? new Camera(800, 600) : CurrentScene.SceneCamera;
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
                        scene.EntityManager.AddComponentToEntity<IScriptComponent>(curr, new AiMovementScript(_gameTime, scene, curr, 2 * metaScene.BaseObjectSize));
                    }
                    if (item.LifeComponent != null && item.LifeComponent.IsPlayer)
                    {
                        scene.EntityManager.AddComponentToEntity<IScriptComponent>(curr, new PlayerMovementScript(_gameTime, _gameInput, scene, curr, 4 * metaScene.BaseObjectSize));
                        scene.EntityManager.AddComponentToEntity<ILifeComponent>(curr, item.LifeComponent);
                        scene.PlayerEntity = curr;
                        scene.PlayerTransform = scene.EntityManager.GetComponentOfType<ITransformComponent>(curr);
                    }
                }
            }

            if (scene.PlayerTransform == null)
            {
                GeneratePlayer(scene.EntityManager, scene, metaScene.BaseObjectSize, _gameTime, _gameInput);
            }

            scene.NextScene = metaScene.NextScene;

            CurrentScene = scene;
            CurrentScene.NextScene = scene.NextScene;

            return scene;
        }

        private void GeneratePlayer(IEntityManager manager, IScene scene, int objectSize, GameTime gameTime, GameInput gameInputHandler)
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

            manager.AddComponentToEntity<ILifeComponent>(player, new LifeComponent { IsPlayer = true});
        }
    }
}

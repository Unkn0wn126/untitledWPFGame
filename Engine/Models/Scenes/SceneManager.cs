using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script;
using Engine.Models.Factories.Entities;
using Engine.Models.Factories.Scenes;
using GameInputHandler;
using ResourceManagers.Images;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using TimeUtils;

namespace Engine.Models.Scenes
{
    public class SceneManager : ISceneManager
    {
        public IScene CurrentScene { get; set; }
        private List<MetaScene> _metaScenes { get; set; }
        private GameInput _gameInput;
        private GameTime _gameTime;

        public SceneManager(GameInput gameInput, GameTime gameTime)
        {
            _gameInput = gameInput;
            _gameTime = gameTime;
            _metaScenes = new List<MetaScene>();
        }

        public SceneManager(List<MetaScene> metaScenes, GameInput gameInput, GameTime gameTime)
        {
            _gameInput = gameInput;
            _gameTime = gameTime;
            _metaScenes = metaScenes;
        }

        public IScene LoadBattleScene()
        {
            throw new NotImplementedException();
        }

        public IScene LoadNextScene()
        {
            if (CurrentScene == null)
            {
                return GenerateSceneFromMeta(_metaScenes[0]);
            }

            MetaScene searched = _metaScenes[0];

            foreach (var item in _metaScenes)
            {
                if (item.ID.CompareTo(CurrentScene.NextScene) == 0)
                {
                    searched = item;
                }
            }

            return GenerateSceneFromMeta(searched);
        }

        private IScene GenerateSceneFromMeta(MetaScene metaScene)
        {
            ICamera oldCamera = CurrentScene == null ? new Camera(800, 600) : CurrentScene.SceneCamera;
            int cellSize = metaScene.BaseObjectSize * metaScene.NumOfObjectsInCell;
            ISpatialIndex grid = new Grid(metaScene.NumOfEntitiesOnX, metaScene.NumOfEntitiesOnY, cellSize);
            IScene scene = new GeneralScene(new Camera(oldCamera.Width, oldCamera.Height), new EntityManager(grid), grid);
            foreach (var item in metaScene.GroundEntities)
            {
                EntityFactory.GenerateEntity(scene.EntityManager, item.Components, item.Graphics, item.CollisionType, new Vector2(item.SizeX, item.SizeY), new Vector2(item.PosX, item.PosY), item.ZIndex);
            }            
            foreach (var item in metaScene.StaticCollisionEntities)
            {
                EntityFactory.GenerateEntity(scene.EntityManager, item.Components, item.Graphics, item.CollisionType, new Vector2(item.SizeX, item.SizeY), new Vector2(item.PosX, item.PosY), item.ZIndex);
            }            
            foreach (var item in metaScene.DynamicEntities)
            {
                uint curr = EntityFactory.GenerateEntity(scene.EntityManager, item.Components, item.Graphics, item.CollisionType, new Vector2(item.SizeX, item.SizeY), new Vector2(item.PosX, item.PosY), item.ZIndex);
                if ((item.Scripts & ScriptType.AiMovement) == ScriptType.AiMovement)
                {
                    scene.EntityManager.AddComponentToEntity<IScriptComponent>(curr, new AiMovementScript(_gameTime, scene, curr, 4 * metaScene.BaseObjectSize));
                }
            }

            GeneratePlayer(scene.EntityManager, scene, metaScene.BaseObjectSize, _gameTime, _gameInput);

            CurrentScene = scene;
            CurrentScene.NextScene = metaScene.NextScene;

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
        }
    }
}

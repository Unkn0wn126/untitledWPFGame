using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Factories;
using Engine.Models.Factories.Scenes;
using GameInputHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TimeUtils;

namespace Engine.Models.Scenes
{
    public class SceneManager : ISceneManager
    {
        public IScene CurrentScene { get; set; }
        public List<byte[]> MetaScenes { get; set; }
        private GameInput _gameInput;
        private GameTime _gameTime;

        public int CurrentIndex { get; set; }

        public SceneManager(GameInput gameInput, GameTime gameTime)
        {
            _gameInput = gameInput;
            _gameTime = gameTime;
            MetaScenes = new List<byte[]>();
            CurrentIndex = 0;
        }

        public SceneManager(List<MetaScene> metaScenes, GameInput gameInput, GameTime gameTime)
        {
            _gameInput = gameInput;
            _gameTime = gameTime;
            MetaScenes = new List<byte[]>();
            CurrentIndex = 0;

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
            CurrentIndex = 0;
        }

        public List<byte[]> GetScenesToSave()
        {
            byte[] currentScene = SerializeMetaScene(SceneFactory.GenerateMetaSceneFromScene(CurrentScene));
            MetaScenes[CurrentIndex - 1] = currentScene;

            return MetaScenes;
        }

        public void UpdateScenes(List<byte[]> newScenes)
        {
            CurrentIndex = 0;
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

        public IScene LoadBattleScene()
        {
            throw new NotImplementedException();
        }

        public IScene LoadNextScene()
        {
            MetaScene searched = DeserializeMetaScene(CurrentIndex);

            CurrentIndex++;
            CurrentScene = SceneFactory.GenerateSceneFromMeta(searched, new Camera(800, 600), _gameInput, _gameTime);
            CurrentScene.SceneCamera.UpdateFocusPoint(CurrentScene.EntityManager.GetComponentOfType<ITransformComponent>(CurrentScene.PlayerEntity));
            return CurrentScene;
        }
    }
}

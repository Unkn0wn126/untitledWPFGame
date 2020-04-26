using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Components.Life;
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

        public event SceneChangeStarted SceneChangeStarted;
        public event SceneChangeFinished SceneChangeFinished;
        public event GameWon GameWon;

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

        /// <summary>
        /// Serializes the current state
        /// of the game
        /// </summary>
        /// <returns></returns>
        public List<byte[]> GetScenesToSave()
        {
            byte[] currentScene = SerializeMetaScene(SceneFactory.GenerateMetaSceneFromScene(CurrentScene));
            MetaScenes[CurrentIndex - 1] = currentScene;

            return MetaScenes;
        }

        /// <summary>
        /// Basically loads a new game
        /// </summary>
        /// <param name="newScenes"></param>
        public void UpdateScenes(List<byte[]> newScenes, int currentIndex)
        {
            CurrentIndex = currentIndex;
            MetaScenes = newScenes;
            CurrentScene = null;
            LoadNextScene();
        }

        /// <summary>
        /// Deserializes meta scene
        /// from the list of current
        /// scenes
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Serializes the given meta scene
        /// to a byte array
        /// </summary>
        /// <param name="metaScene"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Loads the next scene in the list
        /// </summary>
        public void LoadNextScene()
        {
            if (CurrentIndex >= MetaScenes.Count)
            {
                GameWon.Invoke();
                return;
            }
            SceneChangeStarted.Invoke();
            MetaScene searched = DeserializeMetaScene(CurrentIndex);

            CurrentIndex++;
            ILifeComponent currentPlayer = null;
            if (CurrentScene != null)
            {
                currentPlayer = CurrentScene.EntityManager.GetComponentOfType<ILifeComponent>(CurrentScene.PlayerEntity);
            }
            CurrentScene = SceneFactory.GenerateSceneFromMeta(searched, new Camera(800, 600), _gameInput, _gameTime, currentPlayer, LoadNextScene);
            CurrentScene.SceneCamera.UpdateFocusPoint(CurrentScene.EntityManager.GetComponentOfType<ITransformComponent>(CurrentScene.PlayerEntity));
            SceneChangeFinished.Invoke();
        }
    }
}

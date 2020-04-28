using Engine.Coordinates;
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Components.Life;
using Engine.Models.Components.Script;
using Engine.Models.Components.Script.BattleState;
using Engine.Models.Factories;
using Engine.Models.Factories.Scenes;
using GameInputHandler;
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
        public event GameEnd GameWon;
        public event GameEnd GameLost;

        private IScene _returnWorldScene;

        public int CurrentIndex { get; set; }
        public BattleSceneMediator BattleSceneMediator { get; set; }

        private uint _enemyEntityToRemove;
        private bool _removeEnemyEntity;
        private bool _alreadInBattle;

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
            byte[] currentScene;
            if (CurrentScene.SceneType != SceneType.Battle)
            {
                currentScene = SerializeMetaScene(SceneFactory.GenerateMetaSceneFromScene(CurrentScene));
                MetaScenes[CurrentIndex - 1] = currentScene;
            }


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

        private void PrepareBattleScene(uint enemy)
        {
            _enemyEntityToRemove = enemy;
            _removeEnemyEntity = true;
            ILifeComponent playerLife = CurrentScene.EntityManager.GetComponentOfType<ILifeComponent>(CurrentScene.PlayerEntity);
            ILifeComponent enemyLife = CurrentScene.EntityManager.GetComponentOfType<ILifeComponent>(enemy);

            LoadBattleScene(playerLife, enemyLife);
        }

        public void LoadBattleScene(ILifeComponent player, ILifeComponent enemy)
        {
            if (CurrentScene.SceneType != SceneType.Battle && !_alreadInBattle)
            {
                _alreadInBattle = true;
                SceneChangeStarted.Invoke();
                ISpatialIndex grid = new Grid(2, 2, 2);
                IScene scene = new GeneralScene(new Camera(CurrentScene.SceneCamera.Width, CurrentScene.SceneCamera.Height), new EntityManagers.EntityManager(grid), grid, SceneType.Battle);
                uint playerID = scene.EntityManager.AddEntity(new TransformComponent(new System.Numerics.Vector2(0, 0), 1, 1, new System.Numerics.Vector2(0, 0), 1));
                uint enemyID = scene.EntityManager.AddEntity(new TransformComponent(new System.Numerics.Vector2(0, 1), 1, 1, new System.Numerics.Vector2(0, 0), 1));
                uint battleManager = scene.EntityManager.AddEntity();

                IGraphicsComponent playerAvatar = new GraphicsComponent(ResourceManagers.Images.ImgName.Player);
                IGraphicsComponent enemyAvatar = new GraphicsComponent(ResourceManagers.Images.ImgName.Enemy);

                scene.EntityManager.AddComponentToEntity(playerID, playerAvatar);
                scene.EntityManager.AddComponentToEntity(enemyID, enemyAvatar);

                scene.EntityManager.AddComponentToEntity(playerID, player);
                scene.EntityManager.AddComponentToEntity(enemyID, enemy);

                BattleStateMachine enemyBattleSceneState = new BattleStateMachine(enemy);
                BattleStateMachine playerBattleSceneState = new BattleStateMachine(player);
                UpdateBattleSceneMediator(playerAvatar, enemyAvatar, enemy, player, playerBattleSceneState);
                scene.EntityManager.AddComponentToEntity<IScriptComponent>(enemyID, new AIBattleScript(enemyBattleSceneState, _gameTime));
                scene.EntityManager.AddComponentToEntity<IScriptComponent>(battleManager, new HandleBattleScript(player, enemy, playerBattleSceneState, enemyBattleSceneState, new GameEnd(EndGame), new GameEnd(LoadNextScene), BattleSceneMediator.MessageProcessor));

                scene.PlayerEntity = playerID;

                _returnWorldScene = CurrentScene;

                CurrentScene = scene;
                SceneChangeFinished.Invoke();
            }
        }

        private void EndGame()
        {
            SceneChangeStarted.Invoke();
            GameLost.Invoke();
        }

        private void UpdateBattleSceneMediator(IGraphicsComponent playerAvatar, IGraphicsComponent enemyAvatar, ILifeComponent enemyLife, ILifeComponent playerLife, BattleStateMachine playerBattleState)
        {
            BattleSceneMediator.PlayerAvatar = playerAvatar;
            BattleSceneMediator.EnemyAvatar = enemyAvatar;
            BattleSceneMediator.EnemyLife = enemyLife;
            BattleSceneMediator.PlayerLife = playerLife;
            BattleSceneMediator.PlayerBattleState = playerBattleState;
        }

        private void LoadBackWorld()
        {
            SceneChangeStarted.Invoke();
            _alreadInBattle = false;
            if (_removeEnemyEntity)
            {
                _returnWorldScene.EntityManager.RemoveEntity(_enemyEntityToRemove);
            }
            ILifeComponent player = CurrentScene.EntityManager.GetComponentOfType<ILifeComponent>(CurrentScene.PlayerEntity);
            ILifeComponent currentWorldPlayer = _returnWorldScene.EntityManager.GetComponentOfType<ILifeComponent>(_returnWorldScene.PlayerEntity);
            UpdatePlayerStats(currentWorldPlayer, player);
            CurrentScene = _returnWorldScene;

            SceneChangeFinished.Invoke();
        }

        private void UpdatePlayerStats(ILifeComponent oldStats, ILifeComponent newStats)
        {
            oldStats.IsPlayer = newStats.IsPlayer;
            oldStats.Name = newStats.Name;
            oldStats.Gender = newStats.Gender;
            oldStats.Race = newStats.Race;
            oldStats.BattleClass = newStats.BattleClass;
            oldStats.Strength = newStats.Strength;
            oldStats.Agility = newStats.Agility;
            oldStats.Intelligence = newStats.Intelligence;
            oldStats.MaxStamina = newStats.MaxStamina;
            oldStats.Stamina = newStats.Stamina;
            oldStats.MaxHP = newStats.MaxHP;
            oldStats.HP = newStats.HP;
            oldStats.MaxMP = newStats.MaxMP;
            oldStats.MP = newStats.MP;
            oldStats.AttributePoints = newStats.AttributePoints;
            oldStats.CurrentXP = newStats.CurrentXP;
            oldStats.CurrentLevel = newStats.CurrentLevel;
            oldStats.NextLevelXP = newStats.NextLevelXP;
        }

        /// <summary>
        /// Loads the next scene in the list
        /// </summary>
        public void LoadNextScene()
        {
            if (CurrentScene?.SceneType == SceneType.Battle)
            {
                LoadBackWorld();
                return;
            }
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
            CurrentScene = SceneFactory.GenerateSceneFromMeta(searched, new Camera(800, 600), _gameInput, _gameTime, currentPlayer, LoadNextScene, new BattleInitialization(PrepareBattleScene));
            CurrentScene.SceneCamera.UpdateFocusPoint(CurrentScene.EntityManager.GetComponentOfType<ITransformComponent>(CurrentScene.PlayerEntity));
            SceneChangeFinished.Invoke();
        }
    }
}

using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Components.Life;
using Engine.Models.Components.Navmesh;
using Engine.Models.Components.Script;
using Engine.Models.Factories.Entities;
using Engine.Models.Factories.Scenes;
using Engine.Models.Scenes;
using GameInputHandler;
using ResourceManagers.Images;
using System;
using System.Collections.Generic;
using TimeUtils;

namespace Engine.Models.Factories
{
    /// <summary>
    /// Class for generating meta scenes
    /// and actual scenes
    /// </summary>
    public static class SceneFactory
    {
        private static Random _rnd = new Random();

        /// <summary>
        /// Generates ground meta entities
        /// </summary>
        /// <param name="numOfObjectsOnX"></param>
        /// <param name="numOfObjectsOnY"></param>
        /// <param name="baseObjectSize"></param>
        /// <param name="staticCollisionsPositions"></param>
        /// <returns></returns>
        private static List<MetaEntity> GenerateGround(int numOfObjectsOnX, int numOfObjectsOnY, int baseObjectSize, bool[,] staticCollisionsPositions)
        {
            MetaEntity[,] metaMap = new MetaEntity[numOfObjectsOnX, numOfObjectsOnY];

            for (int i = 0; i < metaMap.GetLength(0); i++)
            {
                for (int j = 0; j < metaMap.GetLength(1); j++)
                {
                    if (!staticCollisionsPositions[i, j])
                    {
                        SetUpGroundEntity(metaMap, i, j, baseObjectSize, staticCollisionsPositions);
                    }
                }
            }

            return ConvertMatrixToList(metaMap);
        }

        /// <summary>
        /// Creates new ground meta entity
        /// and adds it to the matrix
        /// </summary>
        /// <param name="metaMap"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="baseObjectSize"></param>
        /// <param name="staticCollisionsPositions"></param>
        private static void SetUpGroundEntity(MetaEntity[,] metaMap, int i, int j, int baseObjectSize, bool[,] staticCollisionsPositions)
        {
            ComponentState current;
            ImgName currImg = (ImgName)_rnd.Next(1, 4);
            current = ComponentState.GraphicsComponent | ComponentState.TransformComponent | ComponentState.NavMeshComponent;
            NavmeshContinues navmeshDirections = DetermineNavmeshContinuation(metaMap, i, j, staticCollisionsPositions);
            metaMap[i, j] = new MetaEntity
            {
                CollisionType = CollisionType.None,
                Graphics = currImg,
                Components = current,
                ZIndex = 0,
                PosX = i * baseObjectSize,
                PosY = j * baseObjectSize,
                SizeX = baseObjectSize,
                SizeY = baseObjectSize,
                NavmeshContinuation = navmeshDirections
            };
        }

        /// <summary>
        /// Determines which way does
        /// the navmesh continue
        /// </summary>
        /// <param name="metaMap"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="staticCollisionsPositions"></param>
        /// <returns></returns>
        private static NavmeshContinues DetermineNavmeshContinuation(MetaEntity[,] metaMap, int i, int j, bool[,] staticCollisionsPositions)
        {
            NavmeshContinues navmeshDirections = 0;
            if (j > 1 && !staticCollisionsPositions[i, j - 1])
            {
                navmeshDirections |= NavmeshContinues.Left;
            }
            if (j < metaMap.GetLength(1) - 1 && !staticCollisionsPositions[i, j + 1])
            {
                navmeshDirections |= NavmeshContinues.Right;
            }
            if (i > 1 && !staticCollisionsPositions[i - 1, j])
            {
                navmeshDirections |= NavmeshContinues.Up;
            }
            if (i < metaMap.GetLength(0) - 1 && !staticCollisionsPositions[i + 1, j])
            {
                navmeshDirections |= NavmeshContinues.Down;
            }

            return navmeshDirections;
        }

        /// <summary>
        /// Generates the static collision
        /// meta entities
        /// </summary>
        /// <param name="staticCollisionsPositions"></param>
        /// <param name="baseObjectSize"></param>
        /// <returns></returns>
        private static List<MetaEntity> GenerateStaticBlocks(bool[,] staticCollisionsPositions, int baseObjectSize)
        {
            MetaEntity[,] metaMap = new MetaEntity[staticCollisionsPositions.GetLength(0), staticCollisionsPositions.GetLength(1)];
            for (int i = 0; i < metaMap.GetLength(0); i++)
            {
                for (int j = 0; j < metaMap.GetLength(1); j++)
                {
                    if (staticCollisionsPositions[i,j])
                    {
                        SetUpStaticCollisionEntity(metaMap, baseObjectSize, i, j);
                    }
                }
            }

            return ConvertMatrixToList(metaMap);
        }
        /// <summary>
        /// Creates a new static collision entity
        /// and adds it to the matrix
        /// </summary>
        /// <param name="metaMap"></param>
        /// <param name="staticCollisionsPositions"></param>
        /// <param name="baseObjectSize"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private static void SetUpStaticCollisionEntity(MetaEntity[,] metaMap, int baseObjectSize, int i, int j)
        {
            ComponentState current = ComponentState.GraphicsComponent | ComponentState.TransformComponent | ComponentState.CollisionComponent;
            metaMap[i, j] = new MetaEntity
            {
                CollisionType = CollisionType.Solid,
                Graphics = ImgName.Cobblestone,
                Components = current,
                ZIndex = 1,
                PosX = i * baseObjectSize,
                PosY = j * baseObjectSize,
                SizeX = baseObjectSize,
                SizeY = baseObjectSize
            };
        }

        /// <summary>
        /// Converts 2D array
        /// of meta entities
        /// to a list of
        /// meta entities
        /// </summary>
        /// <param name="metaMap"></param>
        /// <returns></returns>
        private static List<MetaEntity> ConvertMatrixToList(MetaEntity[,] metaMap)
        {
            List<MetaEntity> output = new List<MetaEntity>();
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
            bool[,] staticCollisionsPositions = GenerateCollisions(numOfObjectsOnX, numOfObjectsOnY);

            metaScene.GroundEntities = GenerateGround(numOfObjectsOnX, numOfObjectsOnY, baseObjectSize, staticCollisionsPositions);

            metaScene.StaticCollisionEntities = GenerateStaticBlocks(staticCollisionsPositions, baseObjectSize);

            int numOfEnemies = numOfObjectsOnX / 2;//(int)((numOfObjectsOnX / 4f) * (numOfObjectsOnY / 4f));
            int currEnemyXIndex = _rnd.Next(numOfObjectsOnX);
            int currEnemyYIndex = _rnd.Next(numOfObjectsOnY);

            for (int i = 0; i < numOfEnemies; i++)
            {
                while (staticCollisionsPositions[currEnemyXIndex, currEnemyYIndex])
                {
                    currEnemyXIndex = _rnd.Next(numOfObjectsOnX);
                    currEnemyYIndex = _rnd.Next(numOfObjectsOnY);
                }

                metaScene.LivingEntities.Add(GenerateDynamicEntities(numOfObjectsOnX, numOfObjectsOnY, baseObjectSize, currEnemyXIndex, currEnemyYIndex));

                currEnemyXIndex = _rnd.Next(numOfObjectsOnX);
                currEnemyYIndex = _rnd.Next(numOfObjectsOnY);
            }

            metaScene.LivingEntities.Add(GenerateMetaPlayer(lifeComponent, baseObjectSize, 1, 1));

            return metaScene;
        }

        private static MetaEntity GenerateDynamicEntities(int numOfObjectsOnX, int numOfObjectsOnY, int baseObjectSize, int xPos, int yPos)
        {
            MetaEntity metaMapEntity = new MetaEntity();
            ComponentState current = ComponentState.GraphicsComponent | ComponentState.TransformComponent | ComponentState.CollisionComponent | ComponentState.RigidBodyComponent | ComponentState.LifeComponent;
            metaMapEntity = new MetaEntity { CollisionType = CollisionType.Solid | CollisionType.Dynamic, Graphics = ImgName.Enemy, Components = current, ZIndex = 3, PosX = xPos, PosY = yPos, SizeX = baseObjectSize, SizeY = baseObjectSize };
            metaMapEntity.Scripts = ScriptType.AiMovement;
            metaMapEntity.LifeComponent = new LifeComponent { Agility = 10, AttributePoints = 0, BattleClass = BattleClass.Swordsman, CurrentLevel = 1, CurrentXP = 0, Gender = Gender.Male, MaxHP = 100, HP = 100, Intelligence = 10, IsPlayer = false, MaxMP = 100, MaxStamina = 100, MP = 100, Name = "Prak", NextLevelXP = 100, Race = Race.Human, Stamina = 100, Strength = 10 };
            return metaMapEntity;
        }

        private static MetaEntity GenerateMetaPlayer(ILifeComponent lifeComponent, int baseObjectSize, int xPos, int yPos)
        {
            MetaEntity metaMapEntity = new MetaEntity();
            ComponentState current = ComponentState.GraphicsComponent | ComponentState.TransformComponent | ComponentState.CollisionComponent | ComponentState.RigidBodyComponent | ComponentState.LifeComponent;
            metaMapEntity = new MetaEntity { CollisionType = CollisionType.Solid | CollisionType.Dynamic, Graphics = ImgName.Player, Components = current, ZIndex = 2, PosX = xPos, PosY = yPos, SizeX = baseObjectSize, SizeY = baseObjectSize };
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
                MetaEntity currentEntity = EntityFactory.GenerateMetaEntityFromEntity(manager, item);

                DetermineEntityType(metaScene, currentEntity);
            }

            metaScene.BaseObjectSize = 1;
            metaScene.NumOfEntitiesOnX = scene.NumOfEntitiesOnX;
            metaScene.NumOfEntitiesOnY = scene.NumOfEntitiesOnY;
            metaScene.NumOfObjectsInCell = scene.NumOfObjectsInCell;

            return metaScene;
        }

        private static void DetermineEntityType(MetaScene metaScene, MetaEntity currentEntity)
        {
            // TODO: Update this to the new structure
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
                metaScene.LivingEntities.Add(currentEntity);
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

        private static bool IsGroundEntity(MetaEntity currentEntity)
        {
            return !IsComponentRequired(currentEntity.Components, ComponentState.CollisionComponent) && IsComponentRequired(currentEntity.Components, ComponentState.NavMeshComponent);
        }

        private static bool IsStaticCollisionEntity(MetaEntity currentEntity)
        {
            return IsCollisionType(currentEntity.CollisionType, CollisionType.Solid) && !IsCollisionType(currentEntity.CollisionType, CollisionType.Dynamic) && currentEntity.LifeComponent == null;
        }

        public static IScene GenerateSceneFromMeta(MetaScene metaScene, ICamera camera, GameInput gameInput, GameTime gameTime, ILifeComponent currentPlayer)
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
            foreach (var item in metaScene.LivingEntities)
            {
                if (item != null)
                {
                    if (item.LifeComponent != null && item.LifeComponent.IsPlayer && currentPlayer != null)
                    {
                        item.LifeComponent = currentPlayer;
                    }

                    uint curr = EntityFactory.GenerateEntity(item, scene, scene.EntityManager, gameTime, gameInput);
                    if (item.LifeComponent != null && item.LifeComponent.IsPlayer)
                    {
                        scene.PlayerEntity = curr;
                    }
                }
            }

            return scene;
        }

        /// <summary>
        /// Generates a matrix of booleans
        /// representing indexes with walls
        /// by boolean value true
        /// </summary>
        /// <param name="numOfObjectsOnX"></param>
        /// <param name="numOfObjectsOnY"></param>
        /// <returns></returns>
        public static bool[,] GenerateCollisions(int numOfObjectsOnX, int numOfObjectsOnY)
        {
            bool[,] map = new bool[numOfObjectsOnX, numOfObjectsOnY];
            GenerateEdges(map);
            GenerateCollisionsInside(map);
            return map;
        }

        /// <summary>
        /// Randomly generates walls
        /// inside of the playable area
        /// </summary>
        /// <param name="map"></param>
        private static void GenerateCollisionsInside(bool[,] map)
        {
            int block = _rnd.Next(2);
            Stack<Tuple<int, int>> vacantToCheck = new Stack<Tuple<int, int>>();
            List<Tuple<int, int>> visitedIndexes = new List<Tuple<int, int>>();
            for (int i = 1; i < map.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < map.GetLength(1) - 1; j++)
                {
                    map[i, j] = block == 1 && (i != 1 && j != 1);
                    if (!map[i, j])
                    {
                        vacantToCheck.Push(new Tuple<int, int>(i, j));
                    }
                    block = _rnd.Next(2);
                }
            }

            while (vacantToCheck.Count > 0)
            {
                Tuple<int, int> tuple = vacantToCheck.Pop();
                if (visitedIndexes.Find(x => x.Item1 == tuple.Item1 && x.Item2 == tuple.Item2) != null) // already visited?
                {
                    continue;
                }

                int numOfRows = map.GetLength(0);
                int numOfCols = map.GetLength(1);

                bool canGoLeft = tuple.Item2 > 2;
                bool canGoRight = tuple.Item2 < map.GetLength(1) - 2;
                bool canGoUp = tuple.Item1 > 2;
                bool canGoDown = tuple.Item1 < map.GetLength(0) - 2;

                List<int> possibleXCoords = new List<int>();
                List<int> possibleYCoords = new List<int>();

                if (canGoLeft)
                    possibleYCoords.Add(tuple.Item2 - 1);
                if (canGoRight)
                    possibleYCoords.Add(tuple.Item2 + 1);
                if (canGoUp)
                    possibleXCoords.Add(tuple.Item1 - 1);
                if (canGoDown)
                    possibleXCoords.Add(tuple.Item1 + 1);

                int nextXIndex = _rnd.Next(possibleXCoords.Count);
                int nextYIndex = _rnd.Next(possibleYCoords.Count);

                bool moveUpDown = canGoUp || canGoDown;
                bool moveLeftRight = canGoLeft || canGoRight;

                if (moveLeftRight)
                {
                    bool vacantFound = false;
                    foreach (var item in possibleYCoords)
                    {
                        int currValue = _rnd.Next(2);
                        bool addAnother = vacantFound && currValue == 1;
                        if (map[tuple.Item1, item] && addAnother)
                        {
                            map[tuple.Item1, item] = false;
                        }
                        else
                        {
                            vacantFound = true;
                        }

                        if (!map[tuple.Item1, item])
                        {
                            vacantToCheck.Push(new Tuple<int, int>(tuple.Item1, item));
                        }
                    }

                }
                if (moveUpDown)
                {

                    bool vacantFound = false;
                    foreach (var item in possibleXCoords)
                    {
                        int currValue = _rnd.Next(2);
                        bool addAnother = vacantFound && currValue == 1;
                        if (map[item, tuple.Item2] && addAnother)
                        {
                            map[item, tuple.Item2] = false;
                        }
                        else
                        {
                            vacantFound = true;
                        }

                        if (!map[item, tuple.Item2])
                        {
                            vacantToCheck.Push(new Tuple<int, int>(item, tuple.Item2));
                        }
                    }
                }

                visitedIndexes.Add(tuple);
            }
        }

        /// <summary>
        /// Generates walls around
        /// the playable area
        /// </summary>
        /// <param name="map"></param>
        private static void GenerateEdges(bool[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == 0 || i == map.GetLength(0) - 1 || j == 0 || j == map.GetLength(1) - 1)
                    {
                        map[i, j] = true;
                    }
                }
            }
        }
    }
}

using Engine.Models.Factories.Entities;
using System;
using System.Collections.Generic;

namespace Engine.Models.Factories.Scenes
{
    /// <summary>
    /// Used to check the type
    /// of the scene
    /// </summary>
    public enum SceneType
    {
        General,
        Battle
    }

    /// <summary>
    /// Reduced version of the
    /// normal scene to hold
    /// in the memory or
    /// save to file
    /// </summary>
    [Serializable]
    public class MetaScene
    {
        public SceneType Type { get; set; }
        public int NumOfObjectsInCell { get; set; }
        public int BaseObjectSize { get; set; }
        public int NumOfEntitiesOnX { get; set; }
        public int NumOfEntitiesOnY { get; set; }
        public List<MetaEntity> GroundEntities { get; set; }
        public List<MetaEntity> StaticCollisionEntities { get; set; }
        public List<MetaEntity> TriggerEntities { get; set; }
        public List<MetaEntity> LivingEntities { get; set; }
        public List<MetaEntity> OtherEntities { get; set; }
        public Guid ID { get; set; }
        public Guid NextScene { get; set; }

        public MetaScene(SceneType type)
        {
            ID = Guid.NewGuid();
            NextScene = Guid.Empty;
            Type = type;
            GroundEntities = new List<MetaEntity>();
            StaticCollisionEntities = new List<MetaEntity>();
            TriggerEntities = new List<MetaEntity>();
            LivingEntities = new List<MetaEntity>();
            OtherEntities = new List<MetaEntity>();
        }
    }
}

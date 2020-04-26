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
        public List<MetaMapEntity> GroundEntities { get; set; }
        public List<MetaMapEntity> StaticCollisionEntities { get; set; }
        public List<MetaMapEntity> TriggerEntities { get; set; }
        public List<MetaMapEntity> LivingEntities { get; set; }
        public List<MetaMapEntity> OtherEntities { get; set; }
        public Guid ID { get; set; }
        public Guid NextScene { get; set; }

        public MetaScene(SceneType type)
        {
            ID = Guid.NewGuid();
            NextScene = Guid.Empty;
            Type = type;
            GroundEntities = new List<MetaMapEntity>();
            StaticCollisionEntities = new List<MetaMapEntity>();
            TriggerEntities = new List<MetaMapEntity>();
            LivingEntities = new List<MetaMapEntity>();
            OtherEntities = new List<MetaMapEntity>();
        }
    }
}

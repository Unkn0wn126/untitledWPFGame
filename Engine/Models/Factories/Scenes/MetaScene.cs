using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Factories.Scenes
{
    public enum SceneType
    {
        General,
        Battle
    }
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

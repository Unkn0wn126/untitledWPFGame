using Engine.EntityManagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public class Entity : IEntity
    {
        public uint ID { get; set; }
        public IEntityManager Manager { get; set; }

        public Entity(uint id, IEntityManager manager)
        {
            ID = id;
            Manager = manager;
        }
    }
}

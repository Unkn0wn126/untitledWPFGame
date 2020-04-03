using Engine.EntityManagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    public interface IEntity
    {
        public uint ID { get; set; }
        public IEntityManager Manager { get; set; }
    }
}

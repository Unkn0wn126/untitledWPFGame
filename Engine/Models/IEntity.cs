using Engine.EntityManagers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    /// <summary>
    /// Not currently used
    /// Kept for possible future expansion's sake
    /// </summary>
    public interface IEntity
    {
        uint ID { get; set; }
        IEntityManager Manager { get; set; }
    }
}

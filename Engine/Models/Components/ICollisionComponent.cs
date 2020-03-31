﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components
{
    public interface ICollisionComponent : IGameComponent
    {
        public ITransformComponent Transform { get; set; }
    }
}

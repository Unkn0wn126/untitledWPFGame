﻿using Engine.Models.Components;
using Engine.Models.GameStateMachine;
using Engine.Models.MovementStateStrategies;
using Engine.Models.Scenes;
using Engine.Processors;
using Engine.ResourceConstants.Images;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.ViewModels
{
    public interface IGame
    {
        public GameStateMachine State { get; set; }
        public ImagePaths ImgPaths { get; set; }
        public IScene CurrentScene { get; set; }
        public void Update();
        public void HandleUserInput(AxisStrategy axisStrategy);
        public void UpdateGraphics();
    }
}

using Engine.Models.Components;
using Engine.Models.Components.Life;
using Engine.Models.Factories.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Saving
{
    [Serializable]
    public class Save
    {
        public List<byte[]> Scenes { get; set; }
        public byte[] CurrentScene { get; set; }
        public float PlayerPosX { get; set; }
        public float PlayerPosY { get; set; }
        public float PlayerSizeX { get; set; }
        public float PlayerSizeY { get; set; }
        public float PlayerZIndex { get; set; }
        public ILifeComponent PlayerLife { get; set; }
    }
}

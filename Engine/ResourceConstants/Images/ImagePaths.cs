using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.ResourceConstants.Images
{
    public enum ImgName
    {
        Dirt,
        Cobblestone,
        Rock,
        Player
    }

    public class ImagePaths
    {
        public Dictionary<ImgName, Uri> ImageSprites { get; set; }

        public ImagePaths()
        {
            ImageSprites = new Dictionary<ImgName, Uri>();
            ImageSprites.Add(ImgName.Dirt, new Uri(@"./Resources/Images/ground.jpg", UriKind.Relative));
            ImageSprites.Add(ImgName.Cobblestone, new Uri(@"./Resources/Images/ground_test.png", UriKind.Relative));
            ImageSprites.Add(ImgName.Rock, new Uri(@"./Resources/Images/rock.jpg", UriKind.Relative));
            ImageSprites.Add(ImgName.Player, new Uri(@"./Resources/Images/player.png", UriKind.Relative));
        }
    }
}

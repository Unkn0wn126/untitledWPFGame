using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.ResourceConstants.Images
{
    public enum ImgNames
    {
        DIRT,
        COBBLESTONE,
        ROCK,
        PLAYER
    }

    public class ImagePaths
    {
        public Dictionary<ImgNames, Uri> ImageSprites { get; set; }

        public ImagePaths()
        {
            ImageSprites = new Dictionary<ImgNames, Uri>();
            ImageSprites.Add(ImgNames.DIRT, new Uri(@"./Resources/Images/ground.jpg", UriKind.Relative));
            ImageSprites.Add(ImgNames.COBBLESTONE, new Uri(@"./Resources/Images/ground_test.png", UriKind.Relative));
            ImageSprites.Add(ImgNames.ROCK, new Uri(@"./Resources/Images/rock.jpg", UriKind.Relative));
            ImageSprites.Add(ImgNames.PLAYER, new Uri(@"./Resources/Images/player.png", UriKind.Relative));
        }
    }
}

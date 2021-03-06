using System;
using System.Collections.Generic;

namespace ResourceManagers.Images
{
    /// <summary>
    /// Defines the images for the systems
    /// </summary>
    public enum ImgName
    {
        None,
        Dirt,
        Grass,
        Rock,
        Cobblestone,
        Player,
        Enemy,
        PlayerLeft,
        PlayerRight,
        LocationTrigger
    }

    /// <summary>
    /// Mediator to get the image info
    /// to logic and graphics
    /// </summary>
    public class ImagePaths
    {
        // use to keep all paths to all images
        public Dictionary<ImgName, Uri> ImageSprites { get; set; }
        public Dictionary<ImgName, byte[]> ColorSprites { get; set; }

        public ImagePaths()
        {
            InitImages();

            InitColors();
        }

        /// <summary>
        /// Assigns the image values
        /// to the image names
        /// </summary>
        private void InitImages()
        {
            ImageSprites = new Dictionary<ImgName, Uri>();
            ImageSprites.Add(ImgName.None, new Uri(@"./Resources/Images/ground.jpg", UriKind.Relative));
            ImageSprites.Add(ImgName.Dirt, new Uri(@"./Resources/Images/ground.jpg", UriKind.Relative));
            ImageSprites.Add(ImgName.Grass, new Uri(@"./Resources/Images/grass.png", UriKind.Relative));
            ImageSprites.Add(ImgName.Cobblestone, new Uri(@"./Resources/Images/cobblestone.png", UriKind.Relative));
            ImageSprites.Add(ImgName.Rock, new Uri(@"./Resources/Images/rock.jpg", UriKind.Relative));
            ImageSprites.Add(ImgName.Player, new Uri(@"./Resources/Images/player.png", UriKind.Relative));
            ImageSprites.Add(ImgName.Enemy, new Uri(@"./Resources/Images/enemy.png", UriKind.Relative));
            ImageSprites.Add(ImgName.PlayerLeft, new Uri(@"./Resources/Images/player_left.png", UriKind.Relative));
            ImageSprites.Add(ImgName.PlayerRight, new Uri(@"./Resources/Images/player_right.png", UriKind.Relative));
            ImageSprites.Add(ImgName.LocationTrigger, new Uri(@"./Resources/Images/rock_trigger.png", UriKind.Relative));
        }

        /// <summary>
        /// Assigns the color values
        /// to the image names
        /// </summary>
        private void InitColors()
        {
            ColorSprites = new Dictionary<ImgName, byte[]>();
            ColorSprites.Add(ImgName.None, new byte[3] { 255, 255, 255});
            ColorSprites.Add(ImgName.Dirt, new byte[3] { 51, 34, 23});
            ColorSprites.Add(ImgName.Grass, new byte[3] { 34, 51, 23});
            ColorSprites.Add(ImgName.Cobblestone, new byte[3] { 28, 28, 28 });
            ColorSprites.Add(ImgName.Rock, new byte[3] { 23, 43, 40 });
            ColorSprites.Add(ImgName.Player, new byte[3] { 78, 71, 255});
            ColorSprites.Add(ImgName.Enemy, new byte[3] { 255, 25, 25 });
            ColorSprites.Add(ImgName.PlayerLeft, new byte[3] { 78, 71, 255 });
            ColorSprites.Add(ImgName.PlayerRight, new byte[3] { 78, 71, 255 });
            ColorSprites.Add(ImgName.LocationTrigger, new byte[3] { 255, 175, 25 });
        }
    }
}

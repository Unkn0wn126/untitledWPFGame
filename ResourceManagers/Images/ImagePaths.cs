using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagers.Images
{
    /// <summary>
    /// Defines the images for the systems
    /// </summary>
    public enum ImgName
    {
        Dirt,
        Cobblestone,
        Rock,
        Player,
        Enemy,
        PlayerLeft,
        PlayerRight
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

        private void InitImages()
        {
            ImageSprites = new Dictionary<ImgName, Uri>();
            ImageSprites.Add(ImgName.Dirt, new Uri(@"./Resources/Images/ground.jpg", UriKind.Relative));
            ImageSprites.Add(ImgName.Cobblestone, new Uri(@"./Resources/Images/ground_test.png", UriKind.Relative));
            ImageSprites.Add(ImgName.Rock, new Uri(@"./Resources/Images/rock.jpg", UriKind.Relative));
            ImageSprites.Add(ImgName.Player, new Uri(@"./Resources/Images/player.png", UriKind.Relative));
            ImageSprites.Add(ImgName.Enemy, new Uri(@"./Resources/Images/enemy.png", UriKind.Relative));
            ImageSprites.Add(ImgName.PlayerLeft, new Uri(@"./Resources/Images/player_left.png", UriKind.Relative));
            ImageSprites.Add(ImgName.PlayerRight, new Uri(@"./Resources/Images/player_right.png", UriKind.Relative));
        }

        private void InitColors()
        {
            ColorSprites = new Dictionary<ImgName, byte[]>();
            ColorSprites.Add(ImgName.Dirt, new byte[3] { 51, 34, 23});
            ColorSprites.Add(ImgName.Cobblestone, new byte[3] { 23, 43, 40 });
            ColorSprites.Add(ImgName.Rock, new byte[3] { 28, 28, 28 });
            ColorSprites.Add(ImgName.Player, new byte[3] { 78, 71, 255});
            ColorSprites.Add(ImgName.Enemy, new byte[3] { 255, 25, 25 });
            ColorSprites.Add(ImgName.PlayerLeft, new byte[3] { 78, 71, 255 });
            ColorSprites.Add(ImgName.PlayerRight, new byte[3] { 78, 71, 255 });
        }
    }
}

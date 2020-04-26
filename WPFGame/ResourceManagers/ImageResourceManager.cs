using ResourceManagers.Images;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFGame.ResourceManagers
{
    /// <summary>
    /// Helper class to
    /// bridge logic to
    /// graphics context
    /// </summary>
    public class ImageResourceManager
    {
        private readonly Dictionary<ImgName, BitmapImage> _sprites = new Dictionary<ImgName, BitmapImage>();
        private readonly Dictionary<ImgName, Brush> _colors = new Dictionary<ImgName, Brush>();
        public ImageResourceManager(ImagePaths imagePaths)
        {
            _sprites = new Dictionary<ImgName, BitmapImage>();
            _colors = new Dictionary<ImgName, Brush>();

            LoadImages(imagePaths);
            LoadColors(imagePaths);
        }

        /// <summary>
        /// Loads bitmap image
        /// values for the given
        /// graphics components
        /// </summary>
        /// <param name="imagePaths"></param>
        private void LoadImages(ImagePaths imagePaths)
        {
            var paths = Enum.GetValues(typeof(ImgName));
            foreach (var item in paths)
            {
                _sprites.Add((ImgName)item, new BitmapImage(imagePaths.ImageSprites[(ImgName)item]));
            }
        }        
        
        /// <summary>
        /// Loads the color values
        /// for the given graphics
        /// components
        /// </summary>
        /// <param name="imagePaths"></param>
        private void LoadColors(ImagePaths imagePaths)
        {
            var paths = Enum.GetValues(typeof(ImgName));
            foreach (var item in paths)
            {
                byte[] currColor = imagePaths.ColorSprites[(ImgName)item];
                _colors.Add((ImgName)item, new SolidColorBrush(Color.FromRgb(currColor[0], currColor[1], currColor[2])));
            }
        }

        /// <summary>
        /// Returns the bitmap
        /// image associated with
        /// the given image name
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public BitmapImage GetImage(ImgName image)
        {
            return _sprites[image];
        }

        /// <summary>
        /// Returns color
        /// associated with
        /// the given image name
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public Brush GetColor(ImgName image)
        {
            return _colors[image];
        }
    }
}

using Engine.ResourceConstants.Images;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFGame.ResourceManagers
{
    public class ImageResourceManager
    {
        private readonly Dictionary<ImgName, BitmapImage> _sprites = new Dictionary<ImgName, BitmapImage>();
        private readonly Dictionary<ImgName, Brush> _colors = new Dictionary<ImgName, Brush>();

        private Random _rnd;
        public ImageResourceManager(ImagePaths imagePaths)
        {
            _rnd = new Random();

            _sprites = new Dictionary<ImgName, BitmapImage>();
            _colors = new Dictionary<ImgName, Brush>();

            LoadImages(imagePaths);
            LoadColors(imagePaths);
        }

        private void LoadImages(ImagePaths imagePaths)
        {
            var paths = Enum.GetValues(typeof(ImgName));
            foreach (var item in paths)
            {
                _sprites.Add((ImgName)item, new BitmapImage(imagePaths.ImageSprites[(ImgName)item]));
            }
        }        
        
        private void LoadColors(ImagePaths imagePaths)
        {
            var paths = Enum.GetValues(typeof(ImgName));
            foreach (var item in paths)
            {
                _colors.Add((ImgName)item, new SolidColorBrush(Color.FromRgb((byte)_rnd.Next(256), (byte)_rnd.Next(256), (byte)_rnd.Next(256))));
            }
        }
        public BitmapImage GetImage(ImgName image)
        {
            return _sprites[image];
        }

        public Brush GetColor(ImgName image)
        {
            return _colors[image];
        }
    }
}

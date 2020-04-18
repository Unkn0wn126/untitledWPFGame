#define TRACE
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.MovementStateStrategies;
using Engine.Models.Scenes;
using Engine.ResourceConstants.Images;
using Engine.ViewModels;
using GameInputHandler;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFGame.ResourceManagers;

namespace WPFGame
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IGame _session;

        private UserInputHandler _inputHandler;

        // image to render to
        private RenderTargetBitmap bitmap;

        // needed for rendering
        private DrawingVisual _drawingVisual = new DrawingVisual();

        //private BitmapImage _groundImage;
        //private BitmapImage _cobbleImage;
        //private BitmapImage _rockImage;
        //private BitmapImage _playerAvatar;
        private ImageResourceManager _imageResourceManager;

        private IScene _currentScene;
        private ICamera _currentCamera;

        Rect _rectangle;

        private int _xRes;
        private int _yRes;

        public MainWindow(GameInput gameInputHandler, IGame session, int xRes, int yRes)
        {
            _xRes = xRes;
            _yRes = yRes;

            InitializeComponent();
            _session = session;
            InitializeImages();
            //InitializeCaching();

            _inputHandler = new UserInputHandler(gameInputHandler);

            // this is what everything renders to
            bitmap = new RenderTargetBitmap(xRes, yRes, 96, 96, PixelFormats.Pbgra32);
            GameImage.Source = bitmap;

            // to get shorter routes to frequently used objects
            _currentScene = _session.CurrentScene;
            _currentCamera = _currentScene.SceneCamera;
        }

        private void InitializeCaching()
        {
            var cache = new BitmapCache();
            cache.RenderAtScale = 0.5; // render at half the resolution for now
            cache.SnapsToDevicePixels = false;
            _drawingVisual.CacheMode = cache;
        }

        private void InitializeImages()
        {
            _imageResourceManager = new ImageResourceManager(_session.ImgPaths);
            // placeholder images
            // I intend to load them all at launch and assign them to a string constant
            // to give objects information of their "avatar" while keeping it independent
            //_groundImage = new BitmapImage(_session.ImgPaths.ImageSprites[ImgName.Dirt]);
            //_cobbleImage = new BitmapImage(_session.ImgPaths.ImageSprites[ImgName.Cobblestone]);
            //_playerAvatar = new BitmapImage(_session.ImgPaths.ImageSprites[ImgName.Player]);
            //_rockImage = new BitmapImage(_session.ImgPaths.ImageSprites[ImgName.Rock]);

            //_sprites.Add(ImgName.Dirt, _groundImage);
            //_sprites.Add(ImgName.Cobblestone, _cobbleImage);
            //_sprites.Add(ImgName.Player, _playerAvatar);
            //_sprites.Add(ImgName.Rock, _rockImage);

            //_colors.Add(ImgName.Dirt, Brushes.Brown);
            //_colors.Add(ImgName.Cobblestone, Brushes.LightGray);
            //_colors.Add(ImgName.Player, Brushes.Green);
            //_colors.Add(ImgName.Rock, Brushes.DarkGray);

            _rectangle = new Rect();
        }

        public void UpdateGraphics(object sender, EventArgs e)
        {
            // redrawing a bitmap image should be faster
            bitmap.Clear();
            var drawingContext = _drawingVisual.RenderOpen();

            DrawBackground(drawingContext);

            // need to update the camera to know what is visible
            _session.UpdateGraphics();

            DrawSceneObjects(drawingContext);

            // focus point always rendered at the center of the scene
            DrawGraphicsComponent(_currentScene.EntityManager.GetGraphicsComponent(_currentScene.PlayerEntity), _currentCamera.XOffset, _currentCamera.YOffset, _currentScene.Transform.ScaleX, _currentScene.Transform.ScaleY, drawingContext, true);
            //DrawGraphicsComponent(_currentScene.PlayerGraphicsComponent, _currentCamera.XOffset, _currentCamera.YOffset, drawingContext);

            drawingContext.Close();
            bitmap.Render(_drawingVisual);
        }

        private void DrawSceneObjects(DrawingContext drawingContext)
        {
            Vector2 focusPos = _currentScene.Transform.Position;
            int index = 0;
            List<ITransformComponent> transformComponents = _currentCamera.VisibleTransforms;

            foreach (var item in _currentCamera.VisibleObjects)
            {
                // conversion of logical coordinates to graphical ones
                float graphicX = CalculateGraphicsCoordinate(transformComponents[index].Position.X, _currentCamera.XOffset, focusPos.X);
                float graphicY = CalculateGraphicsCoordinate(transformComponents[index].Position.Y, _currentCamera.YOffset, focusPos.Y);

                DrawGraphicsComponent(item, graphicX, graphicY, transformComponents[index].ScaleX, transformComponents[index].ScaleY, drawingContext, true);
                index++;
            }
        }

        private float CalculateGraphicsCoordinate(float logicalPosition, float offset, float focusPos)
        {
            return logicalPosition < focusPos ? offset - (focusPos - logicalPosition) : offset + (logicalPosition - focusPos);
        }

        private void DrawGraphicsComponent(IGraphicsComponent item, float graphicX, float graphicY, float width, float height, DrawingContext drawingContext, bool textureMode)
        {
            _rectangle.X = graphicX;
            _rectangle.Y = graphicY;
            _rectangle.Width = width;
            _rectangle.Height = height;
            if (textureMode)
                drawingContext.DrawImage(_imageResourceManager.GetImage(item.CurrentImageName), _rectangle);
            else
                drawingContext.DrawRectangle(_imageResourceManager.GetColor(item.CurrentImageName), null, _rectangle);

        }

        private void DrawBackground(DrawingContext drawingContext)
        {
            // to have a black background as a default
            _rectangle.X = 0;
            _rectangle.Y = 0;
            _rectangle.Width = _xRes;
            _rectangle.Height = _yRes;
            drawingContext.DrawRectangle(Brushes.Black, null, _rectangle);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                _session.State.TogglePause();
                if (!_session.State.IsRunning())
                {
                    ShowPauseOverlay();
                }
                else
                {
                    GameCanvas.Children.RemoveAt(GameCanvas.Children.Count - 1);
                    GameCanvas.Children.RemoveAt(GameCanvas.Children.Count - 1);
                }
            }

            _inputHandler.HandleKeyPressed(e.Key);
            _session.HandleUserInput();
        }
        
        private void ShowPauseOverlay()
        {
            // show "Pause" overlay
            Rectangle overlay = new Rectangle();
            Color testColor = Color.FromArgb(172, 172, 172, 255);
            overlay.Width = _xRes;
            overlay.Height = _yRes;
            overlay.Fill = new SolidColorBrush(testColor);
            GameCanvas.Children.Add(overlay);

            TextBlock textBlock = new TextBlock();

            textBlock.Text = "PAUSED";

            textBlock.FontSize = 60;

            textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

            Canvas.SetLeft(textBlock, 350);

            Canvas.SetTop(textBlock, 250);

            GameCanvas.Children.Add(textBlock);
        }
        
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _inputHandler.HandleKeyReleased(e.Key);
            _session.HandleUserInput();
        }
    }
}

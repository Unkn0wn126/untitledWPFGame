#define TRACE
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Scenes;
using ResourceManagers.Images;
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

        // get the sprites and images from here
        private ImageResourceManager _imageResourceManager;

        private ImagePaths _imagePaths;

        private bool _isTextureModeOn;

        private IScene _currentScene;
        private ICamera _currentCamera;

        Rect _rectangle;

        private int _xRes;
        private int _yRes;

        public MainWindow(ImagePaths imagePaths, GameInput gameInputHandler, IGame session, int xRes, int yRes)
        {
            _imagePaths = imagePaths;
            _isTextureModeOn = true;

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
            _imageResourceManager = new ImageResourceManager(_imagePaths);

            _rectangle = new Rect();
        }

        public void UpdateGraphics(object sender, EventArgs e)
        {
            if (_session.State.CurrentState == Engine.Models.GameStateMachine.GameState.LOADING)
            {
                ShowLoadingOverlay();
            }
            else if (_session.State.IsRunning())
            {
                // redrawing a bitmap image should be faster
                bitmap.Clear();
                var drawingContext = _drawingVisual.RenderOpen();

                DrawBackground(drawingContext);

                // need to update the camera to know what is visible
                _session.UpdateGraphics();

                DrawSceneObjects(drawingContext);

                drawingContext.Close();
                bitmap.Render(_drawingVisual);
            }

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

                DrawGraphicsComponent(item, graphicX, graphicY, transformComponents[index].ScaleX, transformComponents[index].ScaleY, drawingContext, _isTextureModeOn);
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
                if (_session.State.IsPaused())
                {
                        ShowPauseOverlay();
                }
                else
                {
                    GameCanvas.Children.RemoveAt(GameCanvas.Children.Count - 1);
                    GameCanvas.Children.RemoveAt(GameCanvas.Children.Count - 1);
                }
            }

            if (e.Key == Key.F)
            {
                _isTextureModeOn = !_isTextureModeOn;
            }

            _inputHandler.HandleKeyPressed(e.Key);
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
        
        private void ShowLoadingOverlay()
        {
            // show "Pause" overlay
            Rectangle overlay = new Rectangle();
            Color testColor = Color.FromArgb(255, 0, 0, 0);
            overlay.Width = _xRes;
            overlay.Height = _yRes;
            overlay.Fill = new SolidColorBrush(testColor);
            GameCanvas.Children.Add(overlay);

            TextBlock textBlock = new TextBlock();

            textBlock.Text = "LOADING";

            textBlock.FontSize = 60;

            textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

            Canvas.SetLeft(textBlock, 350);

            Canvas.SetTop(textBlock, 250);

            GameCanvas.Children.Add(textBlock);
        }
        
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _inputHandler.HandleKeyReleased(e.Key);
        }
    }
}

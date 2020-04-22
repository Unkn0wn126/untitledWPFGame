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
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Engine.Saving;
using WPFGame.Saving;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using Engine.Models.Factories.Scenes;

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

        // for the context of window size
        private int _xRes;
        private int _yRes;

        private int _sizeMultiplier;

        private Configuration _gameConfiguration;
        private string _configPath = @"./Configuration/GameConfig.xml";

        public MainWindow(ImagePaths imagePaths, GameInput gameInputHandler, IGame session)
        {
            _imagePaths = imagePaths;
            _isTextureModeOn = true;

            InitializeComponent();

            LoadConfig();

            _session = session;
            InitializeImages();
            //InitializeCaching();

            _inputHandler = new UserInputHandler(gameInputHandler, _gameConfiguration);

            // to get shorter routes to frequently used objects
            _currentScene = _session.CurrentScene;
            _currentCamera = _currentScene.SceneCamera;

            SetWindowSize();

            // this is what everything renders to
            bitmap = new RenderTargetBitmap(_gameConfiguration.Width, _gameConfiguration.Height, 96, 96, PixelFormats.Pbgra32);
            GameImage.Source = bitmap;
        }

        private void LoadConfig()
        {
            _gameConfiguration = new Configuration();
            using (FileStream fs2 = new FileStream(_configPath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(_gameConfiguration.GetType());
                _gameConfiguration = serializer.Deserialize(fs2) as Configuration;
            }
        }

        private void SaveCurrentConfig()
        {
            using (FileStream fs = new FileStream(_configPath, FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(_gameConfiguration.GetType());

                serializer.Serialize(fs, _gameConfiguration);
            }
        }

        private void SetWindowSize()
        {
            _xRes = _gameConfiguration.Width;
            _yRes = _gameConfiguration.Height;

            _sizeMultiplier = (int)Math.Ceiling(_xRes / 16f);

            Width = _xRes;
            Height = _yRes;

            GameCanvas.Width = _xRes;
            GameCanvas.Height = _yRes;
            GameImage.Width = _xRes;
            GameImage.Height = _yRes;

            bitmap = new RenderTargetBitmap(_xRes, _yRes, 96, 96, PixelFormats.Pbgra32);
            GameImage.Source = bitmap;

            _currentCamera.UpdateSize(_xRes, _yRes);

            WindowStyle = _gameConfiguration.WindowStyle == 0 ? WindowStyle.SingleBorderWindow : WindowStyle.None;
            WindowState = _gameConfiguration.WindowState == 0 ? WindowState.Normal : WindowState.Maximized;
        }

        private void InitializeCaching()
        {
            var cache = new BitmapCache();
            cache.RenderAtScale = 0.5; // render at half the resolution for now
            cache.SnapsToDevicePixels = false;
            _drawingVisual.CacheMode = cache;
        }

        /// <summary>
        /// Loads image resources
        /// </summary>
        private void InitializeImages()
        {
            _imageResourceManager = new ImageResourceManager(_imagePaths);

            _rectangle = new Rect();
        }

        /// <summary>
        /// Redraws the scene
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateGraphics(object sender, EventArgs e)
        {
            if (_session.State.IsLoading() && !loadingOverlayActive)
            {
                ShowLoadingOverlay();
            }
            else if (_session.State.IsRunning())
            {
                if (loadingOverlayActive)
                {
                    RemoveLoadingOverlay();
                    loadingOverlayActive = false;
                }
                _currentCamera = _session.CurrentScene.SceneCamera;
                _currentScene = _session.CurrentScene;
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

        /// <summary>
        /// Renders all visible entities in a scene
        /// </summary>
        /// <param name="drawingContext"></param>
        private void DrawSceneObjects(DrawingContext drawingContext)
        {
            Vector2 focusPos = _currentScene.PlayerTransform.Position;
            int index = 0;
            List<ITransformComponent> transformComponents = _currentCamera.VisibleTransforms;

            foreach (var item in _currentCamera.VisibleObjects)
            {
                // conversion of logical coordinates to graphical ones
                float graphicX = CalculateGraphicsCoordinate(transformComponents[index].Position.X, _currentCamera.XOffset, focusPos.X);
                float graphicY = CalculateGraphicsCoordinate(transformComponents[index].Position.Y, _currentCamera.YOffset, focusPos.Y);

                DrawGraphicsComponent(item, graphicX, graphicY, transformComponents[index].ScaleX * _sizeMultiplier, transformComponents[index].ScaleY * _sizeMultiplier, drawingContext, _isTextureModeOn);
                index++;
            }
        }

        /// <summary>
        /// Computes graphics coordinates based on the real ones
        /// </summary>
        /// <param name="logicalPosition"></param>
        /// <param name="offset"></param>
        /// <param name="focusPos"></param>
        /// <returns></returns>
        private float CalculateGraphicsCoordinate(float logicalPosition, float offset, float focusPos)
        {
            return logicalPosition < focusPos ? offset - ((focusPos - logicalPosition) * _sizeMultiplier) : offset + ((logicalPosition - focusPos) * _sizeMultiplier);
        }

        /// <summary>
        /// Draws a graphics component on the given position
        /// </summary>
        /// <param name="item"></param>
        /// <param name="graphicX"></param>
        /// <param name="graphicY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="drawingContext"></param>
        /// <param name="textureMode"></param>
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

        /// <summary>
        /// Draws a black background on the "scene"
        /// </summary>
        /// <param name="drawingContext"></param>
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
            // Temporairly here to show pause menu
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
            switch (e.Key)
            {
                case Key.D1:
                    LoadConfig();
                    SetWindowSize();
                    _inputHandler.UpdateConfiguration(_gameConfiguration);
                    break;
                case Key.NumPad1:
                    ShowSaveDialog();
                    break;                
                case Key.NumPad2:
                    ShowLoadDialog();
                    break;
            }

            if (e.Key == Key.F)
            {
                _isTextureModeOn = !_isTextureModeOn;
            }

            _inputHandler.HandleKeyPressed(e.Key);
        }

        private void ShowSaveDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".save";
            dialog.Filter = "Save Files (*.save)|*.save";
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                Save save = new Save();
                var sceneManager = _session.SceneManager;

                save.Scenes = sceneManager.MetaScenes;
                save.PlayerPosX = sceneManager.CurrentScene.PlayerTransform.Position.X;
                save.PlayerPosY = sceneManager.CurrentScene.PlayerTransform.Position.Y;
                save.PlayerSizeX = sceneManager.CurrentScene.PlayerTransform.ScaleX;
                save.PlayerSizeY = sceneManager.CurrentScene.PlayerTransform.ScaleY;
                save.PlayerZIndex = sceneManager.CurrentScene.PlayerTransform.ZIndex;
                SaveGame(filename, save);
            }
        }

        private void ShowLoadDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".save";
            dialog.Filter = "Save Files (*.save)|*.save";
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                Save save = SaveFileManager.LoadGame(filename);

                _session.State.CurrentState = Engine.Models.GameStateMachine.GameState.LOADING;
                _session.SceneManager.MetaScenes = save.Scenes;
                _session.CurrentScene = _session.SceneManager.LoadNextScene();
                _currentScene = _session.CurrentScene;
                _currentCamera = _session.CurrentScene.SceneCamera;
                _session.UpdateProcessorContext();
                _session.State.CurrentState = Engine.Models.GameStateMachine.GameState.RUNNING;
            }


        }

        private void SaveGame(string path, Save save)
        {
            SaveFileManager.SaveGame(path, save);
        }
        
        private void ShowPauseOverlay()
        {
            // hopefully a separate XML component in the future
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

        private bool loadingOverlayActive = false;

        private void RemoveLoadingOverlay()
        {
            GameCanvas.Children.RemoveAt(GameCanvas.Children.Count - 1);
            GameCanvas.Children.RemoveAt(GameCanvas.Children.Count - 1);
        }
        
        private void ShowLoadingOverlay()
        {
            // Hopefully a separate XML component in the future
            // show "Loading" overlay
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
            loadingOverlayActive = true;
        }
        
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _inputHandler.HandleKeyReleased(e.Key);
        }
    }
}

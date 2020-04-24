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
using System.IO;
using System.Xml.Serialization;
using Engine.Saving;
using WPFGame.Saving;
using Microsoft.Win32;
using WPFGame.UI.HUD;
using WPFGame.UI.MainMenu;
using System.Runtime.Serialization.Formatters.Binary;
using Engine.Models.Factories;
using Engine.Models.Factories.Scenes;
using WPFGame.UI.PauseMenu;

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

        private MapPlayerInfo _mapHUD;
        private MainMenu _mainMenu;
        private PauseMenu _pauseMenu;

        Rect _rectangle;

        // for the context of window size
        private int _xRes;
        private int _yRes;

        private int _sizeMultiplier;

        private Configuration _gameConfiguration;
        private string _configPath = @"./Configuration/GameConfig.xml";

        private ProcessMenuButtonClick _pauseResumeAction;
        private ProcessMenuButtonClick _pauseLoadMainAction;
        private ProcessMenuButtonClick _pauseQuitAction;
        private ProcessSettingsApplyButtonClick _pauseApplyAction;


        private ProcessSaveActionButtonClick _saveGameAction;
        private ProcessSaveActionButtonClick _loadGameAction;

        private Uri _savesPath;

        public MainWindow(ImagePaths imagePaths, GameInput gameInputHandler, IGame session)
        {
            _imagePaths = imagePaths;
            _isTextureModeOn = true;

            InitializeComponent();

            LoadConfig();

            _session = session;
            InitializeImages();
            //InitializeCaching();

            InitializeSaveMenusActions();

            _inputHandler = new UserInputHandler(gameInputHandler, _gameConfiguration);

            // this is what everything renders to
            bitmap = new RenderTargetBitmap(_gameConfiguration.Resolution.Width, _gameConfiguration.Resolution.Height, 96, 96, PixelFormats.Pbgra32);
            GameImage.Source = bitmap;

            _mapHUD = new MapPlayerInfo();
            _mainMenu = new MainMenu(new ProcessMenuButtonClick(CloseGame), new ProcessMenuButtonClick(InitializeGame), new ProcessSettingsApplyButtonClick(UpadteCurrentConfig), _gameConfiguration, _loadGameAction, _savesPath);
            InitializePauseMenuActions();
            _pauseMenu = new PauseMenu(_pauseResumeAction, _pauseLoadMainAction, _pauseQuitAction, _pauseApplyAction, _gameConfiguration, _loadGameAction, _saveGameAction, _savesPath);

            SetWindowSize();

            LoadMainMenu();
        }

        private void InitializeSaveMenusActions()
        {
            _savesPath = new Uri(@"./Saves", UriKind.Relative);
            _saveGameAction = new ProcessSaveActionButtonClick(SaveGame);
            _loadGameAction = new ProcessSaveActionButtonClick(LoadGame);
        }

        private void InitializePauseMenuActions()
        {
            _pauseResumeAction = new ProcessMenuButtonClick(TogglePauseMenu);
            _pauseLoadMainAction = new ProcessMenuButtonClick(LoadMainMenu);
            _pauseQuitAction = new ProcessMenuButtonClick(CloseGame);
            _pauseApplyAction = new ProcessSettingsApplyButtonClick(UpadteCurrentConfig);
        }

        private void LoadMainMenu()
        {
            if (GameGrid.Children.Contains(_pauseMenu))
            {
                GameGrid.Children.Remove(_pauseMenu);
            }
            if (GameGrid.Children.Contains(_mapHUD))
            {
                GameGrid.Children.Remove(_mapHUD);
            }
            if (!GameGrid.Children.Contains(_mainMenu))
            {
                GameGrid.Children.Add(_mainMenu);
            }
        }

        private void TogglePauseMenu()
        {
            _session.State.TogglePause();
            ToggleMapHUD();
            if (!GameGrid.Children.Contains(_pauseMenu))
            {
                GameGrid.Children.Add(_pauseMenu);
            }
            else
            {
                GameGrid.Children.Remove(_pauseMenu);
                _pauseMenu.RestoreDefaultState();
            }
        }

        private void ToggleMapHUD()
        {
            if (!GameGrid.Children.Contains(_mapHUD))
            {
                GameGrid.Children.Add(_mapHUD);
            }
            else
            {
                GameGrid.Children.Remove(_mapHUD);
            }
        }

        private void InitializeGame()
        {
            Random rnd = new Random();
            int val = rnd.Next(10, 100);

            // Scene generation should take place elsewhere
            List<byte[]> metaScenes = new List<byte[]>();
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] current;
                var binaryFormatter = new BinaryFormatter();
                for (int i = 0; i < 10; i++)
                {
                    MetaScene metaScene = SceneFactory.CreateMetaScene(null, val, val, 1, 5);
                    binaryFormatter.Serialize(stream, metaScene);
                    current = stream.ToArray();
                    metaScenes.Add(current);
                    stream.SetLength(0);
                }
            }

            _session.InitializeGame(metaScenes);
            _session.State.CurrentState = Engine.Models.GameStateMachine.GameState.Running;
            SetWindowSize();
            UpdateSceneContext();
            RemoveOverlay(_mainMenu);
            ToggleMapHUD();
        }

        private void RemoveOverlay(UserControl control)
        {
            if (GameGrid.Children.Contains(control))
            {
                GameGrid.Children.Remove(control);
            }
        }

        private void CloseGame()
        {
            Close();
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

        private void UpadteCurrentConfig(Configuration newConfig)
        {
            _gameConfiguration = newConfig;
            SetWindowSize();
            _inputHandler.UpdateConfiguration(_gameConfiguration);
            SaveCurrentConfig();
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
            ConfigResolution resolution = new ConfigResolution();
            _xRes = _gameConfiguration.Resolution.Width;
            _yRes = _gameConfiguration.Resolution.Height;

            _sizeMultiplier = (int)Math.Ceiling(_xRes / 16f);

            Width = _xRes;
            Height = _yRes;

            GameCanvas.Width = _xRes;
            GameCanvas.Height = _yRes;
            GameImage.Width = _xRes;
            GameImage.Height = _yRes;

            bitmap = new RenderTargetBitmap(_xRes, _yRes, 96, 96, PixelFormats.Pbgra32);
            GameImage.Source = bitmap;

            _currentCamera?.UpdateSize(_xRes, _yRes);

            WindowStyle = _gameConfiguration.WindowStyle == 0 ? WindowStyle.SingleBorderWindow : WindowStyle.None;
            WindowState = _gameConfiguration.WindowState == 0 ? WindowState.Normal : WindowState.Maximized;
        }

        private void UpdateSceneContext()
        {
            _currentCamera = _session.SceneManager.CurrentScene.SceneCamera;
            _currentScene = _session.SceneManager.CurrentScene;
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
                if (GameGrid.Children.Contains(_mainMenu))
                {
                    GameGrid.Children.Remove(_mainMenu);
                }
                if (GameGrid.Children.Contains(_pauseMenu))
                {
                    GameGrid.Children.Remove(_pauseMenu);
                    _pauseMenu.RestoreDefaultState();
                }

                if (loadingOverlayActive)
                {
                    RemoveLoadingOverlay();
                    loadingOverlayActive = false;
                }

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
            Vector2 focusPos = _currentCamera.FocusPoint.Position;
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
                TogglePauseMenu();
            }
            switch (e.Key)
            {
                case Key.D1:
                    LoadConfig();
                    SetWindowSize();
                    _inputHandler.UpdateConfiguration(_gameConfiguration);
                    break;
            }

            if (e.Key == Key.F)
            {
                _isTextureModeOn = !_isTextureModeOn;
            }

            _inputHandler.HandleKeyPressed(e.Key);
        }

        private void SaveGame(Uri path)
        {
                string filename = path.ToString();
                Save save = new Save();
                var sceneManager = _session.SceneManager;

                save.Scenes = sceneManager.GetScenesToSave();
                save.CurrentIndex = sceneManager.CurrentIndex;
                SaveGame(filename, save);
        }

        private void LoadGame(Uri path)
        {
                string filename = path.ToString();
                Save save = SaveFileManager.LoadGame(filename);

                _session.State.CurrentState = Engine.Models.GameStateMachine.GameState.Loading;
                _session.InitializeGame(save.Scenes);
                _currentScene = _session.SceneManager.CurrentScene;
                _currentCamera = _session.SceneManager.CurrentScene.SceneCamera;
                _session.State.CurrentState = Engine.Models.GameStateMachine.GameState.Running;
        }

        private void SaveGame(string path, Save save)
        {
            SaveFileManager.SaveGame(path, save);
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

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
using System.Windows.Threading;
using WPFGame.UI.LoadingScreen;
using System.Diagnostics;

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
        private LoadingScreen _loadingScreen;

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

        private DispatcherTimer _updateTimer;

        private bool _shouldDisplayLoadingOverlay = false;
        private bool _shouldUpdateSceneContext = false;

        public MainWindow(ImagePaths imagePaths, GameInput gameInputHandler, IGame session)
        {
            _imagePaths = imagePaths;
            _isTextureModeOn = true;

            InitializeComponent();

            LoadConfig();

            _session = session;
            InitializeImages();
            //InitializeCaching();

            _session.SceneManager.SceneChangeStarted += delegate { _shouldDisplayLoadingOverlay = true; };
            _session.SceneManager.SceneChangeFinished += delegate { _shouldUpdateSceneContext = true; };

            _loadingScreen = new LoadingScreen();

            _updateTimer = new DispatcherTimer();
            _updateTimer.Interval = new TimeSpan(0, 0, 0, 0, 17);
            _updateTimer.Tick += UpdateGraphics;

            _updateTimer.Start();

            InitializeSaveMenusActions();

            _inputHandler = new UserInputHandler(gameInputHandler, _gameConfiguration);

            // this is what everything renders to
            bitmap = new RenderTargetBitmap(_gameConfiguration.Resolution.Width, _gameConfiguration.Resolution.Height, 96, 96, PixelFormats.Pbgra32);
            GameImage.Source = bitmap;

            _mapHUD = new MapPlayerInfo();
            _mainMenu = new MainMenu(new ProcessMenuButtonClick(CloseGame), new ProcessMenuButtonClick(GenerateGame), new ProcessSettingsApplyButtonClick(UpadteCurrentConfig), _gameConfiguration, _loadGameAction, _savesPath);
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
            //ToggleMapHUD();
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

        private void GenerateGame()
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

            InitializeGame(metaScenes);
        }

        private void InitializeGame(List<byte[]> metaScenes)
        {
            ShowLoadingOverlay();

            _session.InitializeGame(metaScenes);
            SetWindowSize();

            //ToggleMapHUD();

            if (_session.SceneManager.CurrentScene != null)
            {
                _currentCamera?.UpdateFocusPoint(_session.SceneManager.CurrentScene.EntityManager.GetComponentOfType<ITransformComponent>(_session.SceneManager.CurrentScene.PlayerEntity));
            }
        }

        private void RemoveOverlay(UserControl control)
        {
            if (GameGrid.Children.Contains(control))
            {
                GameGrid.Children.Remove(control);
            }
            if (control == _pauseMenu)
            {
                _pauseMenu.RestoreDefaultState();
            }
            else if (control == _mainMenu)
            {
                _mainMenu.RestoreDefaultState();
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

            _gameConfiguration.PerformDuplicateCheck();
        }

        private void UpadteCurrentConfig(Configuration newConfig)
        {
            _gameConfiguration = new Configuration(newConfig);
            _gameConfiguration.PerformDuplicateCheck();
            _mainMenu.UpdateConfig(_gameConfiguration);
            _pauseMenu.UpdateConfig(_gameConfiguration);
            SetWindowSize();
            _inputHandler.UpdateConfiguration(_gameConfiguration);
            SaveCurrentConfig();
            if (_session.SceneManager.CurrentScene != null)
            {
                _currentCamera?.UpdateFocusPoint(_session.SceneManager.CurrentScene.EntityManager.GetComponentOfType<ITransformComponent>(_session.SceneManager.CurrentScene.PlayerEntity));
            }
        }

        private void SaveCurrentConfig()
        {
            using (FileStream fs = new FileStream(_configPath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(_gameConfiguration.GetType());

                serializer.Serialize(fs, _gameConfiguration);
            }
        }

        private void SetWindowSize()
        {
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

            UpdateSceneContext();

            _currentCamera?.UpdateSize(_xRes, _yRes);


            WindowStyle = _gameConfiguration.WindowStyle == 0 ? WindowStyle.SingleBorderWindow : WindowStyle.None;
            WindowState = _gameConfiguration.WindowState == 0 ? WindowState.Normal : WindowState.Maximized;
        }

        private void UpdateSceneContext()
        {
            RemoveOverlay(_loadingScreen);
            _currentCamera = _session.SceneManager.CurrentScene?.SceneCamera;
            _currentScene = _session.SceneManager.CurrentScene;

            if (_session.SceneManager.CurrentScene != null)
            {
                _currentCamera.UpdateFocusPoint(_session.SceneManager.CurrentScene.EntityManager.GetComponentOfType<ITransformComponent>(_session.SceneManager.CurrentScene.PlayerEntity));
            }

            _shouldUpdateSceneContext = false;
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
            if (_shouldDisplayLoadingOverlay)
            {
                ShowLoadingOverlay();
            }

            if (_shouldUpdateSceneContext)
            {
                UpdateSceneContext();
            }
            if (_session.State.IsRunning())
            {
                // redrawing a bitmap image should be faster
                bitmap.Clear();
                var drawingContext = _drawingVisual.RenderOpen();

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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Temporairly here to show pause menu
            if (e.Key == Key.Escape)
            {
                if (!GameGrid.Children.Contains(_mainMenu))
                {
                    TogglePauseMenu();
                }
            }

            if (e.Key == Key.F)
            {
                _isTextureModeOn = !_isTextureModeOn;
            }

            _inputHandler.HandleKeyPressed(e.Key);
        }

        /// <summary>
        /// Saves the current game
        /// </summary>
        /// <param name="path"></param>
        private void SaveGame(Uri path)
        {
            string filename = path.ToString();
            Save save = new Save();
            var sceneManager = _session.SceneManager;

            save.Scenes = sceneManager.GetScenesToSave();
            save.CurrentIndex = sceneManager.CurrentIndex;
            SaveFileManager.SaveGame(filename, save);
        }

        /// <summary>
        /// Loads a saved game
        /// </summary>
        /// <param name="path"></param>
        private void LoadGame(Uri path)
        {
            string filename = path.ToString();
            Save save = SaveFileManager.LoadGame(filename);

            InitializeGame(save.Scenes);
        }
        
        private void ShowLoadingOverlay()
        {
            if (!GameGrid.Children.Contains(_loadingScreen))
            {
                GameGrid.Children.Add(_loadingScreen);
            }
            RemoveOverlay(_mainMenu);
            RemoveOverlay(_pauseMenu);

            _shouldDisplayLoadingOverlay = false;
        }
        
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _inputHandler.HandleKeyReleased(e.Key);
        }
    }
}

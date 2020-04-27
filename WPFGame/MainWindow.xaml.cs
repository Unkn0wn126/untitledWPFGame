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
using WPFGame.ResourceManagers;
using System.IO;
using System.Xml.Serialization;
using Engine.Saving;
using WPFGame.Saving;
using WPFGame.UI.HUD;
using WPFGame.UI.MainMenu;
using System.Runtime.Serialization.Formatters.Binary;
using Engine.Models.Factories;
using Engine.Models.Factories.Scenes;
using WPFGame.UI.PauseMenu;
using System.Windows.Threading;
using WPFGame.UI.LoadingScreen;

namespace WPFGame
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IGame _session;

        private readonly UserInputHandler _inputHandler;
        private readonly GameInput _gameInput;

        // image to render to
        private RenderTargetBitmap bitmap;

        // needed for rendering
        private DrawingVisual _drawingVisual = new DrawingVisual();

        // get the sprites and images from here
        private ImageResourceManager _imageResourceManager;

        private ImagePaths _imagePaths;

        private DispatcherTimer _updateTimer;

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
        private readonly string _configPath; 

        private ProcessMenuButtonClick _pauseResumeAction;
        private ProcessMenuButtonClick _pauseLoadMainAction;
        private ProcessMenuButtonClick _pauseQuitAction;
        private ProcessSettingsApplyButtonClick _pauseApplyAction;


        private ProcessSaveActionButtonClick _saveGameAction;
        private ProcessSaveActionButtonClick _loadGameAction;

        private readonly Uri _savesPath;


        public MainWindow(ImagePaths imagePaths, GameInput gameInputHandler, IGame session)
        {
            _gameInput = gameInputHandler;
            _configPath = @"./Configuration/GameConfig.xml";
            _savesPath = new Uri(@"./Saves", UriKind.Relative);
            _imagePaths = imagePaths;
            _isTextureModeOn = true;
            _session = session;

            InitializeComponent();
            LoadConfig();
            _inputHandler = new UserInputHandler(gameInputHandler, _gameConfiguration);

            FinishInitialization();
        }

        /// <summary>
        /// Creates a new instance of image
        /// to render the game to
        /// </summary>
        private void InitializeRenderImage()
        {
            // this is what everything renders to
            bitmap = new RenderTargetBitmap(_gameConfiguration.Resolution.Width, 
                _gameConfiguration.Resolution.Height, 96, 96, PixelFormats.Pbgra32);

            GameImage.Source = bitmap;
        }

        /// <summary>
        /// Takes care of the rest of
        /// the initialization process
        /// </summary>
        private void FinishInitialization()
        {
            InitializeRenderImage();

            InitializeImages();

            _session.SceneManager.SceneChangeStarted += ShowLoadingOverlay;
            _session.SceneManager.SceneChangeFinished += UpdateSceneContext;
            _session.SceneManager.GameWon += ShowWinnerOverlay;

            _loadingScreen = new LoadingScreen();

            InitializeGameRenderLoop();

            InitializeSaveMenusActions();

            InitializeOverlays();

            SetWindowSize();

            LoadMainMenu();
        }

        /// <summary>
        /// Initializes main menu
        /// </summary>
        private void InitializeMainMenu()
        {
            ProcessMenuButtonClick closeGameAction = new ProcessMenuButtonClick(CloseGame);
            ProcessMenuButtonClick generateGameAction = new ProcessMenuButtonClick(GenerateGame);
            ProcessSettingsApplyButtonClick updateCurrentConfigAction = new ProcessSettingsApplyButtonClick(UpadteCurrentConfig);
            _mainMenu = new MainMenu(closeGameAction, generateGameAction, updateCurrentConfigAction, _gameConfiguration, _loadGameAction, _savesPath);
        }

        /// <summary>
        /// Initializes the overlays
        /// </summary>
        private void InitializeOverlays()
        {
            _mapHUD = new MapPlayerInfo();
            InitializeMainMenu();
            InitializePauseMenu();
        }

        /// <summary>
        /// Initializes the main game renderer
        /// </summary>
        private void InitializeGameRenderLoop()
        {
            _updateTimer = new DispatcherTimer();
            _updateTimer.Interval = new TimeSpan(0, 0, 0, 0, 17);
            _updateTimer.Tick += UpdateGraphics;

            _updateTimer.Start();
        }

        /// <summary>
        /// Initializes the save menu actions
        /// </summary>
        private void InitializeSaveMenusActions()
        {
            _saveGameAction = new ProcessSaveActionButtonClick(SaveGame);
            _loadGameAction = new ProcessSaveActionButtonClick(LoadGame);
        }

        /// <summary>
        /// Initializes the pause menu actions
        /// </summary>
        private void InitializePauseMenu()
        {
            _pauseResumeAction = new ProcessMenuButtonClick(TogglePauseMenu);
            _pauseLoadMainAction = new ProcessMenuButtonClick(LoadMainMenu);
            _pauseQuitAction = new ProcessMenuButtonClick(CloseGame);
            _pauseApplyAction = new ProcessSettingsApplyButtonClick(UpadteCurrentConfig);

            _pauseMenu = new PauseMenu(_pauseResumeAction, _pauseLoadMainAction, 
                _pauseQuitAction, _pauseApplyAction, _gameConfiguration, 
                _loadGameAction, _saveGameAction, _savesPath);
        }

        /// <summary>
        /// Loads the main menu
        /// </summary>
        private void LoadMainMenu()
        {
            RemoveOverlay(_pauseMenu);
            RemoveOverlay(_mapHUD);
            if (!GameGrid.Children.Contains(_mainMenu))
            {
                GameGrid.Children.Add(_mainMenu);
            }
        }

        /// <summary>
        /// Toggles the pause menu overlay
        /// </summary>
        private void TogglePauseMenu()
        {
            _session.State.TogglePause();
            //ToggleMapHUD();
            if (!GameGrid.Children.Contains(_pauseMenu))
            {
                GameGrid.Children.Add(_pauseMenu);
            }
            else
                RemoveOverlay(_pauseMenu);
        }

        /// <summary>
        /// Toggles the player HUD
        /// on the map
        /// </summary>
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

        /// <summary>
        /// Generates a new game
        /// </summary>
        private void GenerateGame()
        {
            ShowLoadingOverlay();
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

            InitializeGame(metaScenes, 0);
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        /// <param name="metaScenes"></param>
        private void InitializeGame(List<byte[]> metaScenes, int currentIndex)
        {
            _session.InitializeGame(metaScenes, currentIndex);
        }

        /// <summary>
        /// Removes the given GUI overlay
        /// </summary>
        /// <param name="control"></param>
        private void RemoveOverlay(UserControl control)
        {
            if (GameGrid.Children.Contains(control))
            {
                GameGrid.Children.Remove(control);
            }
            ResetMenus(control);
        }


        /// <summary>
        /// Resets the states of menus
        /// </summary>
        /// <param name="control"></param>
        private void ResetMenus(UserControl control)
        {
            if (control == _pauseMenu)
            {
                _pauseMenu.RestoreDefaultState();
            }
            else if (control == _mainMenu)
            {
                _mainMenu.RestoreDefaultState();
            }
        }

        /// <summary>
        /// Allows to close the game
        /// via a delegate
        /// </summary>
        private void CloseGame()
        {
            Close();
        }

        /// <summary>
        /// Loads the configuration
        /// from a configuration file
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                _gameConfiguration = new Configuration();
                using (FileStream fs2 = new FileStream(_configPath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(_gameConfiguration.GetType());
                    _gameConfiguration = serializer.Deserialize(fs2) as Configuration;
                }

                _gameConfiguration.PerformDuplicateCheck();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Updates the current configuration
        /// </summary>
        /// <param name="newConfig"></param>
        private void UpadteCurrentConfig(Configuration newConfig)
        {
            UpdateMenuConfiguration(newConfig);
            SetWindowSize();
            _inputHandler.UpdateConfiguration(_gameConfiguration);
            SaveCurrentConfig();
            UpdateSceneContext();
        }

        /// <summary>
        /// Updates the current configuration
        /// and menu configuration as well
        /// </summary>
        /// <param name="newConfig"></param>
        private void UpdateMenuConfiguration(Configuration newConfig)
        {
            _gameConfiguration = new Configuration(newConfig);
            _gameConfiguration.PerformDuplicateCheck();
            _mainMenu.UpdateConfig(_gameConfiguration);
            _pauseMenu.UpdateConfig(_gameConfiguration);
        }

        /// <summary>
        /// Saves the new configuration to the config file
        /// </summary>
        private void SaveCurrentConfig()
        {
            try
            {
                using (FileStream fs = new FileStream(_configPath, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(_gameConfiguration.GetType());

                    serializer.Serialize(fs, _gameConfiguration);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Updates the state of the game window
        /// </summary>
        private void SetWindowSize()
        {
            UpdateGameRenderSize(_xRes, _yRes);

            bitmap = new RenderTargetBitmap(_xRes, _yRes, 96, 96, PixelFormats.Pbgra32);
            GameImage.Source = bitmap;

            UpdateCameraFocusPoint();

            _currentCamera?.UpdateSize(_xRes, _yRes);

            WindowStyle = _gameConfiguration.WindowStyle == 0 ? WindowStyle.SingleBorderWindow : WindowStyle.None;
            WindowState = _gameConfiguration.WindowState == 0 ? WindowState.Normal : WindowState.Maximized;
        }

        /// <summary>
        /// Updates the renderable size info
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void UpdateGameRenderSize(int width, int height)
        {
            _xRes = _gameConfiguration.Resolution.Width;
            _yRes = _gameConfiguration.Resolution.Height;

            _sizeMultiplier = (int)Math.Ceiling(_xRes / 16f);

            Width = _xRes;
            Height = _yRes;

            GameCanvas.Width = width;
            GameCanvas.Height = height;
            GameImage.Width = width;
            GameImage.Height = height;
        }

        /// <summary>
        /// Updates the context
        /// according to the current scene
        /// </summary>
        private void UpdateSceneContext()
        {
            Dispatcher.Invoke(() =>
            {
                _isTextureModeOn = true;
                RemoveOverlay(_loadingScreen);
                _currentCamera = _session.SceneManager.CurrentScene?.SceneCamera;
                _currentScene = _session.SceneManager.CurrentScene;

                SetWindowSize();
            });
        }

        private void UpdateCameraFocusPoint()
        {
            if (_session.SceneManager.CurrentScene != null)
            {
                uint player = _session.SceneManager.CurrentScene.PlayerEntity;
                ITransformComponent playerTransform = _currentScene.EntityManager.GetComponentOfType<ITransformComponent>(player);
                _currentCamera.UpdateFocusPoint(playerTransform);
            }
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
                float graphicX = CalculateGraphicsCoordinate(
                    transformComponents[index].Position.X, 
                    _currentCamera.XOffset, focusPos.X);

                float graphicY = CalculateGraphicsCoordinate(
                    transformComponents[index].Position.Y, 
                    _currentCamera.YOffset, focusPos.Y);

                DrawGraphicsComponent(item, graphicX, graphicY, 
                    transformComponents[index].ScaleX * _sizeMultiplier, 
                    transformComponents[index].ScaleY * _sizeMultiplier, 
                    drawingContext, _isTextureModeOn);

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
            return logicalPosition < focusPos ? 
                offset - ((focusPos - logicalPosition) * _sizeMultiplier) 
                : 
                offset + ((logicalPosition - focusPos) * _sizeMultiplier);
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
        /// Handles when a key is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            _inputHandler.HandleKeyPressed(e.Key);

            HandleSpecialKeyActions();
        }

        /// <summary>
        /// Allows to trigger
        /// pause menu or
        /// detective mode
        /// </summary>
        private void HandleSpecialKeyActions()
        {
            if ((_gameInput.CurrentKeyValue & GameKey.DetectiveMode) == GameKey.DetectiveMode)
            {
                _isTextureModeOn = !_isTextureModeOn;
            }
            if ((_gameInput.CurrentKeyValue & GameKey.Escape) == GameKey.Escape)
            {
                TogglePauseMenu();
            }
        }

        /// <summary>
        /// Handles when a key is no longer pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _inputHandler.HandleKeyReleased(e.Key);
        }

        /// <summary>
        /// Saves the current game
        /// </summary>
        /// <param name="path"></param>
        private void SaveGame(Uri path)
        {
            try
            {
                string filename = path.ToString();
                Save save = new Save();
                var sceneManager = _session.SceneManager;

                save.Scenes = sceneManager.GetScenesToSave();
                save.CurrentIndex = sceneManager.CurrentIndex;
                SaveFileManager.SaveGame(filename, save);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Loads a saved game
        /// </summary>
        /// <param name="path"></param>
        private void LoadGame(Uri path)
        {
            try
            {
                string filename = path.ToString();
                Save save = SaveFileManager.LoadGame(filename);

                InitializeGame(save.Scenes, save.CurrentIndex);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }
        
        /// <summary>
        /// Shows loading overlay
        /// and removes menu overlays
        /// </summary>
        private void ShowLoadingOverlay()
        {
            Dispatcher.Invoke(() =>
            {
                if (!GameGrid.Children.Contains(_loadingScreen))
                {
                    GameGrid.Children.Add(_loadingScreen);
                }
                RemoveOverlay(_mainMenu);
                RemoveOverlay(_pauseMenu);
            });
        }

        private void ShowWinnerOverlay()
        {
            Dispatcher.Invoke(() =>
            {
                if (!GameGrid.Children.Contains(_mainMenu))
                {
                    _session.State.Pause();
                    GameGrid.Children.Add(_mainMenu);
                }
            });
        }

    }
}

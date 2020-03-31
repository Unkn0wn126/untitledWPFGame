#define TRACE
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.GameObjects;
using Engine.Models.Scenes;
using Engine.ResourceConstants.Images;
using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFGame
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IGame _session;

        // image to render to
        private RenderTargetBitmap bitmap;

        // TODO: Put in a separate class
        private readonly Dictionary<int, Action> _userInputActions =
            new Dictionary<int, Action>();

        // Possibly better in another class
        private readonly Dictionary<ImgNames, BitmapImage> _sprites = new Dictionary<ImgNames, BitmapImage>();

        // TODO: Put in a separate class
        private readonly Dictionary<Key, int> _keyCodes =
            new Dictionary<Key, int>();

        // TODO: Put in a separate class
        private List<Key> _previousKeys = new List<Key>();

        // needed for rendering
        private DrawingVisual _drawingVisual = new DrawingVisual();


        // TODO: Move into separate class
        private int _currentKeyValue = 0;

        private BitmapImage _groundImage;
        private BitmapImage _cobbleImage;
        private BitmapImage _playerAvatar;

        private IScene _currentScene;
        private ICamera _currentCamera;

        Rect _rectangle;

        private int _xRes;
        private int _yRes;

        public MainWindow(IGame session, int xRes, int yRes)
        {
            _xRes = xRes;
            _yRes = yRes;

            InitializeComponent();
            _session = session;
            InitializeImages();
            //InitializeCaching();


            InitializeUserInputActions();

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
            // placeholder images
            // I intend to load them all at launch and assign them to a string constant
            // to give objects information of their "avatar" while keeping it independent
            _groundImage = new BitmapImage(_session.ImgPaths.ImageSprites[ImgNames.DIRT]);
            _cobbleImage = new BitmapImage(_session.ImgPaths.ImageSprites[ImgNames.COBBLESTONE]);
            _playerAvatar = new BitmapImage(_session.ImgPaths.ImageSprites[ImgNames.PLAYER]);

            _sprites.Add(ImgNames.DIRT, _groundImage);
            _sprites.Add(ImgNames.COBBLESTONE, _cobbleImage);
            _sprites.Add(ImgNames.PLAYER, _playerAvatar);

            _rectangle = new Rect();
        }

        private void InitializeUserInputActions()
        {
            // This could be it's own separate class...
            // Kind of like a state machine of pressed down keys
            // powers of 2 to allow diagonal movement
            _keyCodes.Add(Key.W, 2);
            _keyCodes.Add(Key.A, 4);
            _keyCodes.Add(Key.S, 8);
            _keyCodes.Add(Key.D, 16);

            _userInputActions.Add(2, () => _session.HandleUserInput(MovementState.UP));
            _userInputActions.Add(4, () => _session.HandleUserInput(MovementState.LEFT));
            _userInputActions.Add(6, () => _session.HandleUserInput(MovementState.UPLEFT));
            _userInputActions.Add(8, () => _session.HandleUserInput(MovementState.DOWN));
            _userInputActions.Add(12, () => _session.HandleUserInput(MovementState.DOWNLEFT));
            _userInputActions.Add(16, () => _session.HandleUserInput(MovementState.RIGHT));
            _userInputActions.Add(18, () => _session.HandleUserInput(MovementState.UPRIGHT));
            _userInputActions.Add(24, () => _session.HandleUserInput(MovementState.DOWNRIGHT));
        }

        public void UpdateGraphics(object sender, EventArgs e)
        {
            // redrawing a bitmap image should be faster
            bitmap.Clear();
            var drawingContext = _drawingVisual.RenderOpen();

            // to have a black background as a default
            _rectangle.X = 0;
            _rectangle.Y = 0;
            _rectangle.Width = _xRes;
            _rectangle.Height = _yRes;
            drawingContext.DrawRectangle(Brushes.Black, null, _rectangle);

            // need to update the camera to know what is visible
            _session.CurrentScene.SceneCamera.UpdatePosition(_currentScene.PlayerGraphicsComponent, _currentScene);

            float xOffset = _currentCamera.XOffset;
            float yOffset = _currentCamera.YOffset;
            Vector2 focusPos = _currentScene.PlayerGraphicsComponent.Transform.Position;

            foreach (var item in _currentCamera.VisibleObjects)
            {

                // conversion of logical coordinates to graphical ones
                float graphicX = item.Transform.Position.X < focusPos.X ? xOffset - (focusPos.X - item.Transform.Position.X) : xOffset + (item.Transform.Position.X - focusPos.X);
                float graphicY = item.Transform.Position.Y < focusPos.Y ? yOffset - (focusPos.Y - item.Transform.Position.Y) : yOffset + (item.Transform.Position.Y - focusPos.Y);

                _rectangle.X = graphicX;
                _rectangle.Y = graphicY;
                _rectangle.Width = item.Transform.ScaleX;
                _rectangle.Height = item.Transform.ScaleY;

                //drawingContext.DrawRectangle(Brushes.Gray, null, _rectangle);

                drawingContext.DrawImage(_sprites[item.CurrentImageName], _rectangle);
            }

            IGraphicsComponent player = _currentScene.PlayerGraphicsComponent;

            // focus point always rendered at the center of the scene
            _rectangle.X = _currentCamera.XOffset;
            _rectangle.Y = _currentCamera.YOffset;
            _rectangle.Width = player.Transform.ScaleX;
            _rectangle.Height = player.Transform.ScaleY;

            drawingContext.DrawImage(_sprites[player.CurrentImageName], _rectangle);

            drawingContext.Close();
            bitmap.Render(_drawingVisual);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                _session.State.TogglePause();
                if (!_session.State.IsRunning())
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
                else
                {
                    GameCanvas.Children.RemoveAt(GameCanvas.Children.Count - 1);
                    GameCanvas.Children.RemoveAt(GameCanvas.Children.Count - 1);
                }
            }
            if (!_previousKeys.Contains(e.Key))
            {
                _previousKeys.Add(e.Key);
            }

            _currentKeyValue = 0;

            foreach (var item in _previousKeys)
            {
                if (_keyCodes.ContainsKey(item))
                {
                    _currentKeyValue += _keyCodes[item];
                }
            }

            if (_userInputActions.ContainsKey(_currentKeyValue))
            {
                _userInputActions[_currentKeyValue].Invoke();
            }
            else
            {
                _session.HandleUserInput(MovementState.STILL);
            }
        }        
        
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

            if (_previousKeys.Contains(e.Key))
            {
                _previousKeys.Remove(e.Key);
            }

            _currentKeyValue = 0;

            foreach (var item in _previousKeys)
            {
                if (_keyCodes.ContainsKey(item))
                {
                    _currentKeyValue += _keyCodes[item];
                }
            }

            if (_userInputActions.ContainsKey(_currentKeyValue))
            {
                _userInputActions[_currentKeyValue].Invoke();
            }
            else
            {
                _session.HandleUserInput(MovementState.STILL);
            }
        }
    }
}

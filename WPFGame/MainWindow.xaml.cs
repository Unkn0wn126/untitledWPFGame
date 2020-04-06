#define TRACE
using Engine.Models.Components;
using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFGame
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IGame _session;
        private RenderTargetBitmap bitmap;

        private readonly Dictionary<int, Action> _userInputActions =
            new Dictionary<int, Action>();

        private readonly Dictionary<Key, int> _keyCodes =
            new Dictionary<Key, int>();

        private List<Key> _previousKeys = new List<Key>();

        private int _currentKeyValue = 0;

        private BitmapImage _groundImage;
        private BitmapImage _playerAvatar;
        private ImageBrush _groundBrush;
        private ImageBrush _playerBrush;

        public MainWindow()
        {
            InitializeComponent();
            _session = new Game();
            // placeholder images
            // I intend to load them all at launch and assign them to a string constant
            // to give objects information of their "avatar" while keeping it independent
            _groundImage = new BitmapImage(new Uri(@"./Resources/Images/ground.jpg", UriKind.Relative));
            _playerAvatar = new BitmapImage(new Uri(@"./Resources/Images/player.png", UriKind.Relative));
            _groundBrush = new ImageBrush(_groundImage);
            _playerBrush = new ImageBrush(_playerAvatar);
            InitializeUserInputActions();
            // this is what everything renders to
            bitmap = new RenderTargetBitmap(800, 600, 96, 96, PixelFormats.Pbgra32);
            GameImage.Source = bitmap;

            CompositionTarget.Rendering += UpdateGraphics;

            // another timer to allow for independent non-graphics update
            Timer timer = new Timer(16);
            timer.Elapsed += Update;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void InitializeUserInputActions()
        {
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

        private void Update(object sender, ElapsedEventArgs e)
        {
            _session.Update();
        }

        private void UpdateGraphics(object sender, EventArgs e)
        {
            // redrawing a bitmap image should be faster
            bitmap.Clear();

            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();

            // to have a black background as a default
            drawingContext.DrawRectangle(
                    Brushes.Black,
                    null,
                    new Rect(0, 0, 800, 600)
                    );
            int counter = 0;

            // need to update the camera to know what is visible
            _session.CurrentScene.SceneCamera.UpdatePosition(_session.CurrentScene.PlayerObject, _session.CurrentScene);

            float xOffset = _session.CurrentScene.SceneCamera.XOffset;
            float yOffset = _session.CurrentScene.SceneCamera.YOffset;
            Vector2 focusPos = _session.CurrentScene.PlayerObject.Position;

            foreach (var item in _session.CurrentScene.SceneCamera.VisibleObjects)
            {
                counter++;

                // conversion of logical coordinates to graphical ones
                float graphicX = item.Position.X < focusPos.X ? xOffset - (focusPos.X - item.Position.X) : xOffset + (item.Position.X - focusPos.X);
                float graphicY = item.Position.Y < focusPos.Y ? yOffset - (focusPos.Y - item.Position.Y) : yOffset + (item.Position.Y - focusPos.Y);

                Rect rectangle = new Rect(graphicX, graphicY, item.Width, item.Height);

                drawingContext.DrawImage(_groundImage, rectangle);
            }

            // focus point always rendered at the center of the scene
            Rect rec = new Rect(_session.CurrentScene.SceneCamera.XOffset,
                _session.CurrentScene.SceneCamera.YOffset,
                _session.CurrentScene.PlayerObject.Width,
                _session.CurrentScene.PlayerObject.Height);

            drawingContext.DrawImage(_playerAvatar, rec);

            drawingContext.Close();
            bitmap.Render(drawingVisual);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
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

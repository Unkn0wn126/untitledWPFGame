#define TRACE
using Engine.Models.Components;
using Engine.Models.GameObjects;
using Engine.ResourceConstants.Images;
using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
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
        private RenderTargetBitmap bitmap;

        private readonly Dictionary<int, Action> _userInputActions =
            new Dictionary<int, Action>();

        private readonly Dictionary<ImgNames, BitmapImage> _sprites = new Dictionary<ImgNames, BitmapImage>();

        private readonly Dictionary<Key, int> _keyCodes =
            new Dictionary<Key, int>();

        private List<Key> _previousKeys = new List<Key>();

        private int _currentKeyValue = 0;

        private BitmapImage _groundImage;
        private BitmapImage _cobbleImage;
        private BitmapImage _playerAvatar;
        private ImageBrush _groundBrush;
        private ImageBrush _playerBrush;

        private ImageBrush _brush;

        public MainWindow()
        {
            InitializeComponent();
            _session = new Game(800, 600);
            // placeholder images
            // I intend to load them all at launch and assign them to a string constant
            // to give objects information of their "avatar" while keeping it independent
            _groundImage = new BitmapImage(_session.ImgPaths.ImageSprites[ImgNames.DIRT]);
            _cobbleImage = new BitmapImage(_session.ImgPaths.ImageSprites[ImgNames.COBBLESTONE]);
            _playerAvatar = new BitmapImage(_session.ImgPaths.ImageSprites[ImgNames.PLAYER]);
            _sprites.Add(ImgNames.DIRT, _groundImage);
            _sprites.Add(ImgNames.COBBLESTONE, _cobbleImage);
            _sprites.Add(ImgNames.PLAYER, _playerAvatar);
            _brush = new ImageBrush();

            _groundBrush = new ImageBrush(_groundImage);
            _playerBrush = new ImageBrush(_playerAvatar);
            InitializeUserInputActions();
            // this is what everything renders to
            bitmap = new RenderTargetBitmap(800, 600, 96, 96, PixelFormats.Pbgra32);
            GameImage.Source = bitmap;

            //CompositionTarget.Rendering += UpdateGraphics;

            // another timer to allow for independent non-graphics update
            Timer timer = new Timer(16);
            timer.Elapsed += Update;
            timer.AutoReset = true;
            timer.Enabled = true;
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

        private void Update(object sender, ElapsedEventArgs e)
        {
            _session.Update();
        }

        public void UpdateGraphics(object sender, EventArgs e)
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

                drawingContext.DrawImage(_sprites[item.GraphicsComponent.CurrentImageName], rectangle);
            }

            IGameObject player = _session.CurrentScene.PlayerObject;

            // focus point always rendered at the center of the scene
            Rect rec = new Rect(_session.CurrentScene.SceneCamera.XOffset,
                _session.CurrentScene.SceneCamera.YOffset, 
                _session.CurrentScene.PlayerObject.Width, 
                _session.CurrentScene.PlayerObject.Height);

            drawingContext.DrawImage(_sprites[player.GraphicsComponent.CurrentImageName], rec);

            drawingContext.Close();
            bitmap.Render(drawingVisual);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                _session.State.TogglePause();
                if (!_session.State.IsRunning())
                {
                    Rectangle overlay = new Rectangle();
                    Color testColor = Color.FromArgb(172, 172, 172, 255);
                    overlay.Width = 800;
                    overlay.Height = 600;
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

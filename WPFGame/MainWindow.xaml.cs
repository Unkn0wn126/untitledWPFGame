#define TRACE
using Engine.Models.Components;
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
            _groundImage = new BitmapImage(new Uri(@"./Resources/Images/ground.jpg", UriKind.Relative));
            _playerAvatar = new BitmapImage(new Uri(@"./Resources/Images/player.png", UriKind.Relative));
            _groundBrush = new ImageBrush(_groundImage);
            _playerBrush = new ImageBrush(_playerAvatar);
            InitializeUserInputActions();
            bitmap = new RenderTargetBitmap(800, 600, 96, 96, PixelFormats.Pbgra32);
            GameImage.Source = bitmap;

            CompositionTarget.Rendering += UpdateGraphics;

            Timer timer = new Timer(1);
            timer.Elapsed += Update;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void InitializeUserInputActions()
        {
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
            bitmap.Clear();
            //GameCanvas.Children.Clear();
            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawRectangle(
                    Brushes.Black,
                    null,
                    new Rect(0, 0, 800, 600)
                    );
            int counter = 0;
            _session.CurrentScene.SceneCamera.UpdatePosition(_session.CurrentScene.PlayerObject, _session.CurrentScene);

            float xOffset = _session.CurrentScene.SceneCamera.XOffset;
            float yOffset = _session.CurrentScene.SceneCamera.YOffset;
            Vector2 focusPos = _session.CurrentScene.PlayerObject.Position;

            //float minX = _session.CurrentScene.SceneCamera.VisibleObjects.Count > 0 ? _session.CurrentScene.SceneCamera.VisibleObjects.Min(x => x.Position.X) : 0;
            //float minY = _session.CurrentScene.SceneCamera.VisibleObjects.Count > 0 ? _session.CurrentScene.SceneCamera.VisibleObjects.Min(x => x.Position.Y) : 0;

            Color color = Color.FromRgb(61, 61, 69);

            

            foreach (var item in _session.CurrentScene.SceneCamera.VisibleObjects)
            {
                counter++;
                //Rectangle rectangle = new Rectangle
                //{
                //    Height = item.Height,
                //    Width = item.Width
                //};
                //Trace.WriteLine($"{item.Position.X - minX}; {item.Position.Y - minY}");

                float graphicX = item.Position.X < focusPos.X ? xOffset - (focusPos.X - item.Position.X) : xOffset + (item.Position.X - focusPos.X);
                float graphicY = item.Position.Y < focusPos.Y ? yOffset - (focusPos.Y - item.Position.Y) : yOffset + (item.Position.Y - focusPos.Y);

                Rect rectangle = new Rect(graphicX, graphicY, item.Width, item.Height);

                //rectangle.Fill = Brushes.Brown;

                //GameCanvas.Children.Add(rectangle);

                //Canvas.SetLeft(rectangle, item.Position.X * item.Width);
                //Canvas.SetTop(rectangle, item.Position.Y * item

                drawingContext.DrawImage(_groundImage, rectangle);

                //drawingContext.DrawRectangle(
                //    _groundBrush, 
                //    null, 
                //    rectangle
                //    );

                //FormattedText text = new FormattedText($"{item.Position.X}\n{item.Position.Y}", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.Black);
                //drawingContext.DrawText(text, new Point(graphicX, graphicY));
            }

            //Rectangle rec = new Rectangle
            //{
            //    Height = _session.GraphicsComponents[0].Height,
            //    Width = _session.GraphicsComponents[0].Width
            //};

            //rec.Fill = Brushes.Green;

            //GameCanvas.Children.Add(rec);

            //Canvas.SetLeft(rec, _session.GraphicsComponents[0].Position.X);
            //Canvas.SetTop(rec, _session.GraphicsComponents[0].Position.Y);

            Rect rec = new Rect(_session.CurrentScene.SceneCamera.XOffset,
                _session.CurrentScene.SceneCamera.YOffset, 
                _session.CurrentScene.PlayerObject.Width, 
                _session.CurrentScene.PlayerObject.Height);

            Color player = Color.FromRgb(42, 34, 115);

            drawingContext.DrawImage(_playerAvatar, rec);

            //drawingContext.DrawRectangle(
            //    _playerBrush,
            //    null,
            //    rec
            //);

            //Render(drawingContext);
            drawingContext.Close();
            bitmap.Render(drawingVisual);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if (_userInputActions.ContainsKey(e.Key))
            //{
            //    _userInputActions[e.Key].Invoke();
            //}
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
            //if (_userInputActions.ContainsKey(e.Key))
            //{
            //    _session.HandleUserInput(MovementState.STILL);
            //}
        }
    }
}

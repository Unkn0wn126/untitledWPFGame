using Engine.Models.Components.RigidBody;
using Engine.Models.Scenes;
using GameInputHandler;
using TimeUtils;

namespace Engine.Models.Components.Script
{
    /// <summary>
    /// Script component used
    /// to move the player character
    /// around based on the player input
    /// </summary>
    public class PlayerMovementScript : IScriptComponent
    {
        private readonly float _baseVelocity;
        private readonly uint _player;

        private readonly float _baseForceX;
        private readonly float _baseForceY;

        private readonly IScene _context;

        private readonly GameInput _gameInputHandler;

        private readonly GameTime _gameTime;

        public PlayerMovementScript(GameTime gameTime, GameInput gameInputHandler, IScene context, uint player, float baseVelocity)
        {
            _gameTime = gameTime;
            _gameInputHandler = gameInputHandler;
            _baseVelocity = baseVelocity;
            _player = player;
            _context = context;

            _baseForceX = 0;
            _baseForceY = 0;
        }

        /// <summary>
        /// Changes the player avatar
        /// based on the direction headed to
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="currValue"></param>
        private void ChangeAvatar(IGraphicsComponent graphics, GameKey currValue)
        {
            if ((currValue & GameKey.Right) == GameKey.Right)
            {
                graphics.CurrentImageName = ResourceManagers.Images.ImgName.PlayerRight;
            }
            else if((currValue & GameKey.Left) == GameKey.Left)
            {
                graphics.CurrentImageName = ResourceManagers.Images.ImgName.PlayerLeft;
            }
            else
            {
                graphics.CurrentImageName = ResourceManagers.Images.ImgName.Player;
            }
        }

        public void Update()
        {
            GameKey currValue = _gameInputHandler.CurrentKeyValue;
            IRigidBodyComponent rigidBody = _context.EntityManager.GetComponentOfType<IRigidBodyComponent>(_player);
            IGraphicsComponent graphics = _context.EntityManager.GetComponentOfType<IGraphicsComponent>(_player);

            ChangeAvatar(graphics, currValue);

            if ((currValue & GameKey.Up) == GameKey.Up)
                rigidBody.ForceY = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if ((currValue & GameKey.Down) == GameKey.Down)
                rigidBody.ForceY = +_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if ((currValue & GameKey.Left) == GameKey.Left)
                rigidBody.ForceX = -_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if ((currValue & GameKey.Right) == GameKey.Right)
                rigidBody.ForceX = +_baseVelocity * _gameTime.DeltaTimeInSeconds;
            if ((currValue & GameKey.Up) != GameKey.Up && (currValue & GameKey.Down) != GameKey.Down)
                rigidBody.ForceY = _baseForceY * _gameTime.DeltaTimeInSeconds;
            if ((currValue & GameKey.Left) != GameKey.Left && (currValue & GameKey.Right) != GameKey.Right)
                rigidBody.ForceX = _baseForceX * _gameTime.DeltaTimeInSeconds;
        }
    }
}

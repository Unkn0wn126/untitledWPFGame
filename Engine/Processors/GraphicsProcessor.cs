using Engine.Models.Components;
using Engine.Models.Scenes;
using System.Collections.Generic;

namespace Engine.Processors
{
    /// <summary>
    /// Handles entity visibility
    /// updates for the graphics
    /// engine
    /// </summary>
    public class GraphicsProcessor : IProcessor
    {
        private Dictionary<ITransformComponent, IGraphicsComponent> _renderables;

        private IScene _context;
        private ITransformComponent _focusPoint;

        public GraphicsProcessor(IScene context)
        {
            _context = context;
            _focusPoint = context.SceneCamera.FocusPoint;
            _renderables = new Dictionary<ITransformComponent, IGraphicsComponent>();
        }

        public void ChangeContext(IScene context)
        {
            _context = context;
            _focusPoint = context.SceneCamera.FocusPoint;
        }

        public void ProcessOneGameTick(float lastFrameTime)
        {
            _renderables.Clear();

            List<uint> activeEntites = _context.EntityManager.GetAllActiveEntities();
            activeEntites.ForEach(x => 
            {
                if (_context.EntityManager.EntityHasComponent<IGraphicsComponent>(x) && _context.EntityManager.EntityHasComponent<ITransformComponent>(x))
                {
                    if (!_renderables.ContainsKey(_context.EntityManager.GetComponentOfType<ITransformComponent>(x)))
                    {
                        _renderables.Add(_context.EntityManager.GetComponentOfType<ITransformComponent>(x), _context.EntityManager.GetComponentOfType<IGraphicsComponent>(x));
                    }
                }
            });

            _context.SceneCamera.UpdatePosition(_renderables);
        }
    }
}

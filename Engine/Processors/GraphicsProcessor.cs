using Engine.Models.Components;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Processors
{
    public class GraphicsProcessor : IProcessor
    {
        private Dictionary<ITransformComponent, IGraphicsComponent> _renderables;

        private IScene _context;
        private ITransformComponent _focusPoint;

        public GraphicsProcessor(IScene context)
        {
            _context = context;
            _focusPoint = context.PlayerTransform;
            _renderables = new Dictionary<ITransformComponent, IGraphicsComponent>();
        }

        /// <summary>
        /// Updates the context to a new scene
        /// </summary>
        /// <param name="context"></param>
        public void ChangeContext(IScene context)
        {
            _context = context;
            _focusPoint = context.PlayerTransform;
        }

        /// <summary>
        /// Handles the update of visible entities
        /// </summary>
        /// <param name="lastFrameTime"></param>
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

            _context.SceneCamera.UpdatePosition(_focusPoint, _renderables);
        }
    }
}

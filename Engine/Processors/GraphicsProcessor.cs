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

        public GraphicsProcessor(IScene context, ITransformComponent focusPoint)
        {
            _context = context;
            _focusPoint = focusPoint;
            _renderables = new Dictionary<ITransformComponent, IGraphicsComponent>();
        }
        public void ProcessOneGameTick(float lastFrameTime)
        {
            _renderables.Clear();

            List<uint> activeEntites = _context.EntityManager.GetAllActiveEntities();
            activeEntites.ForEach(x => 
            {
                if (_context.EntityManager.EntityHasComponent(x, typeof(IGraphicsComponent)))
                {
                    if (!_renderables.ContainsKey(_context.EntityManager.GetTransformComponent(x)))
                    {
                        _renderables.Add(_context.EntityManager.GetTransformComponent(x), _context.EntityManager.GetGraphicsComponent(x));
                    }
                }
            });

            _context.SceneCamera.UpdatePosition(_focusPoint, _renderables);
        }
    }
}

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
        private List<IGraphicsComponent> _graphics;
        private List<ITransformComponent> _pos;

        private IScene _context;
        private ITransformComponent _focusPoint;

        public GraphicsProcessor(IScene context, ITransformComponent focusPoint)
        {
            _context = context;
            _focusPoint = focusPoint;
            _graphics = new List<IGraphicsComponent>();
            _pos = new List<ITransformComponent>();
        }
        public void ProcessOneGameTick(float lastFrameTime)
        {
            _graphics.Clear();
            _pos.Clear();
            //_context.EntityManager.UpdateActiveEntities(_focusPoint);
            List<uint> activeEntites = _context.EntityManager.GetAllActiveEntities();
            activeEntites.ForEach(x => 
            {
                if (x != _context.PlayerEntity && _context.EntityManager.EntityHasComponent(x, typeof(IGraphicsComponent)))
                {
                    _graphics.Add(_context.EntityManager.GetGraphicsComponent(x));
                    _pos.Add(_context.EntityManager.GetTransformComponent(x));
                }
            });

            _context.SceneCamera.UpdatePosition(_focusPoint, _graphics, _pos);
        }
    }
}

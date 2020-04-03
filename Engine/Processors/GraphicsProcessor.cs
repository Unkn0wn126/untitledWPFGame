using Engine.Models.Components;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
        }
        public void ProcessOnEeGameTick(long lastFrameTime)
        {
            _context.EntityManager.UpdateActiveEntities(_focusPoint);
            _context.SceneCamera.UpdatePosition(_focusPoint, _context.EntityManager);
        }
    }
}

using Engine.Models.Components.Life;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.Components.Script
{
    public class SceneChangeScript : IScriptComponent
    {
        private SceneChange _sceneChange;
        private IScene _context;
        private uint _owner;
        private ICollisionComponent _ownerCollision;
        public SceneChangeScript(IScene context, SceneChange sceneChange, uint owner)
        {
            _sceneChange = sceneChange;
            _context = context;
            _owner = owner;
            _ownerCollision = context.EntityManager.GetComponentOfType<ICollisionComponent>(owner);
        }
        public void Update()
        {
            foreach (var item in _ownerCollision.CollidingWith)
            {
                if (_context.EntityManager.EntityHasComponent<ICollisionComponent>(item) && _context.EntityManager.EntityHasComponent<ILifeComponent>(item))
                {
                    if (_context.EntityManager.GetComponentOfType<ILifeComponent>(item).IsPlayer)
                    {
                        _sceneChange.Invoke();
                    }
                }
            }
        }
    }
}

using Engine.Models.Components.Life;
using Engine.Models.Scenes;

namespace Engine.Models.Components.Script
{
    /// <summary>
    /// Script component used to
    /// invoke scene change when
    /// a condition (player stepping on a trigger)
    /// is met
    /// </summary>
    public class SceneChangeScript : IScriptComponent
    {
        private readonly SceneChange _sceneChange;
        private readonly IScene _context;
        private readonly ICollisionComponent _ownerCollision;

        public SceneChangeScript(IScene context, SceneChange sceneChange, uint owner)
        {
            _sceneChange = sceneChange;
            _context = context;
            _ownerCollision = context.EntityManager.GetComponentOfType<ICollisionComponent>(owner);
        }

        public void Update()
        {
            foreach (var item in _ownerCollision.CollidingWith)
            {
                if (_context.EntityManager.EntityHasComponent<ICollisionComponent>(item) && 
                    _context.EntityManager.EntityHasComponent<ILifeComponent>(item))
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

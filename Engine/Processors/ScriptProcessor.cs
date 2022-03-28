using Engine.EntityManagers;
using Engine.Models.Components.Script;
using Engine.Models.Scenes;
using System.Collections.Generic;

namespace Engine.Processors
{
    /// <summary>
    /// Handles processing of
    /// scripts associated with entities
    /// </summary>
    public class ScriptProcessor : IProcessor
    {
        private IScene _context;
        public ScriptProcessor(IScene context)
        {
            _context = context;
        }

        public void ChangeContext(IScene context)
        {
            _context = context;
        }

        public void ProcessOneGameTick(float lastFrameTime)
        {
            IEntityManager manager = _context.EntityManager;
            List<uint> active = manager.GetAllActiveEntities();

            List<List<IScriptComponent>> scripts = new List<List<IScriptComponent>>();

            // first get all the script components to be updated
            active.ForEach(x =>
            {
                if (manager.EntityHasComponent<IScriptComponent>(x))
                {
                    scripts.Add(manager.GetEntityScriptComponents(x));
                }
            });

            scripts.ForEach(x =>
            {
                foreach (var item in x) // then update them
                {
                    item.Update();
                }
            });
        }
    }
}

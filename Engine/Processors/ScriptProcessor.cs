using Engine.EntityManagers;
using Engine.Models.Components.Script;
using Engine.Models.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Processors
{
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

            active.ForEach(x =>
            {
                if (manager.EntityHasComponent<IScriptComponent>(x))
                {
                    scripts.Add(manager.GetEntityScriptComponents(x));
                }
            });

            Parallel.ForEach(scripts, x =>
            {
                foreach (var item in x)
                {
                    item.Update();
                }
            });
        }
    }
}

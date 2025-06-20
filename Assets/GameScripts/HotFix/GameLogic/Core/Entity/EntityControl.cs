using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityControl : IMemory
    {
        int EntityId = 0;
        
        public static EntityControl Create()
        {
            EntityControl entityLoader = MemoryPool.Acquire<EntityControl>();
            return entityLoader;
        }

        
        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}

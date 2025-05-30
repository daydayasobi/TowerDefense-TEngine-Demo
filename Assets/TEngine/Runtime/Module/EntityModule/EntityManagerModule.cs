using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public class EntityManagerModule : Module, IEntityManagerModule
    {
        private static int s_SerialId = 0;
        
        public int GenerateSerialId()
        {
            return ++s_SerialId;
        }

        public void ShowEntity()
        {
            Log.Debug("ShowEntity not implemented in EntityManager");
        }

        public void ShowEntity<T>(int serialId, int entityId, object userData = null)
        {
            
        }

        public int ShowEntity(int entityId, int entityGroupId, object userData = null)
        {
            return 0;
        }

        public void HideEntity(int serialId, object userData = null)
        {
            
        }

        public override void OnInit()
        {
            
        }

        public override void Shutdown()
        {
            
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityEx : Entity
    {
        public override void OnInit(int entityId, int serialId, string entityAssetName, EntityLogic entityLogic)
        {
            base.OnInit(entityId, serialId, entityAssetName, entityLogic);
        }

        public override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            PoolManager.Instance.PushGameObject(this.gameObject);
        }

        public override void Clear()
        {
            base.Clear();
            // PoolManager.Instance.PushGameObject(this.gameObject);
        }
    }
}

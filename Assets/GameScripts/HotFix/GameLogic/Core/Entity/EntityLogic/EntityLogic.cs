using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class EntityLogic : Entity
    {
        public virtual void OnInit()
        {
        }

        protected virtual void Update()
        {
        }

        public virtual void OnShow(object userData)
        {
        }

        protected virtual void OnHide(bool isShutdown, object userData)
        {
            // Cleanup logic if needed
        }
    }
}
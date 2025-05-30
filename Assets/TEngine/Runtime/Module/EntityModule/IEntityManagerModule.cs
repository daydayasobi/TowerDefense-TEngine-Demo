using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TEngine
{
    public interface IEntityManagerModule
    {
        public int GenerateSerialId();
        public void ShowEntity();

        public abstract void ShowEntity<T>(int serialId, int entityId, object userData = null);

        public int ShowEntity(int entityId, int entityGroupId, object userData = null);

        public void HideEntity(int serialId, object userData = null);
    }
}
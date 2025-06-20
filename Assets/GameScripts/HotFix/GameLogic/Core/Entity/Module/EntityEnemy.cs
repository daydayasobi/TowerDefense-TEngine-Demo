using GameLogic.View;
using UnityEngine;

namespace GameLogic.Module
{
    public class EntityEnemy: EntityBase
    {


        /// <summary>
        /// 释放对象。
        /// </summary>
        /// <param name="isShutdown">是否是关闭对象池时触发。</param>
        protected override void Release(bool isShutdown)
        {
            var Entity = (GameObject)Target;
            GameObject.Destroy(Entity);
        }
        /// <summary>
        /// 获取对象回调
        /// </summary>
        protected override void OnSpawn()
        {
            base.OnSpawn();
            var Entity = (GameObject)Target;
            Entity.SetActive(true);
            Entity.GetComponent<GameLogic.View.EntityEnemy>().OnShow();
        }
        /// <summary>
        /// 回收对象回调
        /// </summary>
        protected override void OnUnspawn()
        {
            base.OnUnspawn();
            var Entity = (GameObject)Target;
            Entity.SetActive(false);
            Entity.GetComponent<GameLogic.View.EntityEnemy>().OnHide();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic.View;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityTowerPreviewLogic : EntityLogic
    {
        // 预览塔的碰撞检测半径
        [SerializeField]
        private float sphereCastRadius = 1;

        // 预览塔移动的阻尼速度
        [SerializeField]
        private float dampSpeed = 5;

        // 碰撞检测时用于世界放置的层掩码
        [SerializeField]
        private LayerMask ghostWorldPlacementMask;

        // 碰撞检测时用于放置区域的层掩码
        [SerializeField]
        private LayerMask placementAreaMask;

        // 预览塔的有效位置材质
        [SerializeField]
        private Material material;

        // 预览塔的无效位置材质
        [SerializeField]
        private Material invalidPositionMaterial;

        // 当前的放置区域
        private IPlacementArea currentArea;

        // 当前网格位置
        private IntVector2 m_GridPosition;

        // 预览塔的数据
        private EntityTowerPreviewData entityTowerPreviewData;

        // 目标位置
        private Vector3 targetPos;

        // 移动速度
        private Vector3 moveVel;

        // 是否为有效位置
        private bool validPos = false;

        // 是否可见
        private bool visible = true;

        // 是否可以放置
        private bool canPlace = false;

        // 预览塔的所有网格渲染器
        private MeshRenderer[] renderers;

        // 是否可以放置的属性
        public bool CanPlace
        {
            get { return canPlace; }
        }

        // 显示方法
        public override void OnInit(object userData)
        {
            base.OnInit(userData);

            if (userData != null)
            {
                if (userData is EntityTowerPreviewData)
                {
                    entityTowerPreviewData = userData as EntityTowerPreviewData;
                    // transform.position = entityTowerPreviewData.Position;
                    // transform.rotation = entityTowerPreviewData.Rotation;
                    transform.parent = entityTowerPreviewData.Parent;
                    // 加载模型
                    // ShowTowerLevelEntity(EntityTowerData.Tower.Level);
                }
                else
                {
                    Log.Error("Invalid userData type in OnInit. Expected EntityTowerPreviewData.");
                }
            }
            else
            {
                Log.Error("userData 为 null！");
            }

            renderers = transform.GetComponentsInChildren<MeshRenderer>(true);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            // 将传入的数据转换为预览塔数据
            // entityTowerPreviewData = userData as EntityTowerPreviewData;
            // if (entityTowerPreviewData == null)
            // {
            //     Log.Error("EntityTowerPreviewData param is invaild");
            //     return;
            // }

            // 初始化状态
            canPlace = false;
            validPos = false;
            moveVel = Vector3.zero;
            SetVisiable(true);
        }

        // 隐藏方法
        protected void OnHide(bool isShutdown, object userData)
        {
            // 清理状态
            currentArea = null;
            m_GridPosition = IntVector2.zero;
            entityTowerPreviewData = null;
        }

        // 更新方法
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            // 如果没有预览塔数据，则直接返回
            if (entityTowerPreviewData == null)
                return;
            // 移动预览塔
            MoveGhost(false);

            // 获取当前位置
            Vector3 currentPos = transform.position;

            // 如果当前位置与目标位置有差距，则平滑移动到目标位置
            if (Vector3.SqrMagnitude(currentPos - targetPos) > 0.01f)
            {
                currentPos = Vector3.SmoothDamp(currentPos, targetPos, ref moveVel, dampSpeed);
                transform.position = currentPos;
            }
            else
            {
                // 如果已经到达目标位置，则停止移动
                moveVel = Vector3.zero;
            }
        }

        // 设置预览塔的可见性
        private void SetVisiable(bool value)
        {
            // 如果可见性没有变化，则直接返回
            if (visible == value)
                return;

            // 如果设置为不可见，则停止移动并标记为无效位置
            if (visible == false)
            {
                moveVel = Vector3.zero;
                validPos = false;
            }

            // 遍历所有网格渲染器，设置其是否可见
            foreach (var item in renderers)
            {
                item.enabled = value;
            }

            // 更新可见性状态
            visible = value;
        }

        // 移动预览塔
        private void MoveGhost(bool hideWhenInvalid = true)
        {
            // 将鼠标位置转换为射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // 如果射线与放置区域发生碰撞，则调用MoveGhostWithRaycastHit方法
            if (Physics.Raycast(ray, out hit, float.MaxValue, placementAreaMask))
            {
                MoveGhostWithRaycastHit(hit);
            }
            else
            {
                // 如果没有碰撞，则调用MoveGhostOntoWorld方法
                MoveGhostOntoWorld(ray, hideWhenInvalid);
            }
        }

        // 当射线与放置区域发生碰撞时，移动预览塔
        private void MoveGhostWithRaycastHit(RaycastHit raycast)
        {
            // 获取碰撞对象的放置区域组件
            currentArea = raycast.collider.GetComponent<IPlacementArea>();

            // 如果没有找到放置区域组件，则记录错误并返回
            if (currentArea == null)
            {
                Log.Error("There is not an IPlacementArea attached to the collider found on the m_PlacementAreaMask");
                return;
            }

            // 将世界坐标转换为网格坐标
            m_GridPosition = currentArea.WorldToGrid(raycast.point, entityTowerPreviewData.Tower.Dimensions);

            // 检查塔是否可以放置在该位置
            TowerFitStatus fits = currentArea.Fits(m_GridPosition, entityTowerPreviewData.Tower.Dimensions);

            // 设置预览塔可见
            SetVisiable(true);

            // 更新是否可以放置的状态
            canPlace = fits == TowerFitStatus.Fits;

            // 移动预览塔到目标位置，并设置材质
            Move(currentArea.GridToWorld(m_GridPosition, entityTowerPreviewData.Tower.Dimensions),
                currentArea.transform.rotation,
                canPlace);
        }

        // 当射线没有与放置区域发生碰撞时，移动预览塔
        private void MoveGhostOntoWorld(Ray ray, bool hideWhenInvalid)
        {
            // 清除当前放置区域
            currentArea = null;

            // 如果不需要在无效位置隐藏预览塔，则进行碰撞检测
            if (!hideWhenInvalid)
            {
                RaycastHit hit;

                // 使用球形射线检测预览塔可以放置的位置
                Physics.SphereCast(ray, sphereCastRadius, out hit, float.MaxValue, ghostWorldPlacementMask);
                // 如果检测到碰撞，则移动预览塔到碰撞点
                if (hit.collider != null)
                {
                    SetVisiable(true);
                    Move(hit.point, hit.collider.transform.rotation, false);
                }
            }
            else
            {
                // 如果需要在无效位置隐藏预览塔，则直接隐藏
                SetVisiable(false);
            }
        }

        // 移动预览塔到指定位置
        private void Move(Vector3 worldPosition, Quaternion rotation, bool validLocation)
        {
            // 设置目标位置
            targetPos = worldPosition;

            // 如果之前不是有效位置，则立即移动到目标位置
            if (!validPos)
            {
                validPos = true;
                transform.position = targetPos;
            }

            // 设置旋转
            transform.rotation = rotation;

            // 遍历所有网格渲染器，根据是否为有效位置设置材质
            foreach (MeshRenderer meshRenderer in renderers)
            {
                meshRenderer.sharedMaterial = validLocation ? material : invalidPositionMaterial;
            }
        }

        // 尝试建造塔
        public bool TryBuildTower()
        {
            // 如果当前放置区域为空，则记录错误并返回false
            if (currentArea == null)
            {
                Log.Error("Current area is null");
                return false;
            }

            // 初始化位置和旋转
            Vector3 position = Vector3.zero;
            Quaternion rotation = currentArea.transform.rotation;

            // 检查塔是否可以放置在当前位置
            TowerFitStatus fits = currentArea.Fits(m_GridPosition, entityTowerPreviewData.Tower.Dimensions);

            // 如果可以放置，则进行建造操作
            if (fits == TowerFitStatus.Fits)
            {
                // 获取建造位置
                position = currentArea.GridToWorld(m_GridPosition, entityTowerPreviewData.Tower.Dimensions);

                // 占用网格
                currentArea.Occupy(m_GridPosition, entityTowerPreviewData.Tower.Dimensions);

                // 触发建造塔事件
                GameEvent.Send(TestEvent.OnTest1);
                GameEvent.Send(LevelEvent.OnBuildTower, entityTowerPreviewData.Tower, currentArea, m_GridPosition, position, rotation);
                return true;
            }

            // 如果不能放置，则返回false
            return false;
        }
    }
}
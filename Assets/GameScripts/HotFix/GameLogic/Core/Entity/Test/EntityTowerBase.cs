using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityTowerBase : EntityLogic
    {
        protected EntityDataTower entityDataTower;
        protected Entity entityTowerLevel;
        protected EntityTowerLevel entityLogicTowerLevel;
        
        protected bool pause = false;
        
    }
}
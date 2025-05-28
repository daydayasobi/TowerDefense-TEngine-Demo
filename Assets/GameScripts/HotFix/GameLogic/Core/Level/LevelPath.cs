using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class LevelPath : MonoBehaviour
    {
        [SerializeField]
        private Transform[] pathNodes;

        public Transform[] PathNodes
        {
            get
            {
                return pathNodes;
            }
        }
    }
}

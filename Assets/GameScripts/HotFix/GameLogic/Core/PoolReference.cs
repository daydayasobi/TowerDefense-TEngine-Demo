using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 应用池
    /// </summary>
    public class PoolReference : MonoBehaviour
    {
        private static PoolReference _instance;

        public static PoolReference Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PoolReference>();
                }

                if (_instance == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = nameof(PoolReference);
                    _instance = gameObject.AddComponent<PoolReference>();
                    _instance.poolRootObj = gameObject;
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }
        
        [SerializeField] private GameObject poolRootObj;
    }
}

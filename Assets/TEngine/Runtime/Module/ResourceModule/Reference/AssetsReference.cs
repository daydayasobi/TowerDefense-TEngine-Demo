using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace TEngine
{
    [Serializable]
    public struct AssetsRefInfo
    {
        public int instanceId;

        public Object refAsset;

        public AssetsRefInfo(Object refAsset)
        {
            this.refAsset = refAsset;
            instanceId = this.refAsset.GetInstanceID();
        }
    }

    [DisallowMultipleComponent]
    public sealed class AssetsReference : MonoBehaviour
    {
        private static readonly Dictionary<GameObject, int> _gameObjectCountMap = new Dictionary<GameObject, int>();

        [SerializeField]
        private GameObject sourceGameObject;

        [SerializeField]
        private List<AssetsRefInfo> refAssetInfoList;

        private static IResourceModule _resourceModule;

        private void CheckInit()
        {
            if (_resourceModule != null)
            {
                return;
            }
            else
            {
                _resourceModule = ModuleSystem.GetModule<IResourceModule>();
            }

            if (_resourceModule == null)
            {
                throw new GameFrameworkException($"resourceModule is null.");
            }
        }

        private void CheckRelease()
        {
            if (_gameObjectCountMap.TryGetValue(sourceGameObject, out var count))
            {
                if (count <= 1)
                {
                    _gameObjectCountMap.Remove(sourceGameObject);
                    _resourceModule.UnloadAsset(sourceGameObject);
                }
                else
                {
                    _gameObjectCountMap[sourceGameObject] = count - 1;
                }
            }
            else
            {
                Log.Warning($"sourceGameObject is not invalid.");
            }
        }

        private void CheckAdd()
        {
            if (_gameObjectCountMap.TryGetValue(sourceGameObject, out var count))
            {
                _gameObjectCountMap[sourceGameObject] = count + 1;
            }
            else
            {
                _gameObjectCountMap[sourceGameObject] = 1;
            }
        }

        private void Awake()
        {
            if (sourceGameObject != null)
            {
                _gameObjectCountMap[sourceGameObject] = _gameObjectCountMap.TryGetValue(sourceGameObject, out var count) ? count + 1 : 1;
            }
        }

        private void OnDestroy()
        {
            CheckInit();
            if (sourceGameObject != null)
            {
                CheckRelease();
            }

            ReleaseRefAssetInfoList();
        }

        private void ReleaseRefAssetInfoList()
        {
            if (refAssetInfoList != null)
            {
                foreach (var refInfo in refAssetInfoList)
                {
                    _resourceModule.UnloadAsset(refInfo.refAsset);
                }

                refAssetInfoList.Clear();
            }
        }

        public AssetsReference Ref(GameObject source, IResourceModule resourceModule = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            _resourceModule = resourceModule;
            sourceGameObject = source;
            CheckAdd();
            return this;
        }

        public AssetsReference Ref<T>(T source, IResourceModule resourceModule = null) where T : UnityEngine.Object
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            _resourceModule = resourceModule;
            if (refAssetInfoList == null)
            {
                refAssetInfoList = new List<AssetsRefInfo>();
            }

            refAssetInfoList.Add(new AssetsRefInfo(source));
            return this;
        }

        public static AssetsReference Instantiate(GameObject source, Transform parent = null, IResourceModule resourceModule = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            GameObject instance = Object.Instantiate(source, parent);
            return instance.AddComponent<AssetsReference>().Ref(source, resourceModule);
        }

        public static AssetsReference Ref(GameObject source, GameObject instance, IResourceModule resourceModule = null)
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            if (source.scene.name != null)
            {
                throw new GameFrameworkException($"Source gameObject is in scene.");
            }

            var comp = instance.GetComponent<AssetsReference>();
            return comp ? comp.Ref(source, resourceModule) : instance.AddComponent<AssetsReference>().Ref(source, resourceModule);
        }

        public static AssetsReference Ref<T>(T source, GameObject instance, IResourceModule resourceModule = null) where T : UnityEngine.Object
        {
            if (source == null)
            {
                throw new GameFrameworkException($"Source gameObject is null.");
            }

            var comp = instance.GetComponent<AssetsReference>();
            return comp ? comp.Ref(source, resourceModule) : instance.AddComponent<AssetsReference>().Ref(source, resourceModule);
        }
    }
}
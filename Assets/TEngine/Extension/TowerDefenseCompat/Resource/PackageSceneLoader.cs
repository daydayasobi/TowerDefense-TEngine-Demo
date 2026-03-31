using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using YooAsset;

namespace TEngine
{
    public static class PackageSceneLoader
    {
        public static async UniTask<Scene> LoadSceneAsync(string packageName, string location,
            LoadSceneMode sceneMode = LoadSceneMode.Single, bool suspendLoad = false, uint priority = 100,
            bool gcCollect = true, Action<float> progressCallBack = null)
        {
            var package = YooAssets.GetPackage(packageName);
            if (package == null)
            {
                throw new InvalidOperationException($"Could not find YooAsset package '{packageName}'.");
            }

            var sceneHandle = package.LoadSceneAsync(location, sceneMode, LocalPhysicsMode.None, suspendLoad, priority);
            if (progressCallBack != null)
            {
                while (!sceneHandle.IsDone && sceneHandle.IsValid)
                {
                    progressCallBack.Invoke(sceneHandle.Progress);
                    await UniTask.Yield();
                }
            }
            else
            {
                await sceneHandle.ToUniTask();
            }

            if (sceneMode == LoadSceneMode.Single)
            {
                ModuleSystem.GetModule<IResourceModule>().ForceUnloadUnusedAssets(gcCollect);
            }

            return sceneHandle.SceneObject;
        }
    }
}

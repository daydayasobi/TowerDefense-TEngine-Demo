using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameConfig;
using TEngine;
using UnityEngine;
using YooAsset;

namespace GameLogic
{
    public class ChangeSceneProcedure : ProcedureBase
    {
        IFsm<IProcedureModule> procedureOwner;
        private bool lockBtn = false;

        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            this.procedureOwner = procedureOwner;
            base.OnEnter(procedureOwner);
            Log.Debug("选择场景流程");
            LoadData();
            GameModule.UI.ShowUI<UILevelSelectForm>();
            GameEvent.AddEventListener(ChangeSceneEvent.MenuSelect, MenuSelect);
            GameEvent.AddEventListener<LevelDataBase>(ChangeSceneEvent.LevelSelect, LevelSelect);
        }

        protected override void OnLeave(IFsm<IProcedureModule> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            Log.Debug("关闭场景流程");
            GameModule.UI.CloseUI<UILevelSelectForm>();
            GameModule.UI.CloseUI<MainMenuUI>();
            GameEvent.RemoveEventListener(ChangeSceneEvent.MenuSelect, MenuSelect);
            GameEvent.RemoveEventListener<LevelDataBase>(ChangeSceneEvent.LevelSelect, LevelSelect);
        }

        private void MenuSelect()
        {
            ChangeState<MainMenuProcedure>(procedureOwner);
        }

        private void LoadData()
        {
            //加载关卡相关
            LevelDataControl.Instance.OnLoad();
            lockBtn = false;
        }

        private void LevelSelect(LevelDataBase level)
        {
            if(lockBtn) return;
            Log.Debug("选择场景: " + level.Id);
            lockBtn = true;
            LoadLevelScene(level).Forget();
        }

        private async UniTaskVoid LoadLevelScene(LevelDataBase level)
        {
            AssetsPathData sceneAsset = AssetsDataLoader.Instance.GetItemConfig(level.SceneData.AssetPath);
            if (sceneAsset == null || string.IsNullOrEmpty(sceneAsset.ResourcesName))
            {
                lockBtn = false;
                Log.Error("Can not load scene asset config for level '{0}'.", level.Id);
                return;
            }

            try
            {
                LevelDataControl.Instance.LoadLevel(level.Id);
                await PackageSceneLoader.LoadSceneAsync(level.PackageName, sceneAsset.ResourcesName);
                GameEvent.Send(LevelEvent.OnLoadLevelFinish, level.SceneData.Id);
                ChangeState<LevelProcedure>(procedureOwner);
            }
            catch (System.Exception exception)
            {
                lockBtn = false;
                Log.Error("Load level scene failed. LevelId:{0}, Scene:{1}, Error:{2}", level.Id, sceneAsset.ResourcesName, exception);
            }
        }
    }
}

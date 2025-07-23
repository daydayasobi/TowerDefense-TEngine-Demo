using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;
using YooAsset;

namespace GameLogic
{
    public class ChangeSceneProcedure : ProcedureBase
    {
        IFsm<IProcedureModule> procedureOwner;

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
        }

        private void LevelSelect(LevelDataBase level)
        {
            Log.Debug("选择场景: " + level.Id);
            LoadLevelScene(level).Forget();
        }

        private async UniTaskVoid LoadLevelScene(LevelDataBase level)
        {
            LevelDataControl.Instance.LoadLevel(level.Id);
            GameEvent.Send(LevelEvent.OnLoadLevelFinish, level.Id);
            if (level.Id == 4)
            {
                await GameModule.Scene.LoadSceneAsync("Level4Package", AssetsDataLoader.Instance.GetItemConfig(level.SceneData.AssetPath).ResourcesName);
            }
            else
            {
                await GameModule.Scene.LoadSceneAsync(AssetsDataLoader.Instance.GetItemConfig(level.SceneData.AssetPath).ResourcesName);
            }

            ChangeState<LevelProcedure>(procedureOwner);
        }
    }
}
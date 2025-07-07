using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class ChangeSceneProcedure: ProcedureBase
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
            GameEvent.RemoveEventListener<LevelDataBase>(ChangeSceneEvent.LevelSelect,LevelSelect);
        }
        
        private void MenuSelect()
        {
            ChangeState<MainMenuProcedure>(procedureOwner);
        }

        private void LoadData()
        {
            //加载关卡相关
            DataLevelManager.Instance.OnLoad();
            
            // TODO: 需要判空
        }
        
        private void LevelSelect(LevelDataBase level)
        {
            Log.Debug("选择场景: " + level.Id);
            TestLevel1().Forget();
            // LevelDataManger.Instance.LoadLevel(level.Id);
            // GameModule.Scene.LoadScene(SceneDataManger.Instance.GetItemConfig(level.SceneId).SceneName,
            //     progressCallBack: (Progress) => { 
            //         if (Progress == 1) ChangeState<LevelProcedure>(procedureOwner);
            //     });
        }

        private async UniTaskVoid TestLevel1()
        {
            // LevelDataLoader.Instance.LoadLevel(1);
            DataLevelManager.Instance.OnLoad();
            DataLevelManager.Instance.LoadLevel(1);
            await GameModule.Scene.LoadSceneAsync("Level1");
            ChangeState<LevelProcedure>(procedureOwner);
        }
    }
}

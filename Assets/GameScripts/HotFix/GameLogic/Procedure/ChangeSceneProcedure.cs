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
            // GameEvent.AddEventListener(ChangeSceneEvent.MenuSelect, MenuSelect);
            // GameEvent.AddEventListener<LevelData>(ChangeSceneEvent.LevelSelect, LevelSelect);
            TestLevel1();
        }

        protected override void OnLeave(IFsm<IProcedureModule> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            Log.Debug("关闭场景流程");
            GameModule.UI.CloseUI<MainMenuUI>();
            // GameEvent.RemoveEventListener(ChangeSceneEvent.MenuSelect, MenuSelect);
            // GameEvent.RemoveEventListener<LevelData>(ChangeSceneEvent.LevelSelect,LevelSelect);
        }
        
        private void MenuSelect()
        {
            ChangeState<MainMenuProcedure>(procedureOwner);
        }

        private async UniTaskVoid TestLevel1()
        {
            await GameModule.Scene.LoadSceneAsync("Level1");
            ChangeState<LevelProcedure>(procedureOwner);
        }
    }
}

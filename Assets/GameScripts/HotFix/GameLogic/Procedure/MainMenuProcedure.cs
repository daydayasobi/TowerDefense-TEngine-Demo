using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class MainMenuProcedure : ProcedureBase
    {
        IFsm<IProcedureModule> procedureOwner;

        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            this.procedureOwner = procedureOwner;
            base.OnInit(procedureOwner);
            GameEvent.AddEventListener(MainMenuEvent.ChangeLevelSelect, ChangeLevelSelect);
            GameEvent.AddEventListener(MainMenuEvent.OpenOptions, OpenOptions);
            GameEvent.AddEventListener(MainMenuEvent.QuitGame, QuitGame);
            GameEvent.AddEventListener(MainMenuEvent.OpenMenu, OpenMenu);
            GameModule.Scene.LoadScene("Menu");
            GameModule.UI.ShowUIAsync<UIMainMenuForm>();
        }

        protected override void OnLeave(IFsm<IProcedureModule> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameModule.UI.CloseUI<UIMainMenuForm>();
            GameEvent.RemoveEventListener(MainMenuEvent.ChangeLevelSelect, ChangeLevelSelect);
            GameEvent.RemoveEventListener(MainMenuEvent.OpenOptions, OpenOptions);
            GameEvent.RemoveEventListener(MainMenuEvent.QuitGame, QuitGame);
            GameEvent.RemoveEventListener(MainMenuEvent.OpenMenu, OpenMenu);
        }
        
        #region 菜单事件
        private void ChangeLevelSelect()
        {
            ChangeState<ChangeSceneProcedure>(procedureOwner);
        }
        private void OpenOptions()
        {
            GameModule.UI.ShowUI<UIOptionsForm>();
            GameModule.UI.CloseUI<UIMainMenuForm>();
        }
        private void OpenMenu()
        {
            GameModule.UI.CloseUI<UIOptionsForm>();
            GameModule.UI.ShowUI<UIMainMenuForm>();
        }
        private void QuitGame()
        {
            GameModule.Shutdown();
        }
        #endregion
    }
}

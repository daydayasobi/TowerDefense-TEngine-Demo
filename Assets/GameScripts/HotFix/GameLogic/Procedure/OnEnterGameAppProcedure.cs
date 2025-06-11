using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class OnEnterGameAppProcedure : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("OnEnter GameApp Procedure");
            ChangeState<MainMenuProcedure>(procedureOwner);
        }
    }
}

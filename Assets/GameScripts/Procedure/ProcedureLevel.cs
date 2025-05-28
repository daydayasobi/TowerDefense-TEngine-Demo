using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace Procedure
{
    public class ProcedureLevel : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        protected override void OnInit(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
        }

        protected override void OnLeave(IFsm<IProcedureModule> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
    }
}


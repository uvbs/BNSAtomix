﻿using System.Collections.Generic;
using SmartEngine.Network.Tasks;
using SagaBNS.Common.Skills;

namespace SagaBNS.GameServer.Skills.SkillHandlers.BladeMaster
{
    public class TurningStrike : ISkillHandler
    {
        private readonly uint additionID;
        public TurningStrike(uint additionID)
        {
            this.additionID = additionID;
        }
        #region ISkillHandler 成员
        public void HandleOnSkillCasting(SkillArg arg)
        {
        }

        public void HandleOnSkillCastFinish(SkillArg arg)
        {
        }

        public void HandleSkillActivate(SkillArg arg)
        {
            SkillManager.Instance.DoAttack(arg);
            foreach (SkillAffectedActor i in arg.AffectedActors)
            {
                SkillAttackResult res = i.Result;
                i.NoDamageBroadcast = true;
                if (res != SkillAttackResult.Avoid && res != SkillAttackResult.Miss && res != SkillAttackResult.Parry && res != SkillAttackResult.TotalParry)
                {
                    if (i.Target.Tasks.ContainsKey("ActorDown"))
                    {
                        Buff buff = i.Target.Tasks["ActorDown"] as Buff;
                        buff.Deactivate();
                    }

                    Common.Additions.ActorDown add = new Common.Additions.ActorDown(arg, i.Target, additionID, i.Damage, 3000);

                    i.Target.Tasks["ActorDown"] = add;
                    add.Activate();
                }
            }
        }

        public void OnAfterSkillCast(SkillArg arg)
        {
        }
        #endregion
    }
}

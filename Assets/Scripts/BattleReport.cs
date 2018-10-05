using System.Collections.Generic;
using System.Text;
//using replayEditor.Game;

namespace N2
{
    [System.Serializable]
    public class BatttleReportTeammember
    {
        public string UnitId;
        public int HP;
        public int Position;

        private string _playerID;
        private string _ID;

        public override string ToString()
        {
            return "UnitID:" + UnitId + " hp:" + HP + " at "+Position;//++
        }
        public string GetPlayerID()
        {
            if (string.IsNullOrEmpty(_playerID))
                _playerID = BattleReport.UnPackPlayerID(UnitId);
            return _playerID;
        }

        public string GetID()
        {
            if (string.IsNullOrEmpty(_ID))
                _ID = BattleReport.UnPackID(UnitId);
            return _ID;
        }
    }

    [System.Serializable]
    public class BattleRoundAction_RoundBeginEnd
    {
        public List<BattleRoundAction_Effect> Effects;
        public void Output(string prefix)
        {
            UtilLog.Log("\t"+ prefix);
            if (Effects != null )
                foreach (var eff in Effects)
                {
                    UtilLog.Log("\t\t" + eff.ToString());
                }
        }
    }

    [System.Serializable]
    public class BattleRoundAction_DelaySkill
    {
        public string Caster;
        public int SkillID;

        private string _casterID;
        public override string ToString()
        {
            return "caster:" + Caster + " skill:" + SkillID;
        }
        public string GetCasterID()
        {
            if (string.IsNullOrEmpty(_casterID))
            {
                _casterID = BattleReport.UnPackID(Caster);
            }
            return _casterID;            
        }
    }


    [System.Serializable]
    public class BattleRoundAction_NormalRangeLess
    {
        public string Caster;
    }

    [System.Serializable]
    public class BattleRoundAction_Effect
    {
        public string NodeID;
        public int EffectID;
        public int EffectTypeID;
        public int StateID;
        public string Caster;
        public List<string> Targets;
        public int SkillID;
        public List<int> Recover;
        public List<int> Damage;
        public List<int> LeftHP;
        public List<int> calcRecover;
        public List<int> CalcDamage;
        public List<int> Value;
        public List<int> ResultValue;
        public int ValueType;
        public List<int> AuraEffectType;
        public List<int> BuffEffectType;
        public List<int> DotEffectType;
        public List<int> RemoveEffectTypeID;
        public List<int> Result;


        public string TypeName;
        public string StateName;

        private string _casterID;
        private string _casterPlayerID;
        private List<string> _targetID;
        private List<string> _targetPlayerID;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(TypeName))
            {
                sb.Append("Effect:"+TypeName + " caster:" + GetCasterID() + " skill:" + SkillID + " EID:" + EffectID + " ETID:" + EffectTypeID);
                sb.Append(" Targets:");
                if (Targets != null)
                {
                    foreach (string t in Targets)
                        sb.Append( t + " ");
                }
                if (Recover != null && Recover.Count > 0)
                    sb.Append ("Recover");
                if (Damage != null && Damage.Count > 0)
                    sb.Append("Damage");
                if (LeftHP != null && LeftHP.Count > 0)
                    sb.Append("LeftHP");
                if(Value != null && Value.Count > 0)
                    sb.Append("Value");
                if (ResultValue != null && ResultValue.Count > 0)
                    sb.Append("ResultValue");
                if (AuraEffectType != null && AuraEffectType.Count > 0)
                    sb.Append("AuraEffectType");
                if (BuffEffectType != null && BuffEffectType.Count > 0)
                    sb.Append("BuffEffectType");
                if (DotEffectType != null && DotEffectType.Count > 0)
                    sb.Append("DotEffectType");
                if (RemoveEffectTypeID != null && RemoveEffectTypeID.Count > 0)
                    sb.Append("RemoveEffectTypeID");
                if (Result != null && Result.Count > 0)
                    sb.Append("Result");
            }
            else
            {
                sb.Append("State: target(");
                foreach (string t in Targets)
                    sb.Append(t + " ");
                sb.Append(") enter " + StateName +" id:"+ StateID);// + Target + " skill:" + SkillID + " EID:" + EffectID + " ETID:" + EffectTypeID)
            }
            return  sb.ToString();
        }

        public string GetCasterID()
        {
            if (string.IsNullOrEmpty(_casterID))
            {
                _casterID = BattleReport.UnPackID(Caster);
            }
            return _casterID;
        }

        public string GetPlayerID()
        {
            if (string.IsNullOrEmpty(_casterPlayerID))
            {
                _casterPlayerID = BattleReport.UnPackPlayerID(Caster);
            }
            return _casterPlayerID;        
        }

        public string GetTargetID(int index)
        {
            if (string.IsNullOrEmpty(_casterPlayerID))
            {
                _casterPlayerID = BattleReport.UnPackPlayerID(Caster);
            }
            return _casterPlayerID;
        }

    }

    [System.Serializable]
    public class BattleRoundAction_State
    {
        public int StateID;
        public int NodeID;
        public List<string> Targets;
        public string StateName;
        public override string ToString()
        {
            string tmp = "StateID:" + StateID + " node:" + NodeID + " Targets:";
            if (Targets != null)
            {
                foreach (string t in Targets)
                    tmp += t + " ";
            }
            return tmp;
        }
    }

    [System.Serializable]
    public class BattleRoundAction_ActiveSkill
    {
        public string Caster;
        public List<string> Targets;
        public int SkillID;
        public List<BattleRoundAction_Effect> Effects;

        public void Output(string skillPre)
        {
            if (!string.IsNullOrEmpty(Caster) && SkillID != 0)
            {
                UtilLog.Log("\t" + skillPre + " caster:" + Caster + " skill:" + SkillID);
            }
            else
            {
                UtilLog.Log("\t" + skillPre + " caster:" + Caster);
            }

            if (Effects != null)
            {
                foreach (var eff in Effects)
                {
                    UtilLog.Log("\t\t" + eff.ToString());
                }
            }
        }
        public string GetCasterID()
        {
            return Caster.Substring(Caster.IndexOf("-")+1);
        }

        public string GetPlayerID()
        {
            return Caster.Substring(0,Caster.IndexOf("-"));
        }
    }

    [System.Serializable]
    public class PassiveRoundAction
    {
        public BattleRoundAction_ActiveSkill PassiveSkill;

        public void Output()
        {
            if (PassiveSkill != null)
                PassiveSkill.Output("PassiveSkill");
        }
    }

    [System.Serializable]
    public class PassiveRoundItem
    {
        //[Newtonsoft.Json.JsonProperty]
        public int RoundType;
        public List<PassiveRoundAction> ActionList;
        private string Name = "PassiveRound";
        public void Output()
        {
            UtilLog.Log("PassiveRound RoundType:" + RoundType);
            foreach (var action in ActionList)
            {
                //Debug.Log(action.ToString());
                action.Output();
            }
        }

        public string GetName()
        {
            return Name;
        }
    }

    [System.Serializable]
    public class PrepareRoundAction
    {
        public BattleRoundAction_ActiveSkill CommandSkill;
        public void Output()
        {
            if (CommandSkill != null)
                CommandSkill.Output("CommandSkill");
        }
    }

    [System.Serializable]
    public class PrepareRoundItem
    {
        public int RoundType;
        public List<PrepareRoundAction> ActionList;
        private string Name = "PrepareRound";
        public void Output()
        {
            UtilLog.Log("PrepareRound RoundType:" + RoundType);
            foreach (var action in ActionList)
            {
                //Debug.Log(action.ToString());
                action.Output();
            }
        }

        public string GetName()
        {
            return Name;
        }
    }

    [System.Serializable]
    public class BattleRoundAction
    {
        public BattleRoundAction_RoundBeginEnd RoundBegin;
        public BattleRoundAction_ActiveSkill BeforeAction;
        public BattleRoundAction_DelaySkill DelayRound;
        public BattleRoundAction_DelaySkill NormalRangeLess;
        public BattleRoundAction_ActiveSkill ActiveSkill;
        public BattleRoundAction_ActiveSkill NormalAttack;
        public BattleRoundAction_RoundBeginEnd RoundEnd;

        public void Output()
        {
            //Debug.Log("BattleRoundAction.Output");
            if (RoundBegin != null && RoundBegin.Effects!= null)
                RoundBegin.Output("RoundBegin");
            if (BeforeAction != null)
            {
                //UtilLog.Log();
                BeforeAction.Output("BeforeAction");
            }                
            if (DelayRound != null && DelayRound.Caster != null)
               UtilLog.Log("\tdelayRound:" + DelayRound.ToString());
            if (NormalRangeLess != null && NormalRangeLess.Caster != null)
               UtilLog.Log("\tNormalRangeLess:" + NormalRangeLess.ToString());
            if (ActiveSkill != null && ActiveSkill.Caster != null)
            {
                //System.Console.Write("\tActiveSkill ");
                ActiveSkill.Output("ActiveSkill");
            }                
            if (NormalAttack != null && NormalAttack.Caster != null)
            {
                //System.Console.Write("NormalAttack ");
                NormalAttack.Output("NormalAttack");
            }            
            if (RoundEnd != null && RoundEnd.Effects != null)
            {
                RoundEnd.Output("RoundEnd");
               //UtilLog.Log();
            }
                
        }



    }





    [System.Serializable]
    public class BattleRoundItem
    {
        public int RoundType;
        public List<BattleRoundAction> ActionList;

        //public override string ToString()
        //{
        //   UtilLog.Log("RoundType:"+RoundType);
        //    foreach ( var action in ActionList)
        //    {
        //       UtilLog.Log();
        //    }
        //    return "";
        //}

        public void Output()
        {
           UtilLog.Log("BattleRound RoundType:" + RoundType);
            foreach (var action in ActionList)
            {
                //Debug.Log(action.ToString());
                action.Output();
            }
        }
    }

    [System.Serializable]
    public class BattleReport
    {
        public List<BatttleReportTeammember> TeamBlue;// =  new BattleReportTeam();
        public List<BatttleReportTeammember> TeamRed;// = new BattleReportTeam();
        public PassiveRoundItem PassiveRound;
        public PrepareRoundItem PrepareRound;
        public List<BattleRoundItem> BattleRound;

        public void Output()
        {
            UtilLog.Log("BattleReportTeam");
            UtilLog.Log("TeamBlue:");
            foreach (var team in TeamBlue)
                UtilLog.Log(team.ToString());
            UtilLog.Log("TeamRed:");
            foreach (var team in TeamRed)
                UtilLog.Log(team.ToString());

            UtilLog.Log();
            PassiveRound.Output();
            UtilLog.Log();

            PrepareRound.Output();
            UtilLog.Log();

            foreach (var battleItem in BattleRound)
                battleItem.Output();
        }

        public static string UnPackID(string packedID)
        {
            if (!string.IsNullOrEmpty(packedID) && packedID.IndexOf("-") != -1)
                return packedID.Substring(packedID.IndexOf("-") + 1);
            else
                return "";

      
        }
        public static string UnPackPlayerID(string packedID)
        {
            if (!string.IsNullOrEmpty(packedID) && packedID.IndexOf("-") != -1)
                return packedID.Substring(0, packedID.IndexOf("-"));
            else
                return "";
        }
    }
}

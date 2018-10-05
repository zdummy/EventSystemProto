using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
//using System.Threading.Tasks;


namespace N2
{
    public class RoundEventArgs : EventArgs
    {
        public string RoundInfo;
        public RoundEventArgs(string roundInfo)
        {
            RoundInfo = roundInfo;
        }

        public override string ToString()
        {
            return RoundInfo;
        }
    }

    public class ActiveSkillEventArgs: EventArgs
    {
        public int PlayerID;
        public int Caster;
        public List<int> TargetPlayerIDs = new List<int>();
        public List<int> Targets = new List<int>();
        public int SkillID;
        public int EffectID;
        public int EffectTypeID;
        public string effName;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string result = effName + " caster:" + Caster+"";
            if (SkillID != 0)
                result += " cast skill(" + SkillID + ")";
          
            if (Targets != null && Targets.Count > 0)
            {
                result += " on:";
                foreach (int t in Targets)
                    result += t + " ";
            }        
            if (EffectID != 0  || EffectTypeID != 0)
            {
                result += "effectID:" + EffectID + " EffectTypeID:" + EffectTypeID;
            }
            return result;// + "effectID:"+ EffectID + " EffectTypeID:"+EffectTypeID;
        }
        public string GetTargetsString()
        {
            string result = "";
            if (Targets != null && Targets.Count > 0)
            {
                foreach (int t in Targets)
                    result += t + " ";
            }
            return result;
        }
    }
    
    public class Effect_DamageCureEventArgs : ActiveSkillEventArgs
    {
        public List<int> HPChange = new List<int>();
        public List<int> LeftHP = new List<int>();        
        //public List<int CalcHPChange;

        public override string ToString()
        {
            string result =  base.ToString() + " ";
            if (HPChange != null)
            {
                for (int i = 0; i < HPChange.Count;i++)
                    result += HPChange[i] + "("+ LeftHP[i]+") ";
            }
            return result;
        }
    }
    public enum BuffAuraDot_TYPE
    {
        Aura = 0,
        Buff,
        Dot,
        RemoveEffect
    }
    public class Effect_BuffAuraDotEventArgs : ActiveSkillEventArgs
    {
        public List<BuffAuraDot_TYPE> eventType = new List<BuffAuraDot_TYPE>();
        public List<int> BuffAuraDotID = new List<int>();
        public override string ToString()
        {
            string result =  base.ToString() + " ";
            if (BuffAuraDotID.Count > 0)
            {
                for (int i = 0; i < BuffAuraDotID.Count; i++)
                {
                    result += eventType[i] + ":" + BuffAuraDotID[i];

                }
                   
            }
            return result;
        }    
    }
    public class Effect_BuffAuraDotValueEventArgs : ActiveSkillEventArgs
    {
        public List<int> ValueChange = new List<int>();
        public List<int> ResultValue = new List<int>();
        public List<int> CalcValueChange = new List<int>();
        public bool bUsePercent;

        public override string ToString()
        {
            string result = base.ToString() + "  value:";
            if (ValueChange.Count > 0)
            {
                for (int i = 0; i < ValueChange.Count; i++)
                {
                    result +=+ValueChange[i] + "(" + ResultValue[i] + ") ";
                }
            }
            return result;
        }
    }
    public class Effect_STATEEventArgs : ActiveSkillEventArgs
    {
        public int StateID;
        public bool bEnter;
        public int result;
        //public int CalcHPChange;
        public override string ToString()
        {
            string result = "";
            if (Targets != null)
            {
                foreach (int t in Targets)
                    result += t + " ";
            }
            return result + " Enter state:" + StateID;
        }
    }

    public class EventItem
    {
        public float timer;
        public int type;
        public EventArgs eventArgs;

        public EventItem(float _timer, int eventInternalType, EventArgs args)
        {
            timer = _timer;
            type = eventInternalType;
            eventArgs = args;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1} {2}", ReplayTimer.FormatTimer(timer), EventManager.GetEventType(type), eventArgs.ToString());

            //"[" + ReplayTimer.FormatTimer(timer) + "] " + EventManager.GetEventType(type) + eventArgs.ToString();
        }
    }



    public class EventManager
    {
        public static string GetEventType(int eventInternalType)
        {
            switch(eventInternalType)
            {
                case EVENT_CommandSkill: return "commandskill";
                case EVENT_PassiveSkill: return "PassiveSkill";
                case EVENT_ActiveSkill: return "ActiveSkill";
                case EVENT_NormalAttack: return "NormalAttack";
                case EVENT_Effect_DamageCure: return "    Effect_DamageCure";
                case EVENT_Effect_BuffAuraDot: return "    Effect_BufAuraDot";
                case EVENT_Effect_BuffAuraDotValue: return "    Effect_BufAuraDotValue";
                case EVENT_Effect_State: return "    Effect_State";
                case EVENT_EnterRound: return "EnterRound";
                case EVENT_BeforeAction: return "BeforeAction";
                case EVENT_RoundBegin: return "RoundBegin";
                case EVENT_RoundEnd: return "RoundEnd";
                case EVENT_GameOver: return "GameOver";
            }
            return "Unkown";
        }

        //hero events
        public event EventHandler<ActiveSkillEventArgs> CommandSkill;
        public event EventHandler<ActiveSkillEventArgs> PassiveSkill;
        public event EventHandler<ActiveSkillEventArgs> ActiveSkill;
        public event EventHandler<ActiveSkillEventArgs> NormalAttack;
        public event EventHandler<Effect_DamageCureEventArgs> Effect_DamageCure;
        public event EventHandler<Effect_BuffAuraDotEventArgs> Effect_BuffAuraDot;
        public event EventHandler<Effect_BuffAuraDotValueEventArgs> Effect_BuffAuraDotValue;
        public event EventHandler<Effect_STATEEventArgs> Effect_State;
        //scene events
        public event EventHandler<RoundEventArgs> EnterRound;
        public event EventHandler<RoundEventArgs> GameOver;


        private const int EVENT_CommandSkill = 0;
        private const int EVENT_PassiveSkill = 1;
        private const int EVENT_ActiveSkill = 2;
        private const int EVENT_NormalAttack = 3;
        private const int EVENT_Effect_DamageCure = 4;
        private const int EVENT_Effect_BuffAuraDot = 5;
        private const int EVENT_Effect_BuffAuraDotValue = 7;
        private const int EVENT_Effect_State = 8;
        private const int EVENT_BeforeAction = 9;
        private const int EVENT_RoundBegin = 10;
        private const int EVENT_RoundEnd = 11;

        private const int EVENT_GameOver = 13;
        private const int EVENT_EnterRound = 20;


        private Queue<EventItem> m_eventQueue = new Queue<EventItem>();
        private List<EventItem> m_backupEventQueue = new List< EventItem >();
        private static EventManager _instance;
        private N2.BattleReport m_battleReport;//= new N2.BattleReport();
        private float m_baseTimer;

        private ReplayTimer m_timer = new ReplayTimer();

        private EventManager()
        {
            m_eventQueue.Clear();
            //m_battleReport = JsonUtility.FromJson<N2.BattleReport>(File.ReadAllText("/work/VFXThumbnail/Assets/Data/result.json", Encoding.UTF8));
            //m_battleReport = JsonConvert.DeserializeObject<N2.BattleReport>(File.ReadAllText("result.json", Encoding.UTF8));
            //m_battleReport.Output();
            //foreach (var item in m_eventQueue)
            //{
            //    UtilLog.Log(item.ToString());
            //}
        }

        ~EventManager()
        {
            m_eventQueue.Clear();
        }

        public void BuildEventQueue(N2.BattleReport battleReport, float baseTimer)
        {
            m_battleReport = battleReport;
            BuildEventSequece();
            foreach (var item in m_eventQueue)
                m_backupEventQueue.Add(item);
            //m_backupEventQueue = m_eventQueue.CopyTo()
        }

        public void StartGame()
        {
            float timer = 0;
            while (m_eventQueue.Count > 0)
            {
                EventItem item = m_eventQueue.Peek();
                if (item.timer < timer)
                {                    
                    //UtilLog.Log(ReplayTimer.FormatTimer(timer) + " ");
                    FireEvent(item);
                    m_eventQueue.Dequeue();
                }
                timer += 1;
            }
        }

        public void DispatchEvent()
        {
            float passedTime = Time.time - m_baseTimer; //get delta time
            if (m_eventQueue.Count > 0)
            {
                EventItem item = m_eventQueue.Peek();

                //UtilLog.Log("CheckTimer, cur:"+ passedTime + " event:"+item.timer);
                if (item.timer < passedTime)
                {
                    //UtilLog.Log(ReplayTimer.FormatTimer(passedTime) + " ");
                    FireEvent(item);
                    m_eventQueue.Dequeue();
                    if (m_eventQueue.Count == 0)
                    {
                        FireEvent(new EventItem(0, EVENT_GameOver, new RoundEventArgs("GameOver")));
                    }
                }
            }             
        }

        public static EventManager Instance()
        {
            if (_instance == null)
                _instance = new EventManager();
            return _instance;
        }

        protected virtual void OnCommandSkillEvent(ActiveSkillEventArgs e)
        {
            //EventHandler<ActiveSkillEventArgs> temp = Volatile.Read(ref ActiveSkill);
            if (CommandSkill != null)
            {
                CommandSkill(this, e);
            }
        }
        protected virtual void OnPassiveSkillEvent(ActiveSkillEventArgs e)
        {
            //EventHandler<ActiveSkillEventArgs> temp = Volatile.Read(ref ActiveSkill);
            if (PassiveSkill != null)
            {
                PassiveSkill(this, e);
            }
        }
        protected virtual void OnActiveSkillEvent(ActiveSkillEventArgs e)
        {
            //EventHandler<ActiveSkillEventArgs> temp = Volatile.Read(ref ActiveSkill);
            if (ActiveSkill != null)
            {
                ActiveSkill(this, e);
            }
        }
        protected virtual void OnNormalAttacklEventt(ActiveSkillEventArgs e)
        {
            if (NormalAttack != null)
            {
                NormalAttack(this, e);
            }
        }

        protected virtual void OnEffect_DamageCureEvent(Effect_DamageCureEventArgs e)
        {
            if (Effect_DamageCure != null)
            {
                Effect_DamageCure(this, e);
            }
        }
        protected virtual void OnEffect_BUFFAURADOTEvent(Effect_BuffAuraDotEventArgs e)
        {
            if (Effect_BuffAuraDot != null)
            {
                Effect_BuffAuraDot(this, e);
            }
        }
        protected virtual void OnEffect_BUFFAURADOTValueEvent(Effect_BuffAuraDotValueEventArgs e)
        {
            if (Effect_BuffAuraDotValue != null)
            {
                Effect_BuffAuraDotValue(this, e);
            }
        }
        protected virtual void OnEffect_STATEEvent(Effect_STATEEventArgs e)
        {
            if (Effect_State != null)
            {
                Effect_State(this, e);
            }
        }

        protected virtual void OnEnterRound(RoundEventArgs e)
        {
            if (EnterRound != null)
            {
                EnterRound(this, e);
            }
        }
        protected virtual void OnGameOver(RoundEventArgs e)
        {
            if (GameOver != null)
            {
                GameOver(this, e);
            }
        }

        protected void FireEvent(EventItem item)
        {
            switch (item.type)
            {
                case EVENT_CommandSkill:
                    OnCommandSkillEvent(item.eventArgs as ActiveSkillEventArgs);
                    break;
                case EVENT_PassiveSkill:
                    OnPassiveSkillEvent(item.eventArgs as ActiveSkillEventArgs);
                    break;
                case EVENT_ActiveSkill:
                    OnActiveSkillEvent(item.eventArgs as ActiveSkillEventArgs);
                    break;
                case EVENT_NormalAttack:
                    OnNormalAttacklEventt(item.eventArgs as ActiveSkillEventArgs);
                    break;
                case EVENT_Effect_DamageCure:
                    OnEffect_DamageCureEvent(item.eventArgs as Effect_DamageCureEventArgs);
                    break;
                case EVENT_Effect_BuffAuraDot:
                    OnEffect_BUFFAURADOTEvent(item.eventArgs as Effect_BuffAuraDotEventArgs);
                    break;
                case EVENT_Effect_BuffAuraDotValue:
                    OnEffect_BUFFAURADOTValueEvent(item.eventArgs as Effect_BuffAuraDotValueEventArgs);
                    break;
                case EVENT_Effect_State:
                    OnEffect_STATEEvent(item.eventArgs as Effect_STATEEventArgs);
                    break;
                case EVENT_EnterRound:
                    OnEnterRound(item.eventArgs as RoundEventArgs);
                    break;
                case EVENT_GameOver:
                    OnGameOver(item.eventArgs as RoundEventArgs);
                    break;
            }
        }
        protected void Update()
        {
            if (m_eventQueue.Count > 0)
            {
                EventItem item = m_eventQueue.Last();
                if (item.timer > 0)
                {
                    FireEvent(item);
                }
            }
        }

        protected void BuildEventSequece()
        {
            //UtilLog.Log("BuildEventSequece");
            m_timer.Reset();
            float roundBeginTimer = m_timer.NextRoundBegin();
            m_eventQueue.Enqueue(new EventItem(roundBeginTimer, EVENT_EnterRound, new RoundEventArgs("PassiveRound")));
            //UtilLog.Log(m_eventQueue.Last().ToString()); 
            ProduceEvents(m_battleReport.PassiveRound);

            m_eventQueue.Enqueue(new EventItem(m_timer.NextRoundBegin(roundBeginTimer), EVENT_EnterRound, new RoundEventArgs("PrepareRound")));
            //UtilLog.Log(m_eventQueue.Last().ToString()); 
            ProduceEvents(m_battleReport.PrepareRound);

            foreach (var battleRoundItem in m_battleReport.BattleRound)
            {
                m_eventQueue.Enqueue(new EventItem(m_timer.NextRoundBegin(roundBeginTimer), EVENT_EnterRound, new RoundEventArgs("BattleRound")));
                //UtilLog.Log(m_eventQueue.Last().ToString());
                ProduceEvents(battleRoundItem);
            }
        }

        protected void ProduceEvents(PassiveRoundItem pri)
        {
            float curTime = m_timer.NextTimeStamp();
            foreach (N2.PassiveRoundAction item in pri.ActionList)
            {
                N2.BattleRoundAction_ActiveSkill skillEvent = item.PassiveSkill;
                ProduceEvents(skillEvent, "PassiveSkill");
                m_timer.NextTimeStampSkill();
            }
        }

        protected void ProduceEvents(PrepareRoundItem pri)
        {
            float curTime = m_timer.NextTimeStamp();
            foreach (N2.PrepareRoundAction item in pri.ActionList)
            {
                N2.BattleRoundAction_ActiveSkill skillEvent = item.CommandSkill;
                ProduceEvents(skillEvent, "CommandSkill");
                m_timer.NextTimeStampSkill();
            }
        }

        protected void ProduceEvents(BattleRoundItem bri)
        {
            float curTime = m_timer.NextTimeStamp();
            foreach (N2.BattleRoundAction item in bri.ActionList)
            {
                if (item.RoundBegin != null && item.RoundBegin.Effects != null)
                {
                   ProduceEvents(item.RoundBegin, "RoundBegin");
                }
                else if (item.BeforeAction != null && item.BeforeAction.Effects != null)
                {
                    //UtilLog.Log();
                    ProduceEvents(item.BeforeAction, "BeforeAction"); 
                    //BeforeAction.Output("BeforeAction");
                }
                else if (item.DelayRound != null && item.DelayRound.Caster != null)
                    ProduceEvents(item.DelayRound, "DelayRound");
                else if (item.NormalRangeLess != null && item.NormalRangeLess.Caster != null)
                    ProduceEvents(item.NormalRangeLess, "NormalRangeLess");
                else if (item.ActiveSkill != null && item.ActiveSkill.Caster != null)
                {
                    ProduceEvents(item.ActiveSkill, "ActiveSkill");
                    m_timer.NextTimeStampSkill();
                }
                else if (item.NormalAttack != null && item.NormalAttack.Caster != null)
                {
                    ProduceEvents(item.NormalAttack, "NormalAttack");
                }
                else if (item.RoundEnd != null && item.RoundEnd.Effects != null)
                {
                    ProduceEvents(item.RoundEnd, "RoundEnd");
                }
            }
        }

        protected void ProduceEvents(N2.BattleRoundAction_DelaySkill delaySkill, string prefix = "")
        {
            //if (!string.IsNullOrEmpty(skillEvent.Caster))
            //{
            //    //UtilLog.Log("found caster " + skillEvent.Caster);
            //    ActiveSkillEventArgs args = new ActiveSkillEventArgs();
            //    //ev = new ReplayEvent(prefix, reportFromationTable.GetEffectStatePattern(prefix));
            //    args.PlayerID = int.Parse(skillEvent.GetPlayerID());
            //    args.Caster = int.Parse(skillEvent.GetCasterID());
            //    args.SkillID = skillEvent.SkillID;
            //    //todo ,could not get effect/effectType ID here right now
            //    //args.EffectID = skillEvent.
            //    //args.EffectTypeID = skillEvent.
            //    if (skillEvent.Targets != null && skillEvent.Targets.Count > 0)
            //    {
            //        string targetStr = skillEvent.Targets[0];
            //        args.Targets.Add(int.Parse(targetStr.Substring(targetStr.IndexOf("-") + 1)));
            //        if (skillEvent.Targets.Count > 1)
            //        {
            //            for (int i = 1; i < skillEvent.Targets.Count; i++)
            //            {
            //                targetStr = skillEvent.Targets[i];
            //                args.Targets.Add(int.Parse(targetStr.Substring(targetStr.IndexOf("-") + 1)));
            //            }
            //        }
            //    }
            //    int internalEventType = EVENT_ActiveSkill;
            //    if (prefix.Equals("CommandSkill"))
            //        internalEventType = EVENT_CommandSkill;
            //    else if (prefix.Equals("PassiveSkill"))
            //        internalEventType = EVENT_PassiveSkill;
            //    m_eventQueue.Enqueue(new EventItem(m_timer.GetCurrent(), internalEventType, args));
            //    UtilLog.Log(m_eventQueue.Last().ToString());
            //}
        }

        protected void ProduceEvents(N2.BattleRoundAction_RoundBeginEnd roundBeginEnd, string prefix = "")
        {
           if (roundBeginEnd.Effects != null)
                ProduceEvents(roundBeginEnd.Effects, 0); //no skillID from parent
        }

        protected void ProduceEvents(N2.BattleRoundAction_ActiveSkill skillEvent, string prefix = "")
        {
           //UtilLog.Log("ProduceEvents ActiveSkill "+prefix);
            if (!string.IsNullOrEmpty(skillEvent.Caster))
            {
                if (skillEvent.Effects.Count == 0
                    && skillEvent.Targets.Count == 0)
                    return;

                //UtilLog.Log("found caster " + skillEvent.Caster);
                ActiveSkillEventArgs args = new ActiveSkillEventArgs();
                //ev = new ReplayEvent(prefix, reportFromationTable.GetEffectStatePattern(prefix));
                args.PlayerID = int.Parse(skillEvent.GetPlayerID());
                args.Caster = int.Parse(skillEvent.GetCasterID());
                args.SkillID = skillEvent.SkillID;
                //todo ,could not get effect/effectType ID here right now
                //args.EffectID = skillEvent.
                //args.EffectTypeID = skillEvent.
                if (skillEvent.Targets != null && skillEvent.Targets.Count > 0)
                {
                    string targetStr = skillEvent.Targets[0];
                    args.Targets.Add(int.Parse(targetStr.Substring(targetStr.IndexOf("-") + 1)));
                    if (skillEvent.Targets.Count > 1)
                    {
                        for (int i = 1; i < skillEvent.Targets.Count; i ++)
                        {
                            targetStr = skillEvent.Targets[i];
                            args.Targets.Add(int.Parse(targetStr.Substring(targetStr.IndexOf("-") + 1)));
                        }
                    }
                }

                int internalEventType = EVENT_ActiveSkill;
                if (prefix.Equals("CommandSkill"))
                    internalEventType = EVENT_CommandSkill;
                else if (prefix.Equals("PassiveSkill"))
                    internalEventType = EVENT_PassiveSkill;
                else if (prefix.Equals("NormalAttack"))
                    internalEventType = EVENT_NormalAttack;
                else if (prefix.Equals("BeforeAction"))
                    internalEventType = EVENT_BeforeAction;
                else if (prefix.Equals("RoundBegin"))
                    internalEventType = EVENT_RoundBegin;
                else if (prefix.Equals("RoundEnd"))
                    internalEventType = EVENT_RoundEnd;


                //UtilLog.Log("prefix:" + prefix + " eventType:"+ internalEventType+"/" + EventManager.GetEventType(internalEventType));
                m_eventQueue.Enqueue(new EventItem(m_timer.GetCurrent(), internalEventType, args));
                //UtilLog.Log(m_eventQueue.Last().ToString());
            }
            else{
                //UtilLog.Log("no caster " );
            }

            if (skillEvent.Effects != null)
                ProduceEvents(skillEvent.Effects, skillEvent.SkillID);
        }

        //sometimes no skillID in effect data;
        protected void ProduceEvents(List<N2.BattleRoundAction_Effect> effects, int skillIDFromParent)
        {
            //UtilLog.Log("ProduceEvents BattleRoundActionEffect ");
            foreach (N2.BattleRoundAction_Effect eff in effects)
            {
                //UtilLog.Log("working on eff:"+eff.ToString());
                string effectStr = "";
                int eventInteralType = EVENT_Effect_BuffAuraDot;

                ActiveSkillEventArgs args = null;
                if (!string.IsNullOrEmpty(eff.TypeName))
                {
                    effectStr = eff.TypeName;

                    if (eff.AuraEffectType != null && eff.AuraEffectType.Count > 0
                        || eff.BuffEffectType != null && eff.BuffEffectType.Count > 0
                        || eff.DotEffectType != null && eff.DotEffectType.Count > 0
                        ||  eff.TypeName.Equals("ExitEffectType"))
                    {
                        args = new Effect_BuffAuraDotEventArgs();
                        eventInteralType = EVENT_Effect_BuffAuraDot;
                    }
                    else if (eff.Damage != null && eff.Damage.Count > 0
                             || eff.Recover != null && eff.Recover.Count > 0)
                    {
                        args = new Effect_DamageCureEventArgs();
                        eventInteralType = EVENT_Effect_DamageCure;
                    }
                    else if (eff.Value != null && eff.Value.Count > 0
                             || eff.ResultValue != null && eff.ResultValue.Count > 0)
                    {
                        //todo need more check on data 
                        args = new Effect_BuffAuraDotValueEventArgs();
                        eventInteralType = EVENT_Effect_BuffAuraDotValue;
                    }
                    else
                    {
                        UtilLog.Log("Effect neither damge/cure nor buffAuraDot");
                    }
                }
                else if (!string.IsNullOrEmpty(eff.StateName))
                {
                    effectStr = eff.StateName;
                    args = new Effect_STATEEventArgs();
                    eventInteralType = EVENT_Effect_State;
                }
                else
                {
                    UtilLog.Log("Effect neither typename, nore statename");
                }
                args.effName = effectStr;
                if (eff.Caster != null)
                {
                    args.Caster = int.Parse(eff.GetCasterID());
                }
                //sometimes no skillID in effect data;
                if (eff.SkillID == 0)
                    args.SkillID = skillIDFromParent;
                else
                    args.SkillID = eff.SkillID ;
                
                args.EffectID = eff.EffectID;
                args.EffectTypeID = eff.EffectTypeID;
                if (eventInteralType == EVENT_Effect_BuffAuraDot)
                {
                    Effect_BuffAuraDotEventArgs buffAuraDotArgs = args as Effect_BuffAuraDotEventArgs;
                }
                else if (eventInteralType == EVENT_Effect_State)
                {
                    Effect_STATEEventArgs stateArgs = args as Effect_STATEEventArgs;
                    stateArgs.StateID = eff.StateID;
                }

                //pattern = reportFromationTable.GetEffectStatePattern(effectStr);
                if (eff.Targets != null && eff.Targets.Count > 0)
                {
                    for (int i = 0; i < eff.Targets.Count; i++)
                    {
                        if (eventInteralType == EVENT_Effect_DamageCure)
                        {
                            Effect_DamageCureEventArgs dmgCureArgs = args as Effect_DamageCureEventArgs;
                            if (eff.Damage != null)
                            {
                                dmgCureArgs.HPChange.Add(eff.Damage[i]);                             
                            }
                            else if (eff.Recover != null)
                            {
                                dmgCureArgs.HPChange.Add(eff.Recover[i]);

                            }
                            if (eff.LeftHP != null)
                                dmgCureArgs.LeftHP.Add(eff.LeftHP[i]);
                        }
                        else if (eventInteralType == EVENT_Effect_BuffAuraDot)
                        {
                            Effect_BuffAuraDotEventArgs buffAuraDotArgs = args as Effect_BuffAuraDotEventArgs;
                            if (eff.AuraEffectType != null && eff.AuraEffectType.Count > 0)
                            {
                               //UtilLog.Log("auraEffect has size " + eff.AuraEffectType.Count +" ,we're index:" + i);
                                buffAuraDotArgs.eventType.Add(BuffAuraDot_TYPE.Aura);
                                buffAuraDotArgs.BuffAuraDotID.Add(eff.AuraEffectType[i]);
                            }
                            else if (eff.BuffEffectType != null && eff.BuffEffectType.Count > 0)
                            {
                                buffAuraDotArgs.eventType.Add(BuffAuraDot_TYPE.Buff);
                                buffAuraDotArgs.BuffAuraDotID.Add(eff.BuffEffectType[i]);
                            }
                            else if (eff.DotEffectType != null && eff.DotEffectType.Count > 0)
                            {
                                buffAuraDotArgs.eventType.Add(BuffAuraDot_TYPE.Dot);
                                buffAuraDotArgs.BuffAuraDotID.Add(eff.DotEffectType[i]);
                            }
                            else  if (eff.RemoveEffectTypeID != null && eff.RemoveEffectTypeID.Count > 0)
                            {
                                buffAuraDotArgs.eventType.Add(BuffAuraDot_TYPE.RemoveEffect);
                                buffAuraDotArgs.BuffAuraDotID.Add(eff.RemoveEffectTypeID[i]);
                            }
                            else if (eff.TypeName.Equals("ExitEffectType"))
                            {
                                buffAuraDotArgs.eventType.Add(BuffAuraDot_TYPE.RemoveEffect);
                                buffAuraDotArgs.BuffAuraDotID.Add(eff.EffectTypeID);
                            }
                            //aura/dot/buff has no value
                       }
                        else if (eventInteralType == EVENT_Effect_BuffAuraDotValue)
                        {
                            Effect_BuffAuraDotValueEventArgs buffAuraDotArgs = args as Effect_BuffAuraDotValueEventArgs;
                            if (eff.Value!=null)
                                buffAuraDotArgs.ValueChange.Add(eff.Value[i]);
                            if (eff.ResultValue != null)
                                buffAuraDotArgs.ResultValue.Add(eff.ResultValue[i]);
                            if (eff.Result != null && eff.Result.Count > 0)
                                buffAuraDotArgs.bUsePercent = (eff.Result[0] == 0);
                        }
                        args.TargetPlayerIDs.Add(int.Parse(BattleReport.UnPackPlayerID(eff.Targets[i])));
                        args.Targets.Add(int.Parse(BattleReport.UnPackID(eff.Targets[i])));                                    }                    
                }
                m_eventQueue.Enqueue(new EventItem(m_timer.NextTimeStampEffect(), eventInteralType, args));
                //UtilLog.Log(m_eventQueue.Last().ToString());
            }
        }
    }

    //
    public class ReplayTimer
    {
        public float MinRoundTime = 5;
        public float MaxRoundTime = 15;
        public float ActiveSkillTime = 5;
        public float NormalAttackTime = 5;
        public float EffectTime = 3;
        public float MinimumTimeInterval = 1;

        public static string FormatTimer(float timer)
        {
            int intTimer = (int)Math.Floor(timer);
            int sec = intTimer % 60;
            int min = intTimer / 60;
            int hour = 0;
            if (min > 60)
            {
                hour = min / 60;
                min = min % 60;
            }
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, min, sec);
        }

        private float m_baseTimer = 0;
        public ReplayTimer()
        {
            //m_baseTimer = 0;
            Reset();
        }
        public void Reset()
        {
            m_baseTimer = 0;
        }
        public float NextRoundBegin(float lastRoundBeginTime = 0)
        {
            if (m_baseTimer  - lastRoundBeginTime < MinRoundTime) //make sure each round keep at least minRoundTime
            {
                m_baseTimer = lastRoundBeginTime + MinRoundTime;               
            }
            return m_baseTimer;
        }

        public float GetCurrent()
        {
            return m_baseTimer; ;
        }

        public float NextTimeStamp()
        {
            return m_baseTimer += MinimumTimeInterval;
        }
        public float NextTimeStampEffect()
        {
            return m_baseTimer += EffectTime;
        }
        public float NextTimeStampSkill()
        {
            return m_baseTimer += ActiveSkillTime;
        }
        //public float NextTimeStampAction()
        //{
        //    return m_baseTimer += EffectTime;
        //}

        public void SkipTo(float inputTimer)
        {
            if (inputTimer > m_baseTimer)
                m_baseTimer = inputTimer;
        }
    }

}

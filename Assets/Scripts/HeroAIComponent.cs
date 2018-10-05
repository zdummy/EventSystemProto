using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N2
{
    public class HeroAIComponent : MonoBehaviour
    {
        public class EventInfo
        {
            public string info;
            public float timer;

            public EventInfo(string _info, float _timer)
            {
                info = _info;
                timer = _timer;
            }
        }

        public int hp;
        public int position;
        public int m_playerID;
        public int m_ID;
        public bool m_dead;
        private EventManager eventManager;
        public UnityEngine.UI.Text outputText;
        
        private Queue<EventInfo> m_EventInfoQueue =  new Queue<EventInfo>();
        // Use this for initialization
        void Start()
        {
            eventManager = EventManager.Instance();


            eventManager.Effect_DamageCure += HurtOrCure;
            eventManager.Effect_BuffAuraDot += BuffAuraDot;
            eventManager.Effect_BuffAuraDotValue += BuffAuraDotValue;
            eventManager.Effect_State += StateChange;
            m_dead = false;
        }

        public string GetEventInfo()
        {
            return "";
        }
        // Update is called once per frame
        void Update()
        {
            //update info based on timer
            if (m_EventInfoQueue.Count > 0)
            {
                float curTime = Time.time;
                while (m_EventInfoQueue.Count > 0 && curTime - m_EventInfoQueue.Peek().timer > 30)
                {
                    m_EventInfoQueue.Dequeue();
                }
            }
            string result = GetMyString()+"\n";
            foreach (var eventInfo in m_EventInfoQueue)
            {
                result += eventInfo.info+"\n";
            }
            outputText.text = result;
            //Vector2 screenPos = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);//世界坐标(0,0,0)，一般可以用transform.position获取->屏幕坐标
           // outputText.re
        }

        private string GetMyString()
        {

            return string.Format("{1}-{0} {2}", m_ID, m_playerID, m_dead?"dead":"hp"+hp);
        }
        private void PassiveSkill(object sender, ActiveSkillEventArgs e)
        {
            if (e.PlayerID != m_playerID || e.Caster != m_ID) //not me
                return;
            string eventInfo = GetMyString() + " cast PassiveSkill " + e.SkillID + " to " + e.GetTargetsString();
            UtilLog.Log(eventInfo);
            m_EventInfoQueue.Enqueue(new EventInfo("PassiveSkill:"+e.SkillID, Time.time));
        }
        private void CommandSkill(object sender, ActiveSkillEventArgs e)
        {
            if (e.PlayerID != m_playerID || e.Caster != m_ID) //not me
                return;
            string eventInfo = GetMyString() + " cast CommandSkill " + e.SkillID + " to " + e.GetTargetsString();
            UtilLog.Log();
            m_EventInfoQueue.Enqueue(new EventInfo("CommandSkill:" + e.SkillID, Time.time));
        }

        private void ActiveSkill(object sender, ActiveSkillEventArgs e)
        {
            if (e.PlayerID != m_playerID || e.Caster != m_ID) //not me
                return;
            string eventInfo = GetMyString() + " cast ActiveSkill " + e.SkillID + " to " + e.GetTargetsString();
            UtilLog.Log(eventInfo);
            m_EventInfoQueue.Enqueue(new EventInfo("ActiveSkill:" + e.SkillID , Time.time));
        }

        private void NormalAttack(object sender, ActiveSkillEventArgs e)
        {
            if (e.PlayerID != m_playerID || e.Caster != m_ID) //not me
                return;
            string eventInfo = GetMyString() + " NormalAttack to " + e.GetTargetsString();
            UtilLog.Log(eventInfo);
            m_EventInfoQueue.Enqueue(new EventInfo("NormalAttack" + e.effName, Time.time));
        }

        private void HurtOrCure(object sender, Effect_DamageCureEventArgs e)
        {
            if (!e.Targets.Contains(m_ID))
                return;
            int index = -1;
            for (int i = 0; i < e.Targets.Count; i++)
            {
                if (e.Targets[i] == m_ID)
                {
                    if (e.TargetPlayerIDs[i] == m_playerID)
                        index = i;
                    else
                        return;
                }
            }
            hp -= e.HPChange[index];

            if (hp != e.LeftHP[index])
            {
                UtilLog.Log("HP mismatch with server, something wrong localHP:" + hp + " server:" + e.LeftHP[index]);
            }
            string eventInfo = string.Format("{0} got cure/hurt: {1}:{2} EffectID:{3} EffectTypeID:{4} Skill:{5} eff:{6}", GetMyString(), e.HPChange[index], e.LeftHP[index], e.EffectID, e.EffectTypeID, e.SkillID, e.effName);
            UtilLog.Log(eventInfo);
            m_EventInfoQueue.Enqueue(new EventInfo(e.effName, Time.time));
        }

        private void BuffAuraDot(object sender, Effect_BuffAuraDotEventArgs e)
        {
            if (!e.Targets.Contains(m_ID))
                return;
            int index = -1;
            for (int i = 0; i < e.Targets.Count; i++)
            {
                if (e.Targets[i] == m_ID)
                {
                    if (e.TargetPlayerIDs[i] == m_playerID)
                        index = i;
                    else
                        return;
                }
            }
            //string result = string.Format("{0} got {1}:{4} EffectID:{2} EffectTypeID:{3}", GetMyString(), e.eventType[index].ToString(), e.EffectID, e.EffectTypeID, e.BuffAuraDotID[index]);
            
            string eventInfo = (string.Format("{0} got {1}:{2} EffectID:{3} EffectTypeID:{4} Skill:{5} eff:{6}", GetMyString(), e.eventType[index].ToString(), e.BuffAuraDotID[index], e.EffectID, e.EffectTypeID, e.SkillID, e.effName));
            UtilLog.Log(eventInfo);
            m_EventInfoQueue.Enqueue(new EventInfo(e.effName+" "+ e.BuffAuraDotID[index], Time.time));
        }

        private void BuffAuraDotValue(object sender, Effect_BuffAuraDotValueEventArgs e)
        {
            if (!e.Targets.Contains(m_ID))
                return;
            int index = -1;
            for (int i = 0; i < e.Targets.Count; i++)
            {
                if (e.Targets[i] == m_ID)
                {
                    if (e.TargetPlayerIDs[i] == m_playerID)
                        index = i;
                    else
                        return;
                }
            }
            string eventInfo = string.Format("{0} got BuffAuraDotValue {1}({2}) EffectID:{3} EffectTypeID:{4} Skill:{5} eff:{6}", GetMyString(), e.ValueChange[index], e.ResultValue[index], e.EffectID, e.EffectTypeID, e.SkillID, e.effName);
            UtilLog.Log(eventInfo);
            m_EventInfoQueue.Enqueue(new EventInfo(e.effName+":" +e.ValueChange[index]+"("+e.ResultValue[index]+")", Time.time));
        }

        private void StateChange(object sender, Effect_STATEEventArgs e)
        {
            if (!e.Targets.Contains(m_ID))
                return;
            int index = -1;
            for (int i = 0; i < e.Targets.Count; i++)
            {
                if (e.Targets[i] == m_ID)
                {
                    if (e.TargetPlayerIDs[i] == m_playerID)
                        index = i;
                    else
                        return;
                }
            }
            string eventInfo = GetMyString() + " enter/leave state:" + e.StateID;
            if (e.StateID == 1)
                m_dead = true;
            UtilLog.Log(eventInfo);
            m_EventInfoQueue.Enqueue(new EventInfo("State:"+e.StateID, Time.time));
        }

        private void Print2DText()
        {

        }
    }
}

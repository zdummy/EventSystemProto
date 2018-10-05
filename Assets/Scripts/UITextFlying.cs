using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace N2
{
    public class UITextFlying : MonoBehaviour
    {
        public AnimationCurve FlyingTextSize;
        public AnimationCurve FlyingTextSpeed;
        public float MaxTime = 5;
        public float speedInPixel = 1000;
        private Vector2 anchPos;
        //RectTransform rect;
        float m_XAxis = 0f;
        float m_YAxis = 0.5f;
        //public Rect
        float m_timer = 0;
        float fontMaxSize;//= FlyingText.fontSize;
        private UnityEngine.UI.Text text;

        void Start()
        {
            m_timer = -10f;
            text = gameObject.GetComponent<UnityEngine.UI.Text>();
            text.text = "";
            fontMaxSize = text.fontSize;
            //anchPos = text.rectTransform.anchoredPosition;

            EventManager eventManager = EventManager.Instance();
            eventManager.CommandSkill += ActiveSkill;
            eventManager.PassiveSkill += ActiveSkill;
            eventManager.ActiveSkill += ActiveSkill;
        }

        private void OnDestroy()
        {
            EventManager eventManager = EventManager.Instance();
            eventManager.CommandSkill -= ActiveSkill;
            eventManager.PassiveSkill -= ActiveSkill;
            eventManager.ActiveSkill -= ActiveSkill;
        }

        void Update()
        {
            if (m_timer >=0)
            {
                float speedScale = FlyingTextSpeed.Evaluate(GetNormalizedTime());
                m_XAxis += speedInPixel / MaxTime * Time.deltaTime * speedScale;
                //text.seta
                m_timer += Time.deltaTime;
                text.rectTransform.anchoredPosition = new Vector2(m_XAxis, m_YAxis);//GetAnchorePos();
                text.fontSize = (int)(fontMaxSize * FlyingTextSize.Evaluate(GetNormalizedTime()));
            }

        }



        private void ActiveSkill(object sender, ActiveSkillEventArgs e)
        {

            UtilLog.Log("UI ActiveSkill:" + e.ToString());
            text.text = e.effName+":"+e.SkillID.ToString();
            StartFly();
            //textFlying.StartFly();
        }

        public void StartFly()
        {
            m_XAxis = -0.5f;
            m_YAxis = 0.5f;
            m_timer = 0;
        }
        public Vector2 GetAnchorePos()
        {
            return new Vector2(m_XAxis, m_YAxis);
        }
        public float GetNormalizedTime()
        {
            return m_timer / MaxTime;
        }
    }
}

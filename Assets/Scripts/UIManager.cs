using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

namespace N2
{


    public class UIManager : MonoBehaviour
    {
        public UnityEngine.UI.Text RoundInfo;
        public UnityEngine.UI.Text Timer;

        //flying text
        //private int fontMaxSize;
        //public UnityEngine.UI.Text FlyingText;
       
        //private UITextFlying textFlying = new UITextFlying();
        //public GradientColorKey FlyingTextColor;

        //public event EventHandler<ActiveSkillEventArgs> ActiveSkill;
        private EventManager eventManager;

        private float baseTimer;
        private int m_RoundCounter = 0;

        // Use this for initialization
        void Start()
        {
            RoundInfo.text = "Not started";
            Timer.text = "00:00:00";

            baseTimer = -10;
           

            eventManager = EventManager.Instance();
            eventManager.EnterRound += EnterRound;
            eventManager.GameOver += GameOver;
            eventManager.CommandSkill += ActiveSkill;
            eventManager.PassiveSkill += ActiveSkill;
            eventManager.ActiveSkill += ActiveSkill;
            //eventManager.NormalAttack += NormalAttack;
            //eventManager.Effect_DamageCure += HurtOrCure;
            //eventManager.Effect_BuffAuraDot += BuffAuraDot;
            //eventManager.Effect_BuffAuraDotValue += BuffAuraDotValue;
            //eventManager.Effect_State += StateChange;
        }

        // Update is called once per frame
        void Update()
        {
            if (baseTimer > 0)
                Timer.text = ReplayTimer.FormatTimer(Time.time - baseTimer);               
        }

        private void EnterRound(object sender, RoundEventArgs e)
        {
            if (baseTimer < 0)
            {
                baseTimer = Time.time;
            }
            UtilLog.Log("UI EnterRound:" + e.RoundInfo);

            if (e.RoundInfo.Equals("BattleRound"))
            {
                m_RoundCounter++;
                RoundInfo.text = e.RoundInfo + " " + m_RoundCounter;
            }
            else
            {
                RoundInfo.text = e.RoundInfo;
            }
        }

        private void ActiveSkill(object sender, ActiveSkillEventArgs e)
        {
            UtilLog.Log("UI ActiveSkill:" + e.ToString());
            //FlyingText.text = e.SkillID.ToString();
            
                    //textFlying.StartFly();


        }

        private void GameOver(object sender, RoundEventArgs e)
        {
            RoundInfo.text = e.RoundInfo;
            baseTimer = -10;
            Timer.text = "";
           
        }

        private void TextFlying()
        {

        }
    }

}

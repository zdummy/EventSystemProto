using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;


namespace N2
{
    public class ArenaWorld : MonoBehaviour
    {
        private static int m_unitCount = 6;

        public GameObject[]  m_HeroObjs = new GameObject[m_unitCount];
        //6 hero + scene + UI
        //private HeroAIComponent[] m_heroes = new HeroAIComponent[6];
        private SceneManager m_scene = new SceneManager();
        //private UIManager m_ui = new UIManager();
        private float timer;
        public float DisPatchIntervalMS = 30;

        private ArenaWorld()
        {
            //make sure you build the scene after event manage
            //for (int i = 0; i < 6; i++)
            //    m_heroes[i] = new HeroAIComponent();
        }

        private static ArenaWorld _instance;
        public static ArenaWorld Instance()
        {
            if (_instance == null)
                _instance = new ArenaWorld();
            return _instance;
        }

        void Start()
        {
            UtilLog.Log("ArenaWorld.Start");
            string battleReportPath = "";

#if UNITY_EDITOR
            //always got below excpetion, dont know why
            //FileNotFoundException: Could not find file "/work/EventSystemProto/Assets/Data/result.json".;
            battleReportPath = Application.streamingAssetsPath + "/../Data/result.json";
            //battleReportPath = "/work/EventSystemProto/Assets/Data/result.json";

           
            //battleReportPath = Application.streamingAssetsPath + "/Assets/Data/result.json";
#elif UNITY_ANDROID
            battleReportPath = Application.persistentDataPath + "/../Data/result.json";
            //判断路径内数据库是否存在  
            if(!File.Exists(appDBPath))=  
            {  
                //拷贝数据库  
                //StartCoroutine(CopyDataBase());  
                return "";
            }  
#elif UNITY_IPHONE
            battleReportPath = Application.persistentDataPath + "/../Data/result.json";
#endif
            N2.BattleReport br = JsonUtility.FromJson<N2.BattleReport>(File.ReadAllText(battleReportPath, Encoding.UTF8));
            UtilLog.Log("BattleReport Load successfully!");
            //br.Output();

            InitTeamData(br);
            EventManager eventManager = EventManager.Instance();
            eventManager.BuildEventQueue(br, timer);
            timer = Time.time;
            //eventManager.StartGame();
        }

        void Update()
        {
            if (Time.time > timer)
            {
                //UtilLog.Log("DispatchEvent");
                EventManager.Instance().DispatchEvent();
                timer = Time.time + DisPatchIntervalMS * 0.001f;
            }
            else
            {
                //UtilLog.Log("waiting");
            }
           
        }

        public void InitTeamData(N2.BattleReport br)
        {
            int index = 0;
            foreach (var member in br.TeamBlue )
            {
                HeroAIComponent hero =  m_HeroObjs[index].GetComponent<HeroAIComponent>();
                if (hero)
                {
                    hero.hp = member.HP;
                    hero.position = member.Position;
                    hero.m_playerID = int.Parse(member.GetPlayerID());
                    hero.m_ID = int.Parse(member.GetID());
                    index++;
                }              
            }
            foreach (var member in br.TeamRed)
            {
                HeroAIComponent hero = m_HeroObjs[index].GetComponent<HeroAIComponent>();
                if (hero)
                {
                    hero.hp = member.HP;
                    hero.position = member.Position;
                    hero.m_playerID = int.Parse(member.GetPlayerID());
                    hero.m_ID = int.Parse(member.GetID());
                    index++;
                }
            }
        }


    }
}

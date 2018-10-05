using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace N2
{   

    public class SceneManager
    {
        //public event EventHandler<ActiveSkillEventArgs> ActiveSkill;
        private EventManager eventManager;
        public SceneManager()
        {
            eventManager = EventManager.Instance();
            eventManager.EnterRound += EnterRound;
        }
        private void EnterRound(object sender, RoundEventArgs e)
        {
            UtilLog.Log("Scene EnterRound:"+e.RoundInfo);
        }
    }


}

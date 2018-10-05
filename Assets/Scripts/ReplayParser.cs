using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace N2 
{
    public class ReplayParser : MonoBehaviour
    {
        void Start()
        {
            string jsonTest = File.ReadAllText("E:/trunk/Unity/Assets/Data/result.json", Encoding.UTF8);
            BattleReport obj = JsonUtility.FromJson<BattleReport>(jsonTest);
            //Debug.Log(obj.ToString());
            obj.Output();
        }

        void Update()
        {

        }

    }

}

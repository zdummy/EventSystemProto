using System.Collections;
using System.Collections.Generic;
using System.Text;
//using replayEditor.Game;

namespace N2
{
  
    //save the data from excel table
    [System.Serializable]
    public class DataReportFormation
    {
        public static string FileName = @"report_format.xlsx";
        public static string JsonFileName = @"report_format.json";

        /*
         *  covert 
          ID    EffectType	Param1	Param2	Param3	Param4	Param5	Param6	Param7	Param8	Param9	Param10	Pattern
          1     MagicDamage	NodeID	EffectID	EffectTypeID	Caster	Targets	Value	ResultValue	CalcDamage			"Targets"损失"Damage"兵力("LeftHP")

            into

            EffectType :  MagicDamage
            NodeID :
            EffectID:
            EffectTypeID;
            Caster:
            Player:
            TargetS:
            Value:
            ResultValue:
            CalcDamage:
            Pattern :  "Targets"损失"Damage"兵力("LeftHP")
         * 
         * 
        */
        //public Dictionary<string, string> EffectStateItem;

        public List<Dictionary<string, string>> Data =  new List<Dictionary<string, string>>();

       
        public Dictionary<string, int> IndexDict = new Dictionary<string, int>();

        public HashSet<string> PatternKeySet = new HashSet<string>();

        public void BuildQueryIndexFromEffectOrStateName()
        {            
            int index = 0;
            foreach (var item in Data)
            {
                if (item.ContainsKey("EffectType"))  //it's effect
                    IndexDict.Add(item["EffectType"], index);
                else if (item.ContainsKey("HeroStateName"))   //it's state
                    IndexDict.Add(item["HeroStateName"], index);
                else   //it's scene
                    IndexDict.Add(item["Name"], index);
                index++;
            }
        }

        private Dictionary<string, string> GetEffectStateItem(string key)
        {
            if (IndexDict.ContainsKey(key))
            {
                if (Data.Count > IndexDict[key] && IndexDict[key] >= 0 )
                    return Data[IndexDict[key]];
            }
            return null;
        }

        public string GetEffectStatePattern(string key)
        {
            return GetEffectStateItem(key)["Pattern"];
        }
    }
}

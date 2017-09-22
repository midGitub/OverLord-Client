using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DruidSkill : ChampionSkill {
    void Awake()
    {
        if (champion == null)
        {
            champion = GetComponent<ChampionBehaviour>();
        }
    }
    public override void LaunchActiveSkill()
    {
        StartCoroutine(champion.ShowActiveSkillText("战斗咆哮"));
        Dictionary<int, ChampionBehaviour> self = new Dictionary<int, ChampionBehaviour>();
        self = this.champion.belongCity.GetSelfDic(this.champion);
        for (int i = 1; i <= self.Count; i++)
        {
            if (self[i] != null)
            {
                self[i].GetComponent<ChampionSkill>().AddBuff(Buff.Roar);
            }
        }
    }

   
}

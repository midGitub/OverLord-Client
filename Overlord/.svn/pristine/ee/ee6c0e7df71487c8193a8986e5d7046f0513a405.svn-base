using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PoetSkill : ChampionSkill {
    void Awake()
    {
        if (champion == null)
        {
            champion = GetComponent<ChampionBehaviour>();
        }
    }
    public override void LaunchActiveSkill()
    {
        StartCoroutine(champion.ShowActiveSkillText("镇魂曲"));
        Dictionary<int, ChampionBehaviour> self = new Dictionary<int, ChampionBehaviour>();
        self = this.champion.belongCity.GetSelfDic(this.champion);
        for (int i = 1; i <= self.Count; i++)
        {
            if (self[i] != null)
            {
                self[i].GetComponent<ChampionSkill>().AddBuff(Buff.Requiem);
            }
        }
    }

}

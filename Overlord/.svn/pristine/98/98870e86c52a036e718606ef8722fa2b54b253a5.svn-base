using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WarlockSkill : ChampionSkill {

    void Awake()
    {
        if (champion == null)
        {
            champion = GetComponent<ChampionBehaviour>();
        }
    }

    public override void LaunchActiveSkill()
    {
        StartCoroutine(champion.ShowActiveSkillText("恐惧"));
        Dictionary<int,ChampionBehaviour> enemys = new Dictionary<int,ChampionBehaviour>();
        enemys = this.champion.belongCity.GetEnemyDic(this.champion);
        for (int i = 1; i <= enemys.Count; i++)
        {
            if(enemys[i] != null)
            {
                int n = Random.Range(0, 10);
                if (n < 2)
                {
                    enemys[i].GetComponent<ChampionSkill>().AddBuff(Buff.Terrify);
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MageSkill : ChampionSkill {

    void Awake()
    {
        if (champion == null)
        {
            champion = GetComponent<ChampionBehaviour>();
        }
    }
    public override void LaunchActiveSkill()
    {
        StartCoroutine(champion.ShowActiveSkillText("暴风雪"));
        AddBuff(Buff.Blizard);
        Dictionary<int, ChampionBehaviour> enemys = new Dictionary<int, ChampionBehaviour>();
        enemys = this.champion.belongCity.GetEnemyDic(this.champion);

        for (int i = 1; i <= enemys.Count; i++)
        {
            if (enemys[i] != null)
            {
                this.champion.CreateBullet(enemys[i]);
            }
        }
    }

}

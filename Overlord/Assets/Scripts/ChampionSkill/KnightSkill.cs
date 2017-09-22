using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KnightSkill : ChampionSkill {

    void Awake()
    {
        if (champion == null)
        {
            champion = GetComponent<ChampionBehaviour>();
        }
    }
    public override void LaunchActiveSkill()
    {
        AddBuff(Buff.HolyShield);
        StartCoroutine(champion.ShowActiveSkillText("神圣护盾"));
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorSkill : ChampionSkill
{
    void Awake()
    {
        
        if (champion == null)
        {
            champion = GetComponent<ChampionBehaviour>();
        }
    }
    
    public override void LaunchActiveSkill()
    {
        StartCoroutine(champion.ShowActiveSkillText("盾墙"));
        AddBuff(Buff.ShieldWall);
    }

   
}

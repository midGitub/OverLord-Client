using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SeerSkill : ChampionSkill {
    void Awake()
    {
        if (champion == null)
        {
            champion = GetComponent<ChampionBehaviour>();
        }
    }
    public override void LaunchActiveSkill()
    {
        StartCoroutine(champion.ShowActiveSkillText("治疗术"));
        Dictionary<int, ChampionBehaviour> self = new Dictionary<int, ChampionBehaviour>();
        self = champion.belongCity.GetSelfDic(champion);
        
        List<ChampionBehaviour> champ = new List<ChampionBehaviour>(); 
        for (int i = 1; i <= self.Count; i++)
        {

            if (self[i] != null)
            {
                champ.Add(self[i]);
                
            }
        }
        
        int n = 0;
        for (int i = 0; i < champ.Count; i++)
        {
            if (champ[n].GetBlood() >= champ[i].GetBlood())
            {
                n = i;
            }
        }
        champ[n].AddBlood(champ[n].GetMaxBlood() * 0.5f);
    }

}

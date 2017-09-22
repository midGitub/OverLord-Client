using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HunterSkill : ChampionSkill {


    private BulletType bulletType = BulletType.none;
    void Awake()
    {
        if (champion == null)
        {
            champion = GetComponent<ChampionBehaviour>();
        }
    }
    public override void LaunchActiveSkill()
    {
        StartCoroutine(ShowActiveSkillText());
        ChampionBehaviour enemy = champion.belongCity.ChooseAim(champion);
        WindArrow();
        Invoke("WindArrow", 0.5f);
    }

    public override string GetBulletName()
    {
        if(bulletType == BulletType.StunBullet)
            return "stunbullet";
        return "bullet"; 
    }

    /// <summary>
    /// 疾风箭
    /// </summary>
    void WindArrow()
    {
        bulletType = BulletType.StunBullet;
        ChampionBehaviour enemy = champion.belongCity.ChooseAim(champion);
        champion.CreateBullet(enemy);
        bulletType = BulletType.none;
    }

    public IEnumerator ShowActiveSkillText()
    {
        
        champion.lifebarTrans.Find("Text").gameObject.SetActive(true);
        champion.lifebarTrans.Find("Text").GetComponent<Text>().text = "疾风箭";
           
        yield return new WaitForSeconds(2);
        champion.lifebarTrans.Find("Text").gameObject.SetActive(false);
    }
}

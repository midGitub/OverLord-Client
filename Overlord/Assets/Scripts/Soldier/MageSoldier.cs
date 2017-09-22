using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MageSoldier : Soldier
{
    public override bool IsActiveReady()
    {
        if (curActiveCD > 0)
        { curActiveCD = curActiveCD - Time.deltaTime; }


        if (curActiveCD <= 0 && curStatus == Status.InWar && tarEnemy != null)
        {
            { return true; }
        }
        return false;

    }

    public override void LaunchActiveSkill()
    {
        List<GameObject> result = belongCityManager.FindFriendsInRange(tarEnemy.GetComponent<Soldier>(), 5);
        foreach (GameObject enemy in result)
        {
            enemy.GetComponent<Soldier>().AddBuff(Buff.Blizard);
        }

        blizzard.SetActive(true);
        blizzard.transform.position = tarEnemy.transform.position;
        StartCoroutine(DelayHideBlizzard());

    }

    public IEnumerator DelayHideBlizzard()
    {
        yield return new WaitForSeconds(3);
        blizzard.SetActive(false);
    }

    public override bool IsPassiveReady()
    {
        if (curPassiveCD > 0)
        { curPassiveCD = curPassiveCD - Time.deltaTime; }



        if (curPassiveCD <= 0 && curStatus == Status.InWar)
        {
            if (blood < (float)maxBlood * 0.3)
            { return true; }
        }
        return false;


    }

    public override void LaunchPassiveSkill()
    {
        AddBuff(Buff.HolyShield);
    }
	
}

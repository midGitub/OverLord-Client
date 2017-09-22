using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WarlockSoldier : Soldier
{
    public override void OtherThing()
    {
        passiveSkillCD = 60;
    }

    public override void SkillUpdate()
    {
        if (mana <= 0)
        {
            lifebarTrans.Find("energy").gameObject.SetActive(false);
            return;
        }
        if (IsActiveReady() && energy >= maxEnergy && mana >= 10)
        {
            LaunchActiveSkill();
            curActiveCD = activeSkillCD;
            energy = 0;
            mana -= 10;
            StartCoroutine(ShowActiveSkillText());
        }

        if (curPassiveCD > 0)
        { curPassiveCD = curPassiveCD - Time.deltaTime; }


    }

    public override bool IsActiveReady()
    {
        if (curActiveCD > 0)
        { curActiveCD = curActiveCD - Time.deltaTime; }

        List<GameObject> result = belongCityManager.FindEnemiesInRange(this, 6);
        if (result.Count == 0) return false;


        if (curActiveCD <= 0 && curStatus == Status.InWar)
        {
            { return true; }
        }
        return false;

    }

    public override void LaunchActiveSkill()
    {
        List<GameObject> result = belongCityManager.FindEnemiesInRange(this, 6);
        int count = 0;
        foreach (GameObject tarfriend in result)
        {
            if (count >= 3) continue;
            tarfriend.GetComponent<Soldier>().AddBuff(Buff.Terrify);
            count++;
        }
    }

    public override void FriendDie(Soldier friend)
    {
        if (Vector3.Distance(friend.transform.position, transform.position) < 5)
        {
            if (curPassiveCD <= 0 && mana >= 5)
            {
                curPassiveCD = passiveSkillCD;
                StartCoroutine(ShowPassiveSkillText());

                List<GameObject> result = belongCityManager.FindEnemiesInRange(this, 5);
                if (result.Count == 0) return;
                foreach (GameObject enemy in result)
                {
                    enemy.GetComponent<Soldier>().AddBuff(Buff.RevengeCurse);
                }
                mana -= 5;
            }
        }
    }

}

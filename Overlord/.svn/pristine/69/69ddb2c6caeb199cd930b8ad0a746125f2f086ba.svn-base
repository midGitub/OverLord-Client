using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarriorrSoldier : Soldier
{
    public override void OtherThing()
    {
        bulletName = "vampirebullet";
        normalBullet = "vampirebullet";
        passiveSkillCD = 60;
    }

    public override bool IsActiveReady()
    {
        if (curActiveCD > 0)
        { curActiveCD = curActiveCD - Time.deltaTime; }



        if (curActiveCD <= 0 && curStatus == Status.InWar)
        {
            return true; 
        }
        return false;

    }

    public override void LaunchActiveSkill()
    {
        List<GameObject> result = belongCityManager.FindEnemiesInRange(this, 5);
        foreach (GameObject tarfriend in result)
        {
            tarfriend.GetComponent<Soldier>().AddBuff(Buff.Taunt);
        }
        AddBuff(Buff.TauntDefense);

    }

    public override bool IsPassiveReady()
    {
        if (curPassiveCD > 0)
        { curPassiveCD = curPassiveCD - Time.deltaTime; }

        if (curPassiveCD <= 0 && (blood <= (float)maxBlood * 0.3))
            return true;
        return false;

    }

    public override void LaunchPassiveSkill()
    {
        List<GameObject> result = belongCityManager.FindEnemiesInRange(this, 2);
        foreach (GameObject tarfriend in result)
        {
            if (tarfriend.GetComponent<Soldier>().curStatus == Status.InWar)
                tarfriend.GetComponent<Soldier>().WarriorShock(this);
        }
        AddBuff(Buff.ShieldWall);
    }

	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DruidSoldier : Soldier
{
    public override void OtherThing()
    {
        passiveSkillCD = 60;
    }

    public override bool IsActiveReady()
    {
        if (curActiveCD > 0)
        { curActiveCD = curActiveCD - Time.deltaTime; }

        List<GameObject> result = belongCityManager.FindFriendsInRange(this, 3);
        if (result.Count == 0) return false;


        if (curActiveCD <= 0 && curStatus == Status.InWar)
        {
            { return true; }
        }
        return false;

    }

    public override void LaunchActiveSkill()
    {
        List<GameObject> result = belongCityManager.FindFriendsInRange(this, 3);
        foreach (GameObject tarfriend in result)
        {
            tarfriend.GetComponent<Soldier>().AddBuff(Buff.Requiem);
        }
    }

    public override bool IsPassiveReady()
    {
        if (curPassiveCD > 0)
        { curPassiveCD = curPassiveCD - Time.deltaTime; }


        GameObject tarfriend = belongCityManager.FindLeastBloodFriend(this);
        if (tarfriend == null) return false;

        Soldier tarScript = tarfriend.GetComponent<Soldier>();

        if (curPassiveCD <= 0 && tarfriend != null && tarScript.curStatus == Status.InWar && curStatus == Status.InWar)
        {
            if (tarScript.blood < (float)tarScript.maxBlood * 0.3)
            { return true; }
        }
        return false;


    }

    public override void LaunchPassiveSkill()
    {
        GameObject tarfriend = belongCityManager.FindLeastBloodFriend(this);
        tarfriend.GetComponent<Soldier>().ReviveBoold((int)((float)tarfriend.GetComponent<Soldier>().maxBlood * 0.2));
        tarfriend.GetComponent<Soldier>().AddBuff(Buff.LifeBloom);
    }

	
}

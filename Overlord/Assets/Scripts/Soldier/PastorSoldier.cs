using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PastorSoldier : Soldier
{
    public override void OtherThing()
    {
        passiveSkillCD = 30;
    }

    public override bool IsActiveReady()
    {
        if (curActiveCD > 0)
        { curActiveCD = curActiveCD - Time.deltaTime; }

        GameObject tarfriend = belongCityManager.FindLeastBloodFriend(this);
        if (tarfriend == null) return false;

        Soldier tarScript = tarfriend.GetComponent<Soldier>();

        if (curActiveCD <= 0 && tarfriend != null && tarScript.curStatus == Status.InWar && curStatus == Status.InWar)
        {
            if (tarScript.blood != tarScript.maxBlood)
            { return true; }
        }
        return false;

    }

    public override void LaunchActiveSkill()
    {
        GameObject tarfriend = belongCityManager.FindLeastBloodFriend(this);
        tarfriend.GetComponent<Soldier>().ReviveBoold((int)((float)tarfriend.GetComponent<Soldier>().maxBlood * 0.3));
        foreach (GameObject friend in belongCityManager.FindFriendsInRange(tarfriend.GetComponent<Soldier>(), 3))
        {
            friend.GetComponent<Soldier>().AddBuff(Buff.LifeBloom);
        }

    }

    public override bool IsPassiveReady()
    {
        if (curPassiveCD > 0)
        { curPassiveCD = curPassiveCD - Time.deltaTime; }

        GameObject tarfriend = belongCityManager.FindLeastBloodFriend(this);
        if (tarfriend == null) return false;

        Soldier tarScript = tarfriend.GetComponent<Soldier>();
        if (curPassiveCD <= 0 && (tarScript.blood <= (float)tarScript.maxBlood * 0.3))
            return true;
        return false;

    }

    public override void LaunchPassiveSkill()
    {
        GameObject tarfriend = belongCityManager.FindLeastBloodFriend(this);
        
        if (tarfriend == null) return;

        Soldier tarScript = tarfriend.GetComponent<Soldier>();
        tarScript.AddBuff(Buff.HolyShield);
        tarScript.AddBuff(Buff.LifeBloom);
    }

	
}

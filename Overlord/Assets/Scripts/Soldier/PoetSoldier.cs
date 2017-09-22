using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoetSoldier : Soldier
{
    public override bool IsActiveReady()
    {
        if (curActiveCD > 0)
        { curActiveCD = curActiveCD - Time.deltaTime; }

        List<GameObject> result = belongCityManager.FindFriendsInRange(this, 3);
        if (result.Count == 0) return false;


        if (curActiveCD <= 0 &&  curStatus == Status.InWar)
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
        return false;

    }
	
}

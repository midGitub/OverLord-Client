using UnityEngine;
using System.Collections;

public class KnightSoldier : Soldier
{
    public override void OtherThing()
    {
        passiveSkillCD = 60;
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
        tarfriend.GetComponent<Soldier>().AddBuff(Buff.HolyLight);
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
        AddBuff(Buff.HolyShield);
    }

}

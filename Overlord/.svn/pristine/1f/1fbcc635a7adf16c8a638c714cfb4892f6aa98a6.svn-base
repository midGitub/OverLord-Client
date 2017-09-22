using UnityEngine;
using System.Collections;

public class HunterSoldier : Soldier
{
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
        { 
            curPassiveCD = curPassiveCD - Time.deltaTime;
        }


    }

    public override void OtherThing()
    {
        passiveSkillCD = 20;
    }

    public override bool IsActiveReady()
    {
        if (curActiveCD > 0)
        { curActiveCD = curActiveCD - Time.deltaTime; }

        if (curActiveCD <= 0 && curStatus == Status.InWar)
        {
            { return true; }
        }
        return false;

    }

    public override void LaunchActiveSkill()
    {
        AddBuff(Buff.WindArrow);
    }

    public override void ReceiveDamage(int damage)
    {
        if (isDead) return;

        if (HasBuff(Buff.HolyLight))
            damage = (int)((float)damage * 0.7f);
        if (HasBuff(Buff.HolyShield))
            damage = 0;
        if (HasBuff(Buff.Requiem))
            damage = (int)((float)damage * 0.8f);
        if (HasBuff(Buff.Shadow) && Random.value < 0.3)
            damage = 0;

        blood = blood - damage;
        energy = energy + 10;
        onceDamage = damage;
        StartCoroutine(ShowBooldText());

        if (curPassiveCD <= 0 && mana >= 5)
        {
            curPassiveCD = passiveSkillCD;
            StartCoroutine(ShowPassiveSkillText());
            AddBuff(Buff.Shadow);
            mana -= 5;
        }

        if (blood <= 0)
        {
            Die();
        }
    }

   
	
}

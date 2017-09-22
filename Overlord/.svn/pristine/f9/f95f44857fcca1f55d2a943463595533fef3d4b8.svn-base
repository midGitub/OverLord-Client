using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeerSoldier : Soldier
{

    public override void OtherThing()
    {
        passiveSkillCD = 30;
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
        }

        if (curPassiveCD > 0)
        { curPassiveCD = curPassiveCD - Time.deltaTime; }
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
        tarfriend.GetComponent<Soldier>().ReviveBoold((int)((float)tarfriend.GetComponent<Soldier>().maxBlood * 0.4));
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
        

        if (blood <= 0)
        {
            Die();
        }

        if (curPassiveCD <= 0 && mana >= 5)
        {
            curPassiveCD = passiveSkillCD;
            StartCoroutine(ShowPassiveSkillText());

            if (tarEnemy == null) return;
            List<GameObject> result = belongCityManager.FindFriendsInRange(tarEnemy.GetComponent<Soldier>(), 3);
            foreach (GameObject enemy in result)
            {
                enemy.GetComponent<Soldier>().AddBuff(Buff.Exile);
            }
            tarEnemy.GetComponent<Soldier>().AddBuff(Buff.Exile);
            mana -= 5;
        }
    }


}

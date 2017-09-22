using UnityEngine;
using System.Collections;

public class StunBullet : Bullet {

    public override void ReachedVit()
    {
        //if (Random.value < 0.3)
        //{vitSoldier.GetComponent<Soldier>().AddBuff(Soldier.Buff.Stun);}
        //vitSoldier.GetComponent<Soldier>().ReceiveDamage((int)((float)damage * damageScale));
        //Destroy(gameObject);

        int n = Random.Range(0, 10);
        if (n < 3)
        {
            vitSoldier.GetComponent<ChampionSkill>().AddBuff(Buff.WindArrow);
        }
        vitSoldier.GetComponent<ChampionBehaviour>().ReceiveDamage(damage, attSoldier.GetComponent<ChampionBehaviour>());
        Destroy(gameObject);
    }

    public void Update()
    {
        if (vitSoldier == null) return;
        if (vitSoldier.GetComponent<ChampionProp>().isDead)
            Destroy(gameObject);
        if (attSoldier.GetComponent<ChampionProp>().isDead)
            Destroy(gameObject);
        transform.LookAt(vitSoldier.transform);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (Vector3.Distance(transform.position, vitSoldier.transform.position) < 0.7)
        {
            ReachedVit();
        }

    }
}

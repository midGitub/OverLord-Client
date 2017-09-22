using UnityEngine;
using System.Collections;

public class PetSoldier : Soldier
{
    //public override void OtherThing()
    //{
    //    master = transform.parent.GetComponent<Soldier>();
    //}



    void Update()
    {
        if (master.isDead) Die();

        switch (master.curStatus)
        {
            case Status.None:
                break;
            case Status.InWar:
                AttackMasterTarget();
                break;
            case Status.OnWay:
                GoToMaster();
                break;
            case Status.Peace:
                GoToMaster();
                break;
        }

        UpdateLifebar();
    }

    void AttackMasterTarget()
    {
        tarEnemy = master.tarEnemy;
        if (tarEnemy == null) return;

        if (Vector3.Distance(transform.position, tarEnemy.transform.position) > attackRange)
        {
            transform.LookAt(tarEnemy.transform);
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        }
        else
        {
            if (CanAttack())
            {
                isAttacking = true;
                StartCoroutine(Attack());
            }
        }
    }

    void GoToMaster()
    {
        Transform tarTans = master.transform;
        transform.LookAt(tarTans);
        if (Vector3.Distance(tarTans.position, transform.position) >2)
        { transform.Translate(Vector3.forward * Time.deltaTime * Speed);}
    }

    public override void Die()
    {
        isDead = true;
        lifebar.SetActive(false);
        gameObject.SetActive(false);
    }
}

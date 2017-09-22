using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    /// <summary>
    /// 攻击者
    /// </summary>
    public GameObject attSoldier = null;
    /// <summary>
    /// 被击者
    /// </summary>
    public GameObject vitSoldier = null;
    public float speed = 20;
    public float damage = 20f;
    //public float damageScale = 1.0f;


	// Use this for initialization
	void Start () {
	
	}

    public void SetInfo(GameObject att, GameObject vit)
    {
        attSoldier = att;
        vitSoldier = vit;
        transform.position = attSoldier.transform.position;
    }
	
	// Update is called once per frame
	public void Update () 
    {
        if (vitSoldier == null) return;
		if (vitSoldier.GetComponent<ChampionProp> ().isDead)
			Destroy (gameObject);
        if (attSoldier.GetComponent<ChampionProp>().isDead)
            Destroy(gameObject);
        transform.LookAt(vitSoldier.transform);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (Vector3.Distance(transform.position, vitSoldier.transform.position) < 0.7)
        {
            ReachedVit();
        }

	}

    public void SetDamage(float customDamage)
    {
        damage = customDamage;
    }

    public virtual void ReachedVit()
    {
      //  vitSoldier.GetComponent<Soldier>().ReceiveDamage((int)((float)damage * damageScale));
        vitSoldier.GetComponent<ChampionBehaviour>().ReceiveDamage(damage, attSoldier.GetComponent<ChampionBehaviour>());
        Destroy(gameObject);
    }
}

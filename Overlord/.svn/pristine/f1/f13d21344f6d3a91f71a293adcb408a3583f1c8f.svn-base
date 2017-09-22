using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeutralAI : MonoBehaviour {

    Dictionary<Side, float> side2DamageCount = new Dictionary<Side, float>();

	// Use this for initialization
	void Start () {
		side2DamageCount.Add(Side.Blue, 0);
        side2DamageCount.Add(Side.Red, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DamageCount(float damage, ChampionBehaviour champion)
    {
        side2DamageCount[champion.GetSide()] += damage;
    }

    public void ClearDamageCount()
    {
        side2DamageCount[Side.Blue] = 0;
        side2DamageCount[Side.Red] = 0;
    }

    public Side ChooseAttackSide()
    {
        if (side2DamageCount[Side.Blue] > side2DamageCount[Side.Red])
        {
            return Side.Blue;
        }
        else
        {
            return Side.Red;
        }
    }
}

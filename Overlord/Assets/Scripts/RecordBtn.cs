using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordBtn : MonoBehaviour {
    public Soldier linkSoldier = null;

	// Use this for initialization
	void Start () {
        string parentname = transform.parent.gameObject.name;
        parentname = parentname.Substring(0, parentname.Length - 6);
        string strooptag = parentname + "troop";
        string soldiername = gameObject.name;
        soldiername = soldiername.Substring(0, soldiername.Length - 3);

        foreach (GameObject soldier in GameObject.FindGameObjectsWithTag(strooptag))
        {
            if (soldiername == soldier.name)
            { linkSoldier = soldier.GetComponent<Soldier>(); }
        }
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!linkSoldier)
        {
            gameObject.SetActive(false);
            return;
        }
        if (linkSoldier.isDead) gameObject.SetActive(false); 
	}
}

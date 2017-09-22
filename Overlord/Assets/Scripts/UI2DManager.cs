using UnityEngine;
using System.Collections;

public class UI2DManager : MonoBehaviour {
    public GameObject redpanel = null;
    public GameObject bluepanel = null;
    public bool isRedBtnShow = true;

	// Use this for initialization
	void Start () {
	
	}
	

	// Update is called once per frame
	void Update () {
	
	}

    public void OnSwitchClick()
    {
        
        isRedBtnShow = !isRedBtnShow;
        if (isRedBtnShow)
        {
            redpanel.SetActive(true);
            bluepanel.SetActive(false);
        }
        else 
        {
            redpanel.SetActive(false);
            bluepanel.SetActive(true);
        }
    }
}

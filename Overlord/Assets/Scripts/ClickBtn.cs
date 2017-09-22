using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ClickBtn : MonoBehaviour ,IPointerDownHandler{
    public GameObject cityPanel = null;
    public ChampionBehaviour linkChampion = null;
    public GameObject btnPanel = null;
    public FightManager gameManager = null;


	// Use this for initialization
	void Start () {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ClickMe);
	}

    void Update()
    { 
        string parentname = transform.parent.gameObject.name;
        parentname = parentname.Substring(0, parentname.Length - 3);
        string strooptag = parentname + "troop";
        string soldiername = gameObject.name;
        soldiername = soldiername.Substring(0, soldiername.Length - 3);

        linkChampion = null;
        foreach (GameObject soldier in GameObject.FindGameObjectsWithTag(strooptag))
        {
            if (soldiername == soldier.name)
            { linkChampion = soldier.GetComponent<ChampionBehaviour>(); }
        }

        if (linkChampion == null || linkChampion.curStatus != Status.Peace)
        {
            gameObject.SetActive(false);
        }

    }

    void ClickMe()
    {
        cityPanel.SetActive(true);
        //btnPanel.SetActive(false);
        cityPanel.GetComponent<CityPanelManager>().curChampion = linkChampion;
        
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.SetClickBtnLinkSoldier(linkChampion);
    }

}

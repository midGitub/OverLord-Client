using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CityPanelManager : MonoBehaviour {
    public ChampionBehaviour curChampion = null;
    public GameObject btnPanel = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (curChampion == null)
            return;

        foreach (Transform childtrans in transform)
        {
            if (childtrans.gameObject.name == "cancel")
                continue;

            string childname = childtrans.gameObject.name;
            childname = childname.Substring(0, childname.Length - 3);
            if (IsLinkCity(childname))
                childtrans.GetComponent<Button>().interactable = true;
            else
                childtrans.GetComponent<Button>().interactable = false;
        }

	}

    bool IsLinkCity(string checkname)
    {
        RoundManager belongCity = curChampion.belongCity;
        string belongCityName = belongCity.gameObject.name;
        string[] cityList = FightManager.GetLinkCity(belongCityName);
        foreach (string cityname in cityList)
        {
            if (checkname == cityname)
                return true;
        }
        return false;
    }

    public void CityBtnClick(string cityname)
    {
        if ("cancel" == cityname)
        {
            gameObject.SetActive(false);
            btnPanel.SetActive(true);
            return;
        }

        GameObject city = GameObject.Find("/Envi/City/" + cityname);
        if (city == null) return;

        curChampion.belongCity.ChampionLeaveCity(curChampion);
        curChampion.SetStatus(Status.OnWay);
        curChampion.SetTarCity(city.transform);
        curChampion.belongCity.fightManager.AddOnWayChampionList(curChampion);
        gameObject.SetActive(false);
        btnPanel.SetActive(true);
        
    }
}

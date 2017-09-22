using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public int cameraSpeed = 1;
    public GameObject redTroop = null;
    public GameObject blueTroop = null;
    public GameObject winWord = null;
    public GameObject winBtn = null;
    public GameObject citys = null;


    private Vector2 startPos = new Vector2();
    private Vector2 MovedPos = new Vector2();
    private Vector2 direction = new Vector2();
    private Vector2 camDir = new Vector2();
    private Soldier linkSoldier = null;

    static Dictionary<string, string[]> city2Citys = new Dictionary<string, string[]>();

    private Dictionary<int, Soldier> fingerID2Soldier = new Dictionary<int, Soldier>();
    private Dictionary<int, Vector2> fingerID2StartPos = new Dictionary<int, Vector2>();

    private static int count = 0;

    //string showtext1 = "111";
    //string showtext2 = "222";

	// Use this for initialization
	void Start () {
        city2Citys.Clear();
        city2Citys.Add("redcity", "topcity,midcity,botcity".Split(','));
        city2Citys.Add("bluecity", "topcity,midcity,botcity".Split(','));
        city2Citys.Add("topcity", "redcity,midcity,bluecity".Split(','));
        city2Citys.Add("midcity", "topcity,botcity,redcity,bluecity".Split(','));
        city2Citys.Add("botcity", "midcity,redcity,bluecity".Split(','));



	}
	
	// Update is called once per frame
	void Update () {
        //WinGameCheck();
        //HandleTouchEvent();
	
	}

    void WinGameCheck()
    {
        if (!redTroop) return;
        bool isRedTroopAllDead = true;
        foreach (Transform soldiertrans in redTroop.transform)
        {
            if (!soldiertrans.GetComponent<Soldier>().isDead && !soldiertrans.GetComponent<Soldier>().IsDefender()) isRedTroopAllDead = false;  
        }

        if (!blueTroop) return;
        bool isBlueTroopAllDead = true;
        foreach (Transform soldiertrans in blueTroop.transform)
        {
            if (!soldiertrans.GetComponent<Soldier>().isDead && !soldiertrans.GetComponent<Soldier>().IsDefender()) isBlueTroopAllDead = false;
        }

        if (isRedTroopAllDead)
        {
            BlueWin();
        }
        else if (isBlueTroopAllDead)
        {
            RedWin();
        }
    }

    public void BlueWin()
    {
        winWord.SetActive(true);
        winBtn.SetActive(true);
        winWord.GetComponent<Text>().text = "蓝方胜利";
    }
    public void RedWin()
    {
        winWord.SetActive(true);
        winBtn.SetActive(true);
        winWord.GetComponent<Text>().text = "红方胜利";
    }

    public void Restart()
    {
        Application.LoadLevel("fight");
    }

    void OnLevelWasLoaded(int level)
    {
        //LoadLevel之后OnLevelWasLoaded会执行两次，暂时用count屏蔽第二次
        count++;
        if (count % 2 == 1) return;

        string[] soldierTags = new string[] { "neutraltroop", "redtroop", "bluetroop" };
        foreach (string tagname in soldierTags)
        {
            foreach (GameObject soldier in GameObject.FindGameObjectsWithTag(tagname))
            {
                soldier.GetComponent<ChampionBehaviour>().InitSlider();
            }
        }
    }


    void HandleTouchEvent()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    //showtext1 = "TouchPhase.Began";
                    if (linkSoldier)
                    {
                        if (fingerID2Soldier.ContainsKey(touch.fingerId))
                        {
                            fingerID2Soldier.Remove(touch.fingerId);
                        }
                        fingerID2Soldier.Add(touch.fingerId, linkSoldier);
                        linkSoldier = null;
                    }

                    if (fingerID2StartPos.ContainsKey(touch.fingerId))
                    {
                        fingerID2StartPos.Remove(touch.fingerId);
                    }
                    fingerID2StartPos.Add(touch.fingerId, touch.position);
                    //if (Physics.Raycast(ray, out hit))
                    //{
                    //    if ((hit.transform.gameObject.tag.IndexOf("red") != -1)||(hit.transform.gameObject.tag.IndexOf("blue") != -1))
                    //    {
                    //        soldierTrans = hit.transform;
                           

                    //    }
                    //    else
                    //    {
                    //        soldierTrans = null;
                    //    }
                    //}
                    //else
                    //{
                    //    soldierTrans = null;
                    //}

                    break;

                case TouchPhase.Moved:
                    break;

                case TouchPhase.Ended:
                    //showtext1 = "TouchPhase.Ended";
                    Soldier fingerSold = null;
                    Vector2 fingerPos = Vector2.zero;
                    if (fingerID2Soldier.ContainsKey(touch.fingerId))
                    {
                        fingerSold = fingerID2Soldier[touch.fingerId];
                        fingerID2Soldier.Remove(touch.fingerId);
                    }

                    if (fingerID2StartPos.ContainsKey(touch.fingerId))
                    {
                        fingerPos = fingerID2StartPos[touch.fingerId];
                        fingerID2StartPos.Remove(touch.fingerId);
                    }

                    if (fingerSold != null && fingerPos != Vector2.zero)
                    {
                        direction = touch.position - fingerPos;
                        GameObject city = FindTouchCity(fingerSold, touch.position);
                        if (city != null)
                        {
                            fingerSold.SetStatus(Soldier.Status.OnWay);
                            fingerSold.GetComponent<Soldier>().SetTarCity(city);
                            
                        }
                    }
                    break;
            }


        }
    }

    GameObject FindTarCity(Soldier soldscript, Vector2 direction)
    {
        if (soldscript.belongCityManager == null)
            return null;
        string belongName = soldscript.belongCityManager.gameObject.name;
        string[] cityList = city2Citys[belongName];
        GameObject tarCity = null;
        float mixAngle = 90;
        foreach (string onecityname in cityList)
        {
            Vector3 soldierPos = soldscript.transform.localPosition;
            GameObject onecity = GameObject.Find("/Envi/City/" + onecityname);
            Vector3 citydir3 = onecity.transform.localPosition - soldierPos;
            Vector2 citydir2 = new Vector2(citydir3[0], citydir3[2]);
            float angle = Vector2.Angle(citydir2, direction);
            if (angle < mixAngle)
            {
                mixAngle = angle;
                tarCity = onecity;
            }
        }

        return tarCity;
    }

    GameObject FindTouchCity(Soldier soldscript, Vector2 touch)
    {
        if (soldscript.belongCityManager == null)
            return null;
        GameObject tarCity = null; 

        foreach (Transform oneCityTrans in citys.transform)
        {
            if (oneCityTrans.GetComponent<CityManager>().IsTouchPointInCity(touch))
                tarCity = oneCityTrans.gameObject;
        }

        string belongName = soldscript.belongCityManager.gameObject.name;
        string[] cityList = city2Citys[belongName];

        if (tarCity != null && string.Join("", cityList).Contains(tarCity.name))
        {
            return tarCity;
        }
        else
        {return null;}

    }


    void CameraMove()
    {
        Vector3 add = new Vector3(camDir[0], 0, camDir[1]);
        add = add.normalized * cameraSpeed * Time.deltaTime;
        //print(add);
        Camera.main.transform.localPosition = Camera.main.transform.localPosition + add;
    }


    void HandleMouseEvent()
    {
        RaycastHit hit = new RaycastHit();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 off = new Vector3(10, 0, 0);
                if (hit.transform.gameObject.tag.IndexOf("troop") != -1)
                {
                    hit.transform.position = hit.transform.position + off;
                    startPos = Input.mousePosition;

                }
            }
        }
    }

    public static string[] GetLinkCity(string checkcity)
    {
        string[] cityList = city2Citys[checkcity];
        return cityList;
    }

    public void SetClickBtnLinkSoldier(Soldier soldier)
    {
        linkSoldier = soldier;
        //showtext2 = linkSoldier.ToString();
    }
}

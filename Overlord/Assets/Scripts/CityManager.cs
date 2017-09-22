using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CityManager : MonoBehaviour {
    public GameManager gameManager;

    private Material redMaterial;
    private Material greenMaterial;
    private Vector3 cityPos = new Vector3();
    private Vector2 citySize = new Vector2();
    private bool isInWar = false;
    public  int troopsNum = 0;

    private Dictionary<Soldier.Side, ArrayList> side2Troop = new Dictionary<Soldier.Side, ArrayList>();
    private Dictionary<Soldier.Side, string> side2Tag = new Dictionary<Soldier.Side, string>();


    public List<Vector3> redTroopBtnPoints = new List<Vector3>();
    public List<Vector3> blueTroopBtnPoints = new List<Vector3>();

    public Text warTimeText = null;
    private float warBeginTime = 0;
	// Use this for initialization
	void Start () {
        redMaterial = Resources.Load("Materials/red") as Material;
        greenMaterial = Resources.Load("Materials/green") as Material;
        cityPos = transform.localPosition;
        citySize = new Vector2(GetComponent<Collider>().bounds.size.x, GetComponent<Collider>().bounds.size.z);

        side2Troop.Clear();
        side2Troop.Add(Soldier.Side.Neutral, new ArrayList());
        side2Troop.Add(Soldier.Side.Red, new ArrayList());
        side2Troop.Add(Soldier.Side.Blue, new ArrayList());

        side2Tag.Clear();
        side2Tag.Add(Soldier.Side.Neutral, "neutraltroop");
        side2Tag.Add(Soldier.Side.Red, "redtroop");
        side2Tag.Add(Soldier.Side.Blue, "bluetroop");
        CalTroopBtnPoints();

        warTimeText.transform.position = Camera.main.WorldToScreenPoint(new Vector3(cityPos[0] + citySize[0] / 2, cityPos[1], cityPos[2] + citySize[1] / 2));

	
	}

    void CalTroopBtnPoints()
    {
        float x, y, z, w, h;
        x = cityPos[0];
        y = cityPos[1];
        z = cityPos[2];
        w = citySize[0] + 7;
        h = citySize[1] + 7;
        float intervalx = w / 2f * (3f / 5f);
        float intervalz = h / 2f * (3f / 5f);

        redTroopBtnPoints.Add(new Vector3(x + w/2, y, z));
        redTroopBtnPoints.Add(new Vector3(x + w / 2, y, z + intervalz));
        redTroopBtnPoints.Add(new Vector3(x + w / 2, y, z - intervalz));
        redTroopBtnPoints.Add(new Vector3(x + intervalx, y, z + h / 2));
        redTroopBtnPoints.Add(new Vector3(x + intervalx, y, z - h / 2));
        redTroopBtnPoints.Add(new Vector3(x, y, z + h / 2));
        redTroopBtnPoints.Add(new Vector3(x, y, z - h / 2));
        redTroopBtnPoints.Add(new Vector3(x - intervalx, y, z + h / 2));
        redTroopBtnPoints.Add(new Vector3(x - intervalx, y, z - h / 2));

        blueTroopBtnPoints.Add(new Vector3(x - w / 2, y, z));
        blueTroopBtnPoints.Add(new Vector3(x - w / 2, y, z + intervalz));
        blueTroopBtnPoints.Add(new Vector3(x - w / 2, y, z - intervalz));
        blueTroopBtnPoints.Add(new Vector3(x - intervalx, y, z + h / 2));
        blueTroopBtnPoints.Add(new Vector3(x - intervalx, y, z - h / 2));
        blueTroopBtnPoints.Add(new Vector3(x, y, z + h / 2));
        blueTroopBtnPoints.Add(new Vector3(x, y, z - h / 2));
        blueTroopBtnPoints.Add(new Vector3(x + intervalx, y, z + h / 2));
        blueTroopBtnPoints.Add(new Vector3(x + intervalx, y, z - h / 2));
    }

    public bool IsTouchPointInCity(Vector2 touchPos)
    {
        float x, y, z, w, h;
        x = cityPos[0];
        y = cityPos[1];
        z = cityPos[2];
        w = citySize[0];
        h = citySize[1];
        Vector3 lefttop = Camera.main.WorldToScreenPoint(new Vector3(x - w / 2, y, z - h / 2));
        Vector3 rightbtm = Camera.main.WorldToScreenPoint(new Vector3(x + w / 2, y, z + h / 2));
        if ((lefttop[0] < touchPos[0] && touchPos[0] < rightbtm[0]) && (lefttop[1] < touchPos[1] && touchPos[1] < rightbtm[1]))
            return true;
        return false;
    }

	// Update is called once per frame
	void Update () {
        GetTroopInCity();
        ReceiveOnWaySoldier();
        WinGameCheck();
        ShowWarDuration();

        troopsNum = 0;
        foreach (KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            if (item.Value.Count > 0) troopsNum++;
        }

        

        if (troopsNum >= 2)
        {
            if (!isInWar)
            {
                isInWar = true;
                warBeginTime = Time.time;
                AllSoldierEnterWar();
                GetComponent<Renderer>().material = redMaterial;
            }
        }
        else
        {
            if (isInWar)
            {
                isInWar = false;
                AllSoldierLeaveWar();
                
                GetComponent<Renderer>().material = greenMaterial;
            }
            SquareSoldier();
            SquareSoldierBtn();
        }
	
	}

    void ReceiveOnWaySoldier()
    {
         foreach(KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            foreach (GameObject soldier in item.Value)
            {
                if (soldier.GetComponent<Soldier>().curStatus == Soldier.Status.OnWay)
                {
                    if (soldier.GetComponent<Soldier>().tarCity.name == gameObject.name)
                    {
                        if (isInWar)
                            soldier.GetComponent<Soldier>().SetStatus(Soldier.Status.InWar);
                        else
                            soldier.GetComponent<Soldier>().SetStatus(Soldier.Status.Peace);
                        soldier.GetComponent<Soldier>().SetBelongCity(this);
                        soldier.GetComponent<Soldier>().SetTarCity(null);
                    }
                }
            }
            
        }

    }

    void GetTroopInCity() {
        foreach (KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            string tagname = side2Tag[item.Key];
            item.Value.Clear();
            foreach (GameObject soldier in GameObject.FindGameObjectsWithTag(tagname))
            {
                if (soldier.GetComponent<Soldier>().isDead) continue;
                if (IsInCity(soldier.transform.localPosition))
                {
                    item.Value.Add(soldier);

                }
            }

        }

    
    }

    public bool IsInCity(Vector3 pos) {
        float x,z,w,h;
        x = cityPos[0];
        z = cityPos[2];
        w = citySize[0];
        h = citySize[1];
        if (((x - w / 2) < pos[0] && pos[0] < (x + w / 2)) && ((z - h / 2) < pos[2] && pos[2] < (z + h / 2)))
            return true;
        return false;
        
    }

    void AllSoldierEnterWar()
    {
        foreach(KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            foreach (GameObject soldier in item.Value)
            {
                soldier.GetComponent<Soldier>().SetStatus(Soldier.Status.InWar);
            }
        }

    }

    void AllSoldierLeaveWar()
    {
        foreach (KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            foreach (GameObject soldier in item.Value)
            {
                if (soldier.GetComponent<Soldier>().curStatus == Soldier.Status.InWar)
                    soldier.GetComponent<Soldier>().SetStatus(Soldier.Status.Peace);
            }
        }

    }

    void SquareSoldier() {
        int index = 0;
        float x, z, w, h;
        x = cityPos[0];
        z = cityPos[2];
        w = citySize[0] * 3 / 4;
        h = citySize[1] * 3 / 4;
        float intervalx = w / 2;
        float intervalz = h / 2;
        ArrayList checkTroop = null;
        foreach (KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            if (item.Value.Count > 0)
            {
                checkTroop = item.Value;
            }
        }

        if (checkTroop == null) return;

        foreach (GameObject soldier in checkTroop)
        {
            if (soldier.GetComponent<Soldier>().IsOnWay())
                continue;
            if (soldier.name.Contains("defender"))
                continue;
            int col = index % 3;
            int row = index / 3;
            //bluesoldier.transform.localPosition = new Vector3(x - w / 2 + intervalx * col, 1, z - h / 2 + intervalz * row);
            soldier.GetComponent<Soldier>().squarePos = new Vector3(x - w / 2 + intervalx * col, 1, z - h / 2 + intervalz * row);
            index++;
        }
    }

    void SquareSoldierBtn()
    {
        ArrayList checkTroop = null;
        List<Vector3> squarePoints = null;
        foreach (KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            if (item.Value.Count > 0)
            {
                checkTroop = item.Value;
                if (item.Key == Soldier.Side.Red)
                {
                    squarePoints = redTroopBtnPoints;
                }
                else if (item.Key == Soldier.Side.Blue)
                {
                    squarePoints = blueTroopBtnPoints;
                }
            }
        }
        if (checkTroop == null) return;
        if (squarePoints == null) return;

        int index = 0;
        foreach (GameObject soldier in checkTroop)
        {
            if (soldier.name.Contains("defender")) continue;
            if (soldier.GetComponent<Soldier>().curStatus == Soldier.Status.OnWay) continue;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(squarePoints[index]);

            //if(screenPos)
            //Screen.width

            SetBtnWith3DPos(soldier, screenPos);

            index++;
        }
        
    }

    void SetBtnWith3DPos(GameObject soldier, Vector3 screenPos)
    {
        ClickBtn btnScript = soldier.GetComponent<Soldier>().GetControlBtn();
        
        float btnw = btnScript.GetComponent<RectTransform>().rect.size.x;
        float btnh = btnScript.GetComponent<RectTransform>().rect.size.y;

        screenPos.z = 0;
        if (screenPos.y < 0)
            screenPos.y = btnh / 4f;
        if (screenPos.y > Screen.height - btnh / 4f)
        {
            //print(soldier.name + "," + screenPos.y + "," + Screen.height);
            screenPos.y = Screen.height - btnh / 4f;
        }

        btnScript.transform.position = screenPos;
        btnScript.gameObject.SetActive(true);
    }

    void WinGameCheck()
    {
        if (gameObject.name.Contains("red"))
        {
            if (side2Troop[Soldier.Side.Red].Count == 0 && side2Troop[Soldier.Side.Blue].Count != 0)
            {
                gameManager.BlueWin();
            }
        }
        else if (gameObject.name.Contains("blue"))
        {
            if (side2Troop[Soldier.Side.Red].Count != 0 && side2Troop[Soldier.Side.Blue].Count == 0)
            {
                gameManager.RedWin();
            }
        }
    }

    void ShowWarDuration()
    {
        if(isInWar)
        {
            float curTime = Time.time;
            float duration = curTime - warBeginTime;
            warTimeText.text = ((int)duration).ToString();
        }
    }

    public GameObject FindMyEnemy(Soldier soldier)
    {
        Soldier.Side soldSide = soldier.side;

        float mixDis = citySize[0] + citySize[1];
        GameObject result = null;
        foreach(KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            if (item.Key == soldSide)
                continue;
            foreach (GameObject enemy in item.Value)
            {
                if (enemy.GetComponent<Soldier>().isDead) continue;

                float enyDis = Vector3.Distance(soldier.transform.position, enemy.transform.position);
                if (enyDis < mixDis)
                {
                    result = enemy;
                    mixDis = enyDis;
                }
            }
        }

        return result;
    }

    public GameObject FindTauntWarrior(Soldier soldier)
    {
        Soldier.Side soldSide = soldier.side;

        GameObject result = null;
        foreach (KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            if (item.Key == soldSide)
                continue;
            foreach (GameObject enemy in item.Value)
            {
                if (enemy.GetComponent<Soldier>().isDead) continue;

                if (enemy.name == "warrior")
                {
                    result = enemy;
                }
            }
        }

        return result;
    }


    public List<GameObject> FindFriendsInRange(Soldier checksoldier, float range)
    {
        Soldier.Side soldSide = checksoldier.side;

        List<GameObject> result = new List<GameObject>();
        foreach (KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            if (item.Key == soldSide)
            {
                foreach (GameObject soldier in item.Value)
                {
                    if (checksoldier.gameObject.name == soldier.name) continue;

                    if (Vector3.Distance(checksoldier.transform.position, soldier.transform.position) < range)
                    {result.Add(soldier);}
                }
            }
        }

        return result;
    }

    public List<GameObject> FindEnemiesInRange(Soldier checksoldier, float range)
    {
        Soldier.Side soldSide = checksoldier.side;

        List<GameObject> result = new List<GameObject>();
        foreach (KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            if (item.Key != soldSide)
            {
                foreach (GameObject soldier in item.Value)
                {
                    if (Vector3.Distance(checksoldier.transform.position, soldier.transform.position) < range)
                    { result.Add(soldier); }
                }
            }
        }

        return result;
    }

    public GameObject FindLeastBloodFriend(Soldier checksoldier)
    {
        Soldier.Side soldSide = checksoldier.side;

        float leastBlood = 101;
        GameObject result = null;
        foreach (KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            if (item.Key == soldSide)
            {
                foreach (GameObject soldier in item.Value)
                {
                    if (checksoldier.gameObject.name == soldier.name) continue;

                    if (soldier.GetComponent<Soldier>().blood < leastBlood)
                    { 
                        result = soldier;
                        leastBlood = soldier.GetComponent<Soldier>().blood;
                    }
                }
            }
        }

        return result;

    }

    public void NoticeFriendsDeath(Soldier checksoldier)
    {
        foreach (KeyValuePair<Soldier.Side, ArrayList> item in side2Troop)
        {
            if (item.Key == checksoldier.side)
                foreach (GameObject friend in item.Value)
                {
                    if (checksoldier.gameObject.name == friend.name) continue;
                    friend.GetComponent<Soldier>().FriendDie(friend.GetComponent<Soldier>());
                }
        }
    }

}

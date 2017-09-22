using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {
    public int maxBlueChampionNum = 9;
    public int maxRedChampionNum = 9;
    public int maxNeutrlNum = 3;
    public int PrepareBattleTime = 3;
    public float OneAttackTime = 2.0f;
    public FightManager fightManager = null;
    public CityStatus curStatus = CityStatus.Peace;
    public List<string> debugCityMemberShow = new List<string>();

    private int defaultCol = 3;//默认的列数为3，行是row，例如第一排，也就是第一行，指的是1、2、3位置；列是col，与行相对
    private int curRound = 0;
    private Side defenceSide = Side.Neutral;//防守方（势力出手顺序为NPC最优先，其次防守方，最后进攻方（先进场先出手））
    private Vector3 cityPos = new Vector3();
    private Vector2 citySize = new Vector2();
    private List<Vector3> redTroopBtnPoints = new List<Vector3>();
    private List<Vector3> blueTroopBtnPoints = new List<Vector3>();

    //城内城外数据
    private List<ChampionBehaviour> redWaitOutCityChampions = new List<ChampionBehaviour>();
    private List<ChampionBehaviour> blueWaitOutCityChampions = new List<ChampionBehaviour>();
    private Dictionary<int, ChampionBehaviour> redPos2Champion = new Dictionary<int, ChampionBehaviour>();
    private Dictionary<int, ChampionBehaviour> bluePos2Champion = new Dictionary<int, ChampionBehaviour>();
    private Dictionary<int, ChampionBehaviour> neutralPos2Champion = new Dictionary<int, ChampionBehaviour>();

    //增删缓存，防止遍历字典的同时增删字典元素
    private Dictionary<Side, List<ChampionBehaviour>> delayInCityAddBuffer = new Dictionary<Side, List<ChampionBehaviour>>();
    private Dictionary<Side, List<ChampionBehaviour>> delayInCityDelBuffer = new Dictionary<Side, List<ChampionBehaviour>>();

    //战斗数据
    private List<Side> OneRoundAttackOrder = new List<Side>();//红蓝黄出手顺序
    private Queue<ChampionBehaviour> OneRoundChampionAttackSeq = new Queue<ChampionBehaviour>();//一回合全部英雄的攻击序列
    private float LastAttackTime = 0;

    public Text roundCountText = null;
    public Text battleTimeText = null;
    private float warBeginTime = 0;


    

	void Start () {
        InitPos2ChampionDict();
        CalCityPoints();
        InitCityData();
        SquareSoldierBtn();
        InitRoundCountText();

        debugCityMemberShow.Add("由于dictionary不能序列化，用一个list显示当前城池成员");

       
	}

    void InitPos2ChampionDict()
    {
        for (int i = 1; i <= maxRedChampionNum; i++)
        {
            redPos2Champion.Add(i, null);
        }

        for (int i = 1; i <= maxBlueChampionNum; i++)
        {
            bluePos2Champion.Add(i, null);
        }

        for (int i = 1; i <= maxNeutrlNum; i++)
        {
            neutralPos2Champion.Add(i, null);
        }

        foreach (Side item in Enum.GetValues(typeof(Side)))
        {
            delayInCityAddBuffer.Add(item, new List<ChampionBehaviour>());
            delayInCityDelBuffer.Add(item, new List<ChampionBehaviour>());
            //delayOutCityAddBuff.Add(item, new List<ChampionBehaviour>());
            //delayOutCityDelBuff.Add(item, new List<ChampionBehaviour>());
        }

    }

    void CalCityPoints()
    {
        cityPos = transform.localPosition;
        citySize = new Vector2(GetComponent<Collider>().bounds.size.x, GetComponent<Collider>().bounds.size.z);

        float x, y, z, w, h;
        x = cityPos[0];
        y = cityPos[1];
        z = cityPos[2];
        w = citySize[0] + 7;
        h = citySize[1] + 7;
        float intervalx = w / 2f * (3f / 5f);
        float intervalz = h / 2f * (3f / 5f);

        redTroopBtnPoints.Add(new Vector3(x + w / 2, y, z));
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

    void InitCityData()
    {
        string[] championTags = new string[] { "neutraltroop", "redtroop", "bluetroop" };
        foreach (string tagname in championTags)
        {
            foreach (GameObject champion in GameObject.FindGameObjectsWithTag(tagname))
            {
                ChampionBehaviour championBehaviour = champion.GetComponent<ChampionBehaviour>();
                RoundManager belongCity = championBehaviour.belongCity;
                if (belongCity.name == name)
                {
                    AddInCityChampion(championBehaviour);
                    championBehaviour.SetPosNum(championBehaviour.GetPosNum());
                }
            }
        }

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

    //排列按钮
    void SquareSoldierBtn()
    {
        Dictionary<int, ChampionBehaviour> checkTroop = null;
        List<Vector3> squarePoints = null;
        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            if (GetSideChampionNum(side) > 0 && side != Side.Neutral)
            {
                checkTroop = GetTroopBySide(side);
                if (side == Side.Red)
                {
                    squarePoints = redTroopBtnPoints;
                }
                else if (side == Side.Blue)
                {
                    squarePoints = blueTroopBtnPoints;
                }
            }
            
        }


        if (checkTroop == null) return;
        if (squarePoints == null) return;

        int index = 0;
        foreach (KeyValuePair<int, ChampionBehaviour> item in checkTroop)
        {
            if (!item.Value) continue;
            if (item.Value.gameObject.name.Contains("defender")) continue;
            if (item.Value.gameObject.GetComponent<ChampionBehaviour>().GetStatus() != Status.Peace) continue;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(squarePoints[index]);

            SetBtnWith3DPos(item.Value.gameObject, screenPos);

            index++;
        }

    }

    void InitRoundCountText()
    {
        roundCountText.transform.position = Camera.main.WorldToScreenPoint(new Vector3(cityPos[0] + citySize[0] / 2, cityPos[1], cityPos[2] + citySize[1] / 2));
        battleTimeText.transform.position = Camera.main.WorldToScreenPoint(new Vector3(cityPos[0] + citySize[0] / 2 - 2, cityPos[1], cityPos[2] + citySize[1] / 2));
    }
    
	void Update () {
        switch (curStatus)
        {
            case CityStatus.None:
                break;
            case CityStatus.Peace:
                SquareSoldierBtn();
                break;
            case CityStatus.PrepareBattle:
                break;
            case CityStatus.InRound:
                ChampionsAttackInOrder();
                break;
            case CityStatus.RoundInterval:
                CheckAllReachedBattlePosWhenInterval();
                break;
        }


        HandleDelayBuffer();
        DebugCityMemberShow();
        ShowWarDuration();
        WinGameCheck();

	}

    public int GetSideChampionNum(Side side)
    {
        Dictionary<int, ChampionBehaviour> troop = GetTroopBySide(side);
        int result = 0;
        foreach (KeyValuePair<int, ChampionBehaviour> item in troop)
        {
            if (item.Value == null) continue;
            result++;
        }
        return result;
    }


    public Dictionary<int, ChampionBehaviour> GetTroopBySide(Side side)
    {
       
        
            switch (side)
            {
                case Side.Red:
                    return redPos2Champion;
                    break;
                case Side.Blue:
                    return bluePos2Champion;
                    break;
                case Side.Neutral:
                    return neutralPos2Champion;
                    break;

            }
     
        return neutralPos2Champion;
    }

    public void ChampionLeaveCity(ChampionBehaviour champion)
    {
        Side side = champion.GetSide();
        foreach (KeyValuePair<int, ChampionBehaviour> item in GetTroopBySide(side))
        {
            if (item.Value!=null && item.Value.gameObject.name == champion.gameObject.name)
            {
                DelInCityChampion(champion);
            }
        }
        foreach (ChampionBehaviour outchp in GetAllPlayerChampionsOutCity())
        {
            if (outchp.gameObject.name == champion.gameObject.name)
            {
                DelOutCityChampion(champion);
            }
        }
    }

    public void ChampionReachedCity(ChampionBehaviour champion)
    {
        //当前战场队伍数为1，新加入的英雄不属于这个队伍，且为和平状态
        if (GetTroopsNum() == 1 && GetTroopSideIfOnlyOne() != champion.GetSide() && curStatus == CityStatus.Peace)
        {
            LaunchBattle();
            
            //防守方
            if (GetTroopSideIfOnlyOne() == Side.Neutral)
            {
                defenceSide = champion.GetSide();
            }
            else
            {
                defenceSide = GetTroopSideIfOnlyOne();
            }
        }

        
        champion.SetBelongCity(this);

        if (curStatus == CityStatus.PrepareBattle || curStatus == CityStatus.RoundInterval || curStatus == CityStatus.Peace)
        {
            
            champion.SetStatus(Status.ReachingBattlePos);
            champion.SetPosNum(GetChampionBattlePos(champion));
            AddInCityChampion(champion);

        }
        else if (curStatus == CityStatus.InRound)
        {
            AddOutCityChampion(champion);
            champion.SetStatus(Status.WaitOutCity);
        }
        
    }

    public void ChampionReachedPos(ChampionBehaviour champion)
    {
        if (curStatus == CityStatus.Peace)
        {
            champion.SetStatus(Status.Peace);
        }
        //AddInCityChampion(champion);
        //DelOutCityChampion(champion);
    }

    public void ChampionDie(ChampionBehaviour champion)
    {
        GetTroopBySide(champion.GetSide())[champion.GetPosNum()] = null; 
        if (GetTroopsNum() == 1)
        {
            BattleEnd();
        }
    }

    private void CheckAllReachedBattlePosWhenInterval()
    {
        bool isAllReached = true;
        foreach (ChampionBehaviour champion in GetAllPlayerChampionsInCity())
        {
            if (champion != null && champion.GetStatus() == Status.ReachingBattlePos)
            {
                isAllReached = false;
            }
        }
        if (isAllReached)
        {

            foreach (ChampionBehaviour champion in GetAllPlayerChampionsInCity())
            {
                champion.SetStatus(Status.RoundIdle);
            }
            LaunchRound();
        }
    }

    void LaunchBattle()
    {
        warBeginTime = Time.time;
        SetStatus(CityStatus.PrepareBattle);
        ClearRoundNum();
        SetAllInCityChampionStatus(Status.RoundIdle);
        SetAllOutCityChampionStatus(Status.ReachingBattlePos);
        StartCoroutine(Wait2FirstRound());
        
    }
 

    IEnumerator Wait2FirstRound()
    {
        yield return new WaitForSeconds(PrepareBattleTime);
        LaunchFirstRound();
    }

    void LaunchFirstRound()
    {
        //准备结束，到达站位的进入回合，未到达的回到城外
        foreach (ChampionBehaviour champion in GetAllPlayerChampionsInCity())
        {
            if (champion.GetStatus() == Status.ReachedBattlePos)
            {
                champion.SetStatus(Status.RoundIdle);
            }
            else if (champion.GetStatus() == Status.ReachingBattlePos)
            {
                //DelInCityChampion(champion);
                GetTroopBySide(champion.GetSide())[champion.GetPosNum()] = null;//需要即刻删除，后面好计算攻击序列

                champion.SetStatus(Status.WaitOutCity);
                AddOutCityChampion(champion);
                if(champion.GetSide() == Side.Red)
                {
                    champion.transform.position = new Vector3(cityPos[0] - citySize[0] / 2, cityPos[1], cityPos[2] - citySize[1] / 2);
                }
                else if (champion.GetSide() == Side.Blue)
                {
                    champion.transform.position = new Vector3(cityPos[0] + citySize[0] / 2, cityPos[1], cityPos[2] + citySize[1] / 2);
                }

            }
        }
        
        LaunchRound();
        
    }

    void LaunchRound()
    {
        IncreaseRoundNum();
        SetStatus(CityStatus.InRound);
        CalOneRoundChampionAttackSeq();
        OneChampionAttack();
    }

    void RoundEnd()
    {
        SetStatus(CityStatus.RoundInterval);
        foreach (ChampionBehaviour champion in GetAllPlayerChampionsInCity())
        {
            champion.SetStatus(Status.ReachedBattlePos);
        }

        foreach (ChampionBehaviour champion in GetAllPlayerChampionsOutCity())
        {
            champion.SetStatus(Status.ReachingBattlePos);
            champion.SetPosNum(GetChampionBattlePos(champion));
            GetTroopBySide(champion.GetSide())[champion.GetPosNum()] = champion;
            //AddInCityChampion(champion);
        }
        redWaitOutCityChampions.Clear();
        blueWaitOutCityChampions.Clear();
    }

    void CalOneRoundChampionAttackSeq()
    {
        OneRoundAttackOrder.Clear();
        OneRoundChampionAttackSeq.Clear();

        OneRoundAttackOrder.Add(Side.Neutral);
        if (defenceSide == Side.Red)
        {
            OneRoundAttackOrder.Add(Side.Red);
            OneRoundAttackOrder.Add(Side.Blue);
        }
        if (defenceSide == Side.Blue)
        {
            OneRoundAttackOrder.Add(Side.Blue);
            OneRoundAttackOrder.Add(Side.Red);
        }

        Dictionary<Side, Queue<ChampionBehaviour>> side2ChampionQueue = GetTroopQueueFromDic();

        int maxnum = 0;
        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            if (GetSideChampionNum(side) > maxnum) maxnum = GetSideChampionNum(side);
        }
        for (int i = 1; i <= maxnum; i++)
        {
            foreach (Side orderside in OneRoundAttackOrder)
            {
                if(side2ChampionQueue[orderside].Count > 0)
                {
                    ChampionBehaviour champion = side2ChampionQueue[orderside].Dequeue();
                    OneRoundChampionAttackSeq.Enqueue(champion);
                }
            }
        }


    }

    void BattleEnd()
    {
        SetStatus(CityStatus.Peace);
        defenceSide = GetTroopSideIfOnlyOne();
        foreach (ChampionBehaviour chp in GetAllPlayerChampionsInCity())
        {
            chp.BattleEnd();
        }
    }

    void ClearRoundNum()
    {
        curRound = 0;
        roundCountText.text = curRound.ToString();
        foreach (ChampionBehaviour chmp in GetAllPlayerChampionsInCity())
        {
            chmp.SetCurRoundNum(curRound);
        }
    }

    void IncreaseRoundNum()
    {
        curRound++;
        roundCountText.text = curRound.ToString();
        foreach (ChampionBehaviour chmp in GetAllPlayerChampionsInCity())
        {
            chmp.SetCurRoundNum(curRound);
        }
    }

    void SetStatus(CityStatus status)
    {
        curStatus = status;
    }

    List<ChampionBehaviour> GetAllPlayerChampionsInCity()
    { 
        List<ChampionBehaviour> result = new List<ChampionBehaviour>();
        foreach (ChampionBehaviour champion in redPos2Champion.Values)
        {
            if (champion != null && champion.IsAlive())
            {result.Add(champion);}
        }
        foreach (ChampionBehaviour champion in bluePos2Champion.Values)
        {
            if (champion != null && champion.IsAlive())
            { result.Add(champion); }
        }


        return result;
    }

    List<ChampionBehaviour> GetAllPlayerChampionsOutCity()
    {
        List<ChampionBehaviour> result = new List<ChampionBehaviour>();
        foreach (ChampionBehaviour champion in redWaitOutCityChampions)
        {
            result.Add(champion); 
        }
        foreach (ChampionBehaviour champion in blueWaitOutCityChampions)
        {
            result.Add(champion);
        }


        return result;
    }

    

    int GetTroopsNum()
    {
        int result = 0;
        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            if (GetSideChampionNum(side) > 0) result++;
        }

        return result;
    }

    //如果只有一个军队的情况下，返回这个军队的side
    Side GetTroopSideIfOnlyOne()
    {
        Side result = Side.Neutral;
        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            if (GetSideChampionNum(side) > 0) result = side;
        }

        return result;
    }

    ChampionBehaviour GetInCityChampionAtPos(Side side, int pos)
    {
        int curPos = 0;
        foreach(KeyValuePair<int, ChampionBehaviour> item in GetTroopBySide(side))
        {
            if (item.Value != null) curPos++;
            if (curPos == pos && item.Value != null) return item.Value;
        }
        return null;
    }


    void AddInCityChampion(ChampionBehaviour champion)
    {
        delayInCityAddBuffer[champion.GetSide()].Add(champion);
    }

    void DelInCityChampion(ChampionBehaviour champion)
    {
        delayInCityDelBuffer[champion.GetSide()].Add(champion);
    }

    void AddOutCityChampion(ChampionBehaviour champion)
    {
        if (champion.GetSide() == Side.Red && !redWaitOutCityChampions.Contains(champion))
        {
            redWaitOutCityChampions.Add(champion);
        }
        else if (champion.GetSide() == Side.Blue && !blueWaitOutCityChampions.Contains(champion))
        {
            blueWaitOutCityChampions.Add(champion);
        }
    }

    void DelOutCityChampion(ChampionBehaviour champion)
    {
        ChampionBehaviour delitem = null;
        foreach (ChampionBehaviour item in redWaitOutCityChampions)
        {
            if (item.gameObject.name == champion.gameObject.name) delitem = item;
        }
        if (delitem)
        {
            redWaitOutCityChampions.Remove(delitem);
        }
        foreach (ChampionBehaviour item in blueWaitOutCityChampions)
        {
            if (item.gameObject.name == champion.gameObject.name) delitem = item;
        }
        if (delitem)
        {
            blueWaitOutCityChampions.Remove(delitem);
        }
    }

    void HandleDelayBuffer()
    {
        foreach (var item in delayInCityAddBuffer)
        {
            foreach (ChampionBehaviour champion in item.Value)
            {
                GetTroopBySide(item.Key)[champion.GetPosNum()] = champion;
            }
        }
        foreach (var item in delayInCityDelBuffer)
        {
            foreach (ChampionBehaviour champion in item.Value)
            {
                GetTroopBySide(item.Key)[champion.GetPosNum()] = null;
            }
        }

        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            delayInCityAddBuffer[side].Clear();
            delayInCityDelBuffer[side].Clear();
        }
    }

    void DebugCityMemberShow()
    {
        debugCityMemberShow.Clear();
        debugCityMemberShow.Add("由于dictionary不能序列化，用一个list显示当前城池成员");
        foreach (Side side in Enum.GetValues(typeof(Side)))
        {
            foreach (KeyValuePair<int, ChampionBehaviour> item in GetTroopBySide(side))
            {
                if (item.Value == null)
                {
                    debugCityMemberShow.Add(side.ToString() + "," + item.Key + "," + "null");
                }
                else 
                {
                    debugCityMemberShow.Add(side.ToString() + "," + item.Key + "," + item.Value.gameObject.name);
                }
                
            }
        }
    }

    void SetBtnWith3DPos(GameObject soldier, Vector3 screenPos)
    {
        ClickBtn btnScript = soldier.GetComponent<ChampionBehaviour>().GetControlBtn();

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

	//获取下一个英雄的战斗站位
	int GetChampionBattlePos(ChampionBehaviour champion)
	{
		Side side = champion.GetSide ();

		switch (champion.gameObject.GetComponent<ChampionProp> ().Priority) {
		case Priority.FrontMiddleBack:
			return SetFrontMiddleBack (champion);
			break;
		case Priority.BackMiddleFront:
			return  SetBackMiddleFront (champion);
			break;
		case Priority.MiddleBackFront:
			return SetMiddleBackFront (champion);
			break;
		case Priority.MiddleFrontBack:
			return SetMiddleFrontBack (champion);
			break;
		case Priority.none:
			Debug.LogError ("优先级为none");
			return SetFrontMiddleBack (champion);
			break;
		default :
			return 0;
			break;
		}

	}

	/// <summary>
	/// 获取敌方阵容（包括中立方）
	/// </summary>
	/// <returns>The enemy dic.</returns>
	/// <param name="side">Side.</param>
   public Dictionary<int, ChampionBehaviour> GetEnemyDic(ChampionBehaviour champion)
	{
		Dictionary<int,ChampionBehaviour> enemys = new Dictionary<int, ChampionBehaviour> ();

        Side championSide = champion.GetSide();
        //int maxChampionNum = GetChampionMaxNumBySide(champion.GetSide());
        switch (championSide)
        {
			case Side.Blue:
                //for (int i = 1; i <= maxNeutrlNum; i++)
                //{
                //    enemys.Add(i, neutralPos2Champion[i]);
                //}
                //for (int i = 1; i <= redMaxNum; i++)
                //{
                //    enemys.Add(i + 3, redPos2Champion[i]);                
                //}

               //if (gameObject.name != "redcity" || gameObject.name != "bulecity")
                //{
                    foreach (KeyValuePair<int, ChampionBehaviour> item in GetTroopBySide(Side.Neutral))
                    {
                        enemys.Add(item.Key, item.Value);
                    }
               // }
                foreach (KeyValuePair<int, ChampionBehaviour> item in GetTroopBySide(GetTheOtherSide(championSide)))
                {
                    enemys.Add(item.Key + maxNeutrlNum, item.Value);
                }

				break;
			case Side.Neutral:

                Side chooseside = champion.NeutralChooseAttackSide();
                if (GetSideChampionNum(chooseside) == 0)
                {
                    foreach (KeyValuePair<int, ChampionBehaviour> item in GetTroopBySide(GetTheOtherSide(chooseside)))
                    {
                        enemys.Add(item.Key, item.Value);
                    }
                }
                else
                {
                    foreach (KeyValuePair<int, ChampionBehaviour> item in GetTroopBySide(chooseside))
                    {
                        enemys.Add(item.Key, item.Value);
                    }
                }

                //if (chooseside == Side.Blue)
                //{
                //    int n = 0;
                //    for (int i = 1; i <= maxChampionNum; i++) {
                //        if (bluePos2Champion [i] == null)
                //            n++;
                //    }
                //    if (n != 9) {
						
                //        for (int i = 1; i <= maxChampionNum; i++) {
                //            enemys.Add (i, bluePos2Champion [i]);
                //        }
                //    } else {
                //        for (int i = 1; i <= maxChampionNum; i++) {
                //            enemys.Add (i, redPos2Champion [i]);
                //        }
                //    }
                //}
                //else if (chooseside == Side.Red)
                //{
                //    int n = 0;
                //    for (int i = 1; i <= maxChampionNum; i++) {
                //        if (redPos2Champion [i] == null)
                //            n++;
                //    }
                //    if (n != 9) {
                //        for (int i = 1; i < (maxChampionNum + 1); i++) {
                //            enemys.Add (i, redPos2Champion [i]);
                //        }
                //    } else {
                //        for (int i = 1; i < (maxChampionNum + 1); i++) {
                //            enemys.Add (i, bluePos2Champion [i]);
                //        }
                //    }
                //} 
                //else {
                //    for (int i = 1; i < (maxNeutrlNum + 1); i++) {
                //        enemys.Add (i, neutralPos2Champion [i]);
                //    }
                //}
				break;
			case Side.Red:

                //for (int i = 1; i < (maxNeutrlNum + 1); i++) 
                //{
                //    enemys.Add (i, neutralPos2Champion [i]);
                //}
                //for (int i = 1; i < (maxChampionNum + 1); i++) 
                //{
                //    enemys.Add (i + 3, bluePos2Champion [i]); 
                //}

                //if (gameObject.name != "redcity" || gameObject.name != "bulecity")
                //{
                    foreach (KeyValuePair<int, ChampionBehaviour> item in GetTroopBySide(Side.Neutral))
                    {
                        enemys.Add(item.Key, item.Value);
                    }
                //}
                foreach (KeyValuePair<int, ChampionBehaviour> item in GetTroopBySide(GetTheOtherSide(championSide)))
                {
                    enemys.Add(item.Key + maxNeutrlNum, item.Value);
                }
				break;
			}
		return enemys;
	}


	/// <summary>
	/// 获取己方阵容
	/// </summary>
	/// <returns>The self dic.</returns>
	/// <param name="side">己方颜色</param>
    public Dictionary<int, ChampionBehaviour> GetSelfDic(ChampionBehaviour champion)
	{

        int maxChampionNum = GetChampionMaxNumBySide(champion.GetSide());
		Dictionary<int,ChampionBehaviour> self = new Dictionary<int, ChampionBehaviour> ();
		for (int i = 1; i <= maxChampionNum ; i++) {
			if (champion.side == Side.Red) {
				self.Add (i, redPos2Champion [i]);
			}
            if (champion.side == Side.Blue)
            {
				self.Add (i, bluePos2Champion [i]);
			}
		}
		return self;
	}

	/// <summary>
	/// 后中前优先级站位方式
	/// </summary>
	/// <returns>The set front middle back.</returns>
	/// <param name="side">Side.</param>
    int SetBackMiddleFront(ChampionBehaviour champion)
	{
		Dictionary<int,ChampionBehaviour> enemys = new Dictionary<int, ChampionBehaviour> ();
        enemys = GetEnemyDic(champion);

		Dictionary<int,ChampionBehaviour> self = new Dictionary<int, ChampionBehaviour> ();
        self = GetSelfDic(champion);

		List<int> temp = new List<int> ();
		List<int> temp1 = new List<int> ();

        int maxFriendChampionNum = 9;
        int maxEnemyChampionNum = GetChampionMaxNumBySide(GetTheOtherSide(champion.GetSide())) + GetChampionMaxNumBySide(Side.Neutral);//12+3
		if (self [7] == null || self [8] == null || self [9] == null) 
		{
            for (int i = 7; i <= maxFriendChampionNum; i++) 
			{
				if (self [i] == null) 
				{
					if (self [i - 6] != null) 
					{
						return i;
					} 
					else if (self [i - 3] != null) 
					{
						
						return i;
					}
					else 
					{
                        for (int j = i - 6; j <= maxEnemyChampionNum; j = j + 3)
                        {
                            if (enemys.ContainsKey(j) && enemys[j] != null)
                            {
                                if (!temp.Contains(i))
                                    temp.Add(i);
                            }
                            else
                            {
                                if (!temp1.Contains(i))
                                    temp1.Add(i);
                            }
                        }
                        
					}
				}
			}
			System.Random ran = new System.Random ();
			int n = ran.Next (temp.Count);
		
			if (temp.Count == 0) {
				n = ran.Next (temp1.Count);
				return temp1[n];
			}
			return temp[n];
		}
		if (self [4] == null || self [5] == null || self [6] == null) 
		{
            for (int i = 4; i <= maxFriendChampionNum - 3; i++) 
			{
				if (self [i] == null)
				{
					if (self [i - 3] != null) {
						return i;	
					}
					else 
					{
                        for (int j = i - 3; j <= maxEnemyChampionNum; j = j + 3)
                        {
                            if (enemys.ContainsKey(j) && enemys[j] != null)
                            {
                                if (!temp.Contains(i))
                                    temp.Add(i);
                            }
                            else
                            {
                                if (!temp1.Contains(i))
                                    temp1.Add(i);
                            }
                        }
					}
				}
			}
			System.Random ran = new System.Random ();
			int n = ran.Next (temp.Count);
			
			if (temp.Count == 0) {
				n = ran.Next (temp1.Count);
				return temp1[n];
			}
			return temp[n];
		}
		if (self [1] == null || self [2] == null || self [3] == null) 
		{
            for (int i = 1; i <= maxFriendChampionNum - 6; i++)
			{
				if (self [i] == null) 
				{

                    for (int j = i; j <= maxEnemyChampionNum; j = j + 3)
                    {
                        if (enemys.ContainsKey(j) && enemys[j] != null)
                        {
                            if (!temp.Contains(i))
                                temp.Add(i);
                        }
                        else
                        {
                            if (!temp1.Contains(i))
                                temp1.Add(i);
                        }
                    }
				}
			}
			System.Random ran = new System.Random ();
			int n = ran.Next (temp.Count);
			
			if (temp.Count == 0) {
				n = ran.Next (temp1.Count);
				return temp1[n];
			}
			return temp[n];
		}
		return 0;
	}

	/// <summary>
	/// 前中后优先级站位方式
	/// </summary>
	/// <returns>The front middle back.</returns>
	/// <param name="side">Side.</param>
    int SetFrontMiddleBack(ChampionBehaviour champion)
	{
		Dictionary<int,ChampionBehaviour> enemys = new Dictionary<int, ChampionBehaviour> ();
		enemys = GetEnemyDic (champion);

		Dictionary<int,ChampionBehaviour> self = new Dictionary<int, ChampionBehaviour> ();
        self = GetSelfDic(champion);

		List<int> temp = new List<int> ();
		List<int> temp1 = new List<int> ();

        int maxFriendChampionNum = 9;
        int maxEnemyChampionNum = GetChampionMaxNumBySide(GetTheOtherSide(champion.GetSide())) + GetChampionMaxNumBySide(Side.Neutral);//12+3

		if (self [1] == null || self [2] == null || self [3] == null) 
		{
            for (int i = 1; i <= maxFriendChampionNum - 6; i++) 
			{
				if (self [i] == null) 
				{
					if (self [i + 3] != null || self [i + 6] != null) 
					{
						return i;
					} 
					else 
					{
                        for (int j = i; j <= maxEnemyChampionNum; j = j + 3)
                        { 
                            if (enemys.ContainsKey(j) && enemys[j] != null)
                            {
                                if (!temp.Contains(i))
                                    temp.Add(i);
                            }
                            else 
                            {
                                if (!temp1.Contains(i))
                                    temp1.Add(i);
                            }
                        }
                        
					}
				}
			}
			System.Random ran = new System.Random ();
			int n = ran.Next (temp.Count);
			
			if (temp.Count == 0) {
				n = ran.Next (temp1.Count);
				return temp1[n];
			}
			return temp[n];
			
		}
		if (self [4] == null || self [5] == null || self [6] == null) 
		{
            for (int i = 4; i <= maxFriendChampionNum - 3; i++) 
			{
				if (self [i] == null)
				{
					if (self [i + 3] != null) {
						return i;	
					}
					else 
					{
                        for (int j = i - 3; j <= maxEnemyChampionNum; j = j + 3)
                        {
                            if (enemys.ContainsKey(j) && enemys[j] != null)
                            {
                                if (!temp.Contains(i))
                                    temp.Add(i);
                            }
                            else
                            {
                                if (!temp1.Contains(i))
                                    temp1.Add(i);
                            }
                        }
					}
                    
				}
			}

			System.Random ran = new System.Random ();
			int n = ran.Next (temp.Count);
			
			if (temp.Count == 0) {
				n = ran.Next (temp1.Count);
				return temp1[n];
			}
			return temp[n];

		}
		if (self [7] == null || self [8] == null || self [9] == null) 
		{
			for (int i = 7; i <= maxFriendChampionNum; i ++)
			{
				if (self [i] == null) 
				{
                    for (int j = i - 6; j <= maxEnemyChampionNum; j = j + 3)
                    {
                        if (enemys.ContainsKey(j) && enemys[j] != null)
                        {
                            if (!temp.Contains(i))
                                temp.Add(i);
                        }
                        else
                        {
                            if (!temp1.Contains(i))
                                temp1.Add(i);
                        }
                    }
				}
			}

			System.Random ran = new System.Random ();
			int n = ran.Next (temp.Count);
			
			if (temp.Count == 0) 
            {
				n = ran.Next (temp1.Count);
				return temp1[n];
			}
			return temp[n];
			
		}
		return 0;
	}

	/// <summary>
	/// 中后前优先级站位方式
	/// </summary>
	/// <returns>The middle back front.</returns>
	/// <param name="side">Side.</param>
    int SetMiddleBackFront(ChampionBehaviour champion)
	{
		Dictionary<int,ChampionBehaviour> enemys = new Dictionary<int, ChampionBehaviour> ();
		enemys = GetEnemyDic (champion);

		Dictionary<int,ChampionBehaviour> self = new Dictionary<int, ChampionBehaviour> ();
        self = GetSelfDic(champion);

		List<int> temp = new List<int> ();
		List<int> temp1 = new List<int> ();

        int maxFriendChampionNum = 9;
        int maxEnemyChampionNum = GetChampionMaxNumBySide(GetTheOtherSide(champion.GetSide())) + GetChampionMaxNumBySide(Side.Neutral);//12+3
    
		if (self [4] == null || self [5] == null || self [6] == null) 
		{
            for (int i = 4; i <= maxFriendChampionNum - 3; i++) 
			{
				if (self [i] == null)
				{

					if (self [i - 3] != null) {
					
						return i;	
					}
					else 
					{
                        for (int j = i - 3; j <= maxEnemyChampionNum; j = j + 3)
                        {
                            if (enemys.ContainsKey(j) && enemys[j] != null)
                            {
                                if (!temp.Contains(i))
                                {
                                    temp.Add(i);
                             
                                }
                            }
                            else
                            {
                                if (!temp1.Contains(i))
                                {
                                    temp1.Add(i);
                               
                                }
                            }
                        }
					}
				}
			}

			System.Random ran = new System.Random ();
			int n = ran.Next (temp.Count);
			if (temp.Count == 0) {
				n = ran.Next (temp1.Count);
				return temp1[n];
			}
			return temp[n];

		}
		if (self [7] == null || self [8] == null || self [9] == null) 
		{
            for (int i = 7; i <= maxFriendChampionNum; i++)
			{
				if (self [i] == null) 
				{
					temp.Add (i);
				}
			}

			System.Random ran = new System.Random ();
			int n = ran.Next (temp.Count);
	
			if (temp.Count == 0) {
				n = ran.Next (temp1.Count);
				return temp1[n];
			}
			return temp[n];

		}
		if (self [1] == null || self [2] == null || self [3] == null) 
		{
            for (int i = 1; i <= maxFriendChampionNum - 6; i++)
			{
				if (self [i] == null) 
				{
					temp.Add (i);
				}
			}

			System.Random ran = new System.Random ();
			int n = ran.Next (temp.Count);
		
			if (temp.Count == 0) {
				n = ran.Next (temp1.Count);
				return temp1[n];
			}
			return temp[n];

		}
		return 0;
	}
   
	/// <summary>
	/// 中前后优先级站位方式
	/// </summary>
	/// <returns>The middle front back.</returns>
	/// <param name="side">Side.</param>
    int SetMiddleFrontBack(ChampionBehaviour champion)
	{
		Dictionary<int,ChampionBehaviour> enemys = new Dictionary<int, ChampionBehaviour> ();
        enemys = GetEnemyDic(champion);

		Dictionary<int,ChampionBehaviour> self = new Dictionary<int, ChampionBehaviour> ();
        self = GetSelfDic(champion);

		List<int> temp = new List<int> ();
		List<int> temp1 = new List<int> ();

        int maxFriendChampionNum = 9;
        int maxEnemyChampionNum = GetChampionMaxNumBySide(GetTheOtherSide(champion.GetSide())) + GetChampionMaxNumBySide(Side.Neutral);//12+3

		if (self [4] == null || self [5] == null || self [6] == null) 
		{
            for (int i = 4; i <= maxFriendChampionNum - 3; i++) 
			{
				if (self [i] == null)
				{
					if (self [i - 3] != null) {
						return i;	
					}
					else 
					{
                        for (int j = i - 3; j <= maxEnemyChampionNum; j = j + 3)
                        {
                            if (enemys.ContainsKey(j) && enemys[j] != null)
                            {
                                if (!temp.Contains(i))
                                    temp.Add(i);
                            }
                            else
                            {
                                if (!temp1.Contains(i))
                                    temp1.Add(i);
                            }
                        }
					}
				}
			}

			System.Random ran = new System.Random ();
			int n = ran.Next (temp.Count);
	
			if (temp.Count == 0) {
				n = ran.Next (temp1.Count);
				return temp1[n];
			}
			return temp[n];

		}
		if (self [1] == null || self [2] == null || self [3] == null) 
		{
            for (int i = 1; i <= maxFriendChampionNum - 6; i++)
			{
				if (self [i] == null) 
				{
					temp.Add (i);
				}
			}

				System.Random ran = new System.Random ();
				int n = ran.Next (temp.Count);
				return temp[n];
			
		}
		if (self [7] == null || self [8] == null || self [9] == null) 
		{
            for (int i = 7; i <= maxFriendChampionNum; i++)
			{
				if (self [i] == null) 
				{
					temp.Add (i);
				}
			}

			System.Random ran = new System.Random ();
			int n = ran.Next (temp.Count);
		
			if (temp.Count == 0) {
				n = ran.Next (temp1.Count);
				return temp1[n];
			}
			return temp[n];

		}
		return 0;
	}


	public bool IsInCity(Vector3 pos)
    {
        float x, z, w, h;
        x = cityPos[0];
        z = cityPos[2];
        w = citySize[0];
        h = citySize[1];
        if (((x - w / 2) < pos[0] && pos[0] < (x + w / 2)) && ((z - h / 2) < pos[2] && pos[2] < (z + h / 2)))
            return true;
        return false;

    }

    void ChampionsAttackInOrder()
    {
        if (Time.time - LastAttackTime >= OneAttackTime) OneChampionAttack();
    }

    void OneChampionAttack()
    {
        if (OneRoundChampionAttackSeq.Count == 0)
        {
            RoundEnd();
            return;
        }
        ChampionBehaviour champion = OneRoundChampionAttackSeq.Dequeue();
		champion.Attack();
        LastAttackTime = Time.time;   
    }
	/// <summary>
	/// 选择进攻目标
	/// </summary>
	/// <returns>The aim.</returns>
	/// <param name="my">My.</param>
	public ChampionBehaviour ChooseAim(ChampionBehaviour my)
	{
        int maxChampionNum = maxRedChampionNum > maxBlueChampionNum?maxRedChampionNum:maxBlueChampionNum;
        int col = defaultCol;
        int row = maxChampionNum / col + 1;//因为要计算中立，所以加一行
        int[] leftCol = GetColBehindOnePos(1, row);
        int[] midCol = GetColBehindOnePos(2, row);
        int[] rightCol = GetColBehindOnePos(3, row);

        int[] leftPriority = Tool.MergeIntArray(Tool.MergeIntArray(leftCol, midCol), rightCol);
        int[] midPriority = Tool.MergeIntArray(Tool.MergeIntArray(midCol, leftCol), rightCol);
        int[] rightPriority = Tool.MergeIntArray(Tool.MergeIntArray(rightCol, midCol), leftCol);

        if (Array.IndexOf(leftCol, my.GetPosNum()) != -1)
        {
            return ChooseRow(my, leftPriority);
        }
        else if(Array.IndexOf(midCol, my.GetPosNum()) != -1)
        {
            return ChooseRow(my, midPriority);
        }
        else if (Array.IndexOf(rightCol, my.GetPosNum()) != -1)
        {
            return ChooseRow(my, rightPriority);
        }
        Debug.LogError("未找到目标！"+ my.gameObject.name+my.GetPosNum());
        return null;

        //switch(my.GetPosNum())
        //{
        //case 1:
        //    return ChooseRow (my, left);
        //        break;
        //    case 2:
        //    return ChooseRow (my, mid);
        //        break;
        //    case 3:
        //    return ChooseRow (my, right);
        //        break;
        //    case 4:
        //    return ChooseRow (my, left);
        //        break;
        //    case 5:
        //    return ChooseRow (my, mid);
        //        break;
        //    case 6:
        //    return ChooseRow (my, right);
        //        break;
        //    case 7:
        //    return ChooseRow (my, left);
        //        break;
        //    case 8:
        //    return ChooseRow (my, mid);
        //        break;
        //    case 9:
        //    return ChooseRow (my, right);
        //        break;
        //    default:
        //        Debug.LogError ("我的位置不对！");
        //        return null;
        //        break;
		//}
	}
	/// <summary>
	/// 选择我所在的列
	/// </summary>
	/// <returns>The row.</returns>
	/// <param name="my">My.</param>
	/// <param name="row">Row.</param>
	ChampionBehaviour ChooseRow(ChampionBehaviour my ,int[] row)
	{
		Dictionary<int,ChampionBehaviour> enemys = new Dictionary<int, ChampionBehaviour> ();
        if (my.name == (my.side.ToString() + "defender"))
        {
            enemys = GetEnemyDicOfDefender(my.GetSide());
         
        }
        else
        {
            enemys = GetEnemyDic(my);
            
        }

		for (int i = 0; i < row.Length; i++)
		{ 
			
			int n = row [i];
			if (enemys.ContainsKey (n)) {
				if (enemys [n] != null) {
				
					return enemys [n];
				}
			}
		}
		Debug.LogError ("对面无敌人！");
		return null;
	}

    /// <summary>
    /// 获得守军的敌人字典
    /// </summary>
    /// <param name="side"></param>
    /// <returns></returns>
    private Dictionary<int,ChampionBehaviour> GetEnemyDicOfDefender(Side side)
    {
        Dictionary<int,ChampionBehaviour> enemys = new Dictionary<int,ChampionBehaviour> ();

        int maxChampionNum = GetChampionMaxNumBySide(side);
        switch(side)
        {
            case Side.Blue:
                for (int i = 1; i < (maxChampionNum + 1); i++)
                {
                    enemys.Add(i + 3, redPos2Champion[i]);
                }	
                break;
            case Side.Red:         
				for (int i = 1; i < (maxChampionNum + 1); i++) {
					enemys.Add (i + 3, bluePos2Champion [i]);
				}				
                break;
            default:
                Debug.LogError("守军获取敌人失败！");
                break;
        }
        return enemys;
    }

    void WinGameCheck()
    {
        if (gameObject.name.Contains("red"))
        {
            if (GetSideChampionNum(Side.Red) == 0 && GetSideChampionNum(Side.Blue) != 0)
            {
                fightManager.BlueWin();
            }
        }
        else if (gameObject.name.Contains("blue"))
        {
            if (GetSideChampionNum(Side.Red) != 0 && GetSideChampionNum(Side.Blue) == 0)
            {
                fightManager.RedWin();
            }
        }
    }

    Dictionary<Side, Queue<ChampionBehaviour>> GetTroopQueueFromDic()
    {
        Dictionary<Side, Queue<ChampionBehaviour>> result = new Dictionary<Side, Queue<ChampionBehaviour>>();
        result.Add(Side.Blue, new Queue<ChampionBehaviour>());
        result.Add(Side.Red, new Queue<ChampionBehaviour>());
        result.Add(Side.Neutral, new Queue<ChampionBehaviour>());
        foreach (Side side in Enum.GetValues(typeof(Side)))
        { 
            foreach(KeyValuePair<int, ChampionBehaviour> item in GetTroopBySide(side))
            {
                if (item.Value != null)
                {result[side].Enqueue(item.Value);}
            }
        }

        return result;
    
    }

    void ShowWarDuration()
    {
        if (curStatus != CityStatus.Peace)
        {
            float curTime = Time.time;
            float duration = curTime - warBeginTime;
            battleTimeText.text = ((int)duration).ToString();
        }
    }

    void SetAllInCityChampionStatus(Status status)
    {
        foreach (ChampionBehaviour chmp in GetAllPlayerChampionsInCity())
        {
            chmp.SetStatus(status);
        }
    }

    void SetAllOutCityChampionStatus(Status status)
    {
        foreach (ChampionBehaviour chmp in GetAllPlayerChampionsOutCity())
        {
            chmp.SetStatus(status);
        }
    }

    public int GetChampionMaxNumBySide(Side side)
    {
        if (side == Side.Red)
        {
            return maxRedChampionNum;
        }
        else if (side == Side.Blue)
        {
            return maxBlueChampionNum;
        }
        return maxNeutrlNum;
        
    }

    //红返回蓝，蓝返回红
    private Side GetTheOtherSide(Side side)
    {
        if (side == Side.Blue)
        {
            return Side.Red;
        }
        return Side.Blue;
    }

    //返回pos位置后的,deep排的所有位置
    private int[] GetColBehindOnePos(int pos, int deep)
    {
        int[] result = new int[deep];

        for (int i = 0; i < deep; i++)
        {
            result[i] = pos + i * defaultCol;
        }

        return result;
    }

}

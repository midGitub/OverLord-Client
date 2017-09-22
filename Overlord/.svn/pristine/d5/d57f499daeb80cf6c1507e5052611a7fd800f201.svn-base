using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Soldier : MonoBehaviour {
    //属性
    public int Speed = 5;
    public float attackCD = 1;
    public float attackRange = 5;
    public float damageScale = 1.0f;
    public float attckInterval = 1.0f;
    public float attckIntervalScale = 1.0f;
    public string bulletName = "bullet";
    public string normalBullet = "bullet";

    public int blood = 300;
    public int maxBlood = 300;
    public int energy = 0;
    public int maxEnergy = 100;
    public bool isDead = false;
    public int mana = 100;
    public int maxMana = 100;

    public CityManager belongCityManager = null;
    public GameObject tarCity = null;
    public Status curStatus = Status.Peace;
    public Side side = Side.Neutral;
    public GameObject tarEnemy = null;
    public Vector3 squarePos = new Vector3();


    public float activeSkillCD = 10;
    public float passiveSkillCD = 30;
    public float curActiveCD = 0;
    public float curPassiveCD = 0;
    public float activeRange = 5;
    public float passiveRange = 5;

    private Dictionary<Buff, float> buff2time = new Dictionary<Buff, float>();
    private Dictionary<Buff, float> buff2timeBuffer = new Dictionary<Buff, float>();//由于添加buff的时候，有可能在BuffUpdate中遍历buff2time，所以加一个缓存


    public  bool isAttacking = false;
    private float rotateSpeed = 720;
    private float rotateCount = 0;
    private float runY = 1;
    private float runYAdd = 6f;

    public int onceDamage = 0;//单次伤害，可能变化


    public GameObject lifebar = null;
    public Transform lifebarTrans = null;
    public UnityEngine.UI.Slider lifebarSlider = null;
    public UnityEngine.UI.Slider energybarSlider = null;
    public UnityEngine.UI.Slider manabarSlider = null;

    public GameObject blizzard = null;
    public Soldier master = null;

    public GameObject btnPanel = null;
    public ClickBtn controlBtn = null;

    private Dictionary<string, string> soldierName2ActiveText = new Dictionary<string, string>();
    private Dictionary<string, string> soldierName2PassiveText = new Dictionary<string, string>();

    private LuaFramework.ResourceManager m_ResMgr;
    protected LuaFramework.ResourceManager ResManager
    {
        get
        {
            if (m_ResMgr == null)
            {
                m_ResMgr = AppFacade.Instance.GetManager<LuaFramework.ResourceManager>(LuaFramework.ManagerName.Resource);
            }
            return m_ResMgr;
        }
    }

    public enum Status
    { 
        None,
        OnWay,
        InWar,
        Peace
    }

    public enum Side
    {
        Red,
        Blue,
        Neutral
    }


    public enum Buff
    {
        HolyLight,  //圣光术
        HolyShield, //神圣护盾
        Requiem,    //镇魂曲
        SummonSoul, //招魂曲
        Roar,       //咆哮
        LifeBloom,   //生命绽放
        WindArrow,   //疾风箭
        Stun,       //眩晕
        Shadow,     //欢迎
        RevengeCurse,//复仇诅咒
        Exile,       //放逐 
        Blizard,     //暴风雪
        Taunt,       //嘲讽
        TauntDefense,//嘲讽减伤
        ShieldWall,  //盾墙
        Terrify,     //恐惧
    }

	// Use this for initialization
	public virtual void Start () {
        InitSkillText();
        //InitSlider();
        InitBtnPanel();
        OtherThing();
	
	}


    void InitSkillText()
    {
        soldierName2ActiveText.Add("warrior", "战：嘲讽");
        soldierName2PassiveText.Add("warrior", "战：盾墙");
        soldierName2ActiveText.Add("knight", "骑：圣光术");
        soldierName2PassiveText.Add("knight", "骑：神圣护盾");
        soldierName2ActiveText.Add("poet", "诗：镇魂曲");
        soldierName2PassiveText.Add("poet", "诗：招魂曲");
        soldierName2ActiveText.Add("druid", "德：战斗咆哮");
        soldierName2PassiveText.Add("druid", "德：生生不息");
        soldierName2ActiveText.Add("hunter", "猎：疾风箭");
        soldierName2PassiveText.Add("hunter", "猎：幻影");
        soldierName2ActiveText.Add("warlock", "术：恐惧");
        soldierName2PassiveText.Add("warlock", "术：复仇诅咒");
        soldierName2ActiveText.Add("seer", "先：治疗术");
        soldierName2PassiveText.Add("seer", "先：放逐");
        soldierName2ActiveText.Add("mage", "法：暴风雪");
        soldierName2PassiveText.Add("mage", "法：冰箱");
        soldierName2ActiveText.Add("pastor", "牧：圣疗术");
        soldierName2PassiveText.Add("pastor", "牧：庇护");
    }

    public void InitSlider()
    {
        if (lifebarTrans != null) return;

        //ResManager.LoadPrefab("LifeBar".ToLower() + LuaFramework.AppConst.ExtName, "LifeBar", (System.Action<UnityEngine.Object[]>)(objs =>
        //{
        //    GameObject uiroot3d = GameObject.Find("3DCanvas");
        //    GameObject lifebarPreb = objs[0] as GameObject;
        //    lifebar = (GameObject)Instantiate(lifebarPreb);
        //    if (UnityEngine.Object.Equals((UnityEngine.Object)lifebar, (UnityEngine.Object)null))
        //        return;
        //    lifebarTrans = lifebar.transform;
        //    lifebarTrans.SetParent(uiroot3d.transform);
        //    lifebarTrans.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        //    lifebarSlider = lifebarTrans.GetComponent<UnityEngine.UI.Slider>();
        //    energybarSlider = lifebarTrans.Find("energy").GetComponent<UnityEngine.UI.Slider>();
        //    manabarSlider = lifebarTrans.Find("mana").GetComponent<UnityEngine.UI.Slider>();
        //    UpdateLifebar();
        //})
        //    );


    }

    public void DestroySlider()
    {
        lifebarSlider = null;
        energybarSlider = null;
        manabarSlider = null;
        if (lifebarTrans != null)
        {
            lifebarTrans.gameObject.SetActive(false);
            Destroy(lifebarTrans.gameObject);
        }
        lifebarTrans = null;
    
    }

    public void InitBtnPanel()
    {
        if (side == Side.Red)
        {
            btnPanel = GameObject.Find("/2DCanvas/redbtn");
        }
        else if (side == Side.Blue)
        {
            btnPanel = GameObject.Find("/2DCanvas/bluebtn");
        }
        
    }

    public virtual void OtherThing()
    { }

    public void SetStatus(Status status)
    {
        curStatus = status;
    }

    public void SetBelongCity(CityManager citymanager)
    {
        belongCityManager = citymanager;
    }

    public void SetTarCity(GameObject city)
    {
        tarCity = city;
    }

    public bool IsOnWay()
    {
        return (curStatus == Status.OnWay);
    }

	// Update is called once per frame
	 void Update () {
        switch (curStatus)
        {
            case Status.None: 
                break;
            case Status.InWar:
                UpdateAIOrder();
                break;
            case Status.OnWay:
                energy = 0;
                GoToTarCity();
                break;
            case Status.Peace:
                energy = 0;
                GoToSquarePos();
                break;

        
        }

        AnimateUpdate();
        UpdateLifebar();
        SkillUpdate();
        BuffUpdate();
	
	}

    void UpdateAIOrder()
    {
        if (HasBuff(Buff.Taunt))
        {
            GameObject warrior = belongCityManager.FindTauntWarrior(this);
            if (warrior == null)
                tarEnemy = belongCityManager.FindMyEnemy(this);
            else
                tarEnemy = warrior;

        }
        else
            {tarEnemy = belongCityManager.FindMyEnemy(this);}


        if (tarEnemy == null) return;

        if (HasBuff(Buff.Terrify))
        {
            transform.LookAt(tarEnemy.transform);
            transform.Translate(-Vector3.forward * Time.deltaTime * Speed * 0.2f);
            if (!belongCityManager.IsInCity(transform.position))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * Speed * 0.2f);
            }
            return;
        }

        if (Vector3.Distance(transform.position, tarEnemy.transform.position) > attackRange && CanMove())
        {
            transform.LookAt(tarEnemy.transform);
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        }
        else
        {
            if (CanAttack())
            {
                isAttacking = true;
                energy = energy + 10;
                StartCoroutine(Attack());
            }
        }
    }

    void GoToTarCity()
    {
        if (tarCity == null)
            return;
        Transform tarTans = tarCity.transform;
        transform.LookAt(tarTans);
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);

    }

    public bool CanAttack()
    {
        if (isAttacking == false && (!HasBuff(Buff.Stun)) && (!HasBuff(Buff.Exile)))
            return true;
        return false;

    }

    bool CanMove()
    {
        return ((!HasBuff(Buff.Stun)) && (!HasBuff(Buff.Exile)));
    }

    public IEnumerator Attack()
    {
        //ResManager.LoadPrefab("Bullet".ToLower() + LuaFramework.AppConst.ExtName, bulletName, (System.Action<UnityEngine.Object[]>)(objs =>
        //{
        //    GameObject bulletPreb = objs[0] as GameObject;
        //    GameObject createBullet = (GameObject)Instantiate(bulletPreb);
        //    if (UnityEngine.Object.Equals((UnityEngine.Object)createBullet, (UnityEngine.Object)null))
        //        return;
        //    createBullet.GetComponent<Bullet>().SetInfo(gameObject, tarEnemy);
        //    createBullet.GetComponent<Bullet>().SetDamageScale(damageScale);

        //    rotateCount = 360;
            
        //})
        //    );

        yield return new WaitForSeconds(attckInterval * attckIntervalScale);
        isAttacking = false;
    }


    void GoToSquarePos()
    {
        if (squarePos == Vector3.zero)
            return;

       
        if (Vector3.Distance(transform.position, squarePos) > 0.5)
        {


            Vector3 dir = squarePos - transform.position;
            dir.Normalize();
            transform.position = transform.position + dir * Time.deltaTime * Speed;
           // transform.Translate(dir * Time.deltaTime * Speed);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        //if (belongCityManager == null)
        //    return;
        //if (hit.gameObject.name.Contains("Plane") || hit.gameObject.name.Contains("city"))
        //    return;
        //if (curStatus == Status.OnWay && belongCityManager.IsInCity(transform.position))
        //    return;
        //if (hit.GetComponent<Soldier>().curStatus == Status.OnWay && hit.GetComponent<Soldier>().belongCityManager.IsInCity(transform.position))
        //    return;

        //Transform hitTrans = hit.transform;
        //Vector3 oldpos = hitTrans.position;
        //Vector3 dir = hitTrans.position - transform.position;
        //dir.Normalize();
        //dir = 1.414f * dir;
        //Vector3 newpos = new Vector3(hitTrans.position[0] + dir[0], hitTrans.position[1], hitTrans.position[2] + dir[2]);

        //if (belongCityManager.IsInCity(oldpos) && belongCityManager.IsInCity(newpos))
        //{hitTrans.position = newpos;}

    }

    public void WarriorShock(Soldier warrior)
    {
        Transform hitTrans = transform;
        Vector3 oldpos = hitTrans.position;
        Vector3 dir = hitTrans.position - warrior.transform.position;
        dir.Normalize();
        dir = 3.0f * dir;
        Vector3 newpos = new Vector3(hitTrans.position[0] + dir[0], hitTrans.position[1], hitTrans.position[2] + dir[2]);

        if (belongCityManager.IsInCity(oldpos) && belongCityManager.IsInCity(newpos))
        { hitTrans.position = newpos; }
    }

    public virtual void ReceiveDamage(int damage)
    {
        if (isDead) return;

        if (HasBuff(Buff.HolyLight))
            damage = (int)((float)damage * 0.7f);
        if (HasBuff(Buff.HolyShield))
            damage = 0;
        if (HasBuff(Buff.Requiem))
            damage = (int)((float)damage * 0.8f);
        if (HasBuff(Buff.Shadow) && Random.value < 0.3)
            damage = 0;
        if (HasBuff(Buff.TauntDefense))
            damage = (int)((float)damage * 0.5f);

        blood = blood - damage;
        onceDamage = damage;
        StartCoroutine(ShowBooldText());
        energy = energy + 10;
        if (blood <= 0)
        {
            Die();
        }
    }

    public IEnumerator ShowBooldText()
    {
        lifebarTrans.Find("blood").gameObject.SetActive(true);
        lifebarTrans.Find("blood").GetComponent<Text>().text = onceDamage.ToString();
        yield return new WaitForSeconds(0.7f);
        lifebarTrans.Find("blood").gameObject.SetActive(false);
    }

    public virtual void Die()
    {
        isDead = true;
        lifebar.SetActive(false);
        gameObject.SetActive(false);
        belongCityManager.NoticeFriendsDeath(this);
    }

    public virtual void FriendDie(Soldier friend)
    { 
        
    }

    public void ReviveBoold(int num)
    {
        blood = blood + num;
        if (blood > maxBlood) blood = maxBlood;
    }


    void AnimateUpdate()
    {
        switch (curStatus)
        {
            case Status.None:
                break;
            case Status.InWar:
                transform.localPosition = new Vector3(transform.localPosition[0], 1, transform.localPosition[2]);
                if (rotateCount > 0)
                {
                    rotateCount = rotateCount - Time.deltaTime * rotateSpeed / attckIntervalScale;
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles[0], transform.localEulerAngles[1] + Time.deltaTime * rotateSpeed, transform.localEulerAngles[2]);
                }

                break;
            case Status.OnWay:
                if (runY < 1)
                    runYAdd = 6f;
                else if (runY > 3)
                    runYAdd = -6f;
                runY = runY + runYAdd * Time.deltaTime;
                transform.localPosition = new Vector3(transform.localPosition[0], runY, transform.localPosition[2]);
                break;
            case Status.Peace:
                transform.localPosition = new Vector3(transform.localPosition[0], 1, transform.localPosition[2]);
                break;


        }

        
    }

    public void UpdateLifebar()
    {
        
        if (lifebarTrans == null || lifebarSlider == null || lifebar == null)
            return;

        //lifebar.SetActive(!isDead);
        // 更新生命条
        lifebarSlider.value = (float)blood / (float)maxBlood;
        // 更新位置
        Vector3 lifebarpos = this.transform.position;
        lifebarpos.y += 2.0f;
        lifebarpos.z += 1.5f;
        lifebarTrans.position = lifebarpos;
        // 更新角度
        lifebarTrans.eulerAngles = Camera.main.transform.eulerAngles;

        energybarSlider.value = (float)energy / (float)maxEnergy;
        manabarSlider.value = (float)mana / (float)maxMana;

        if ((float)energy / (float)maxEnergy >= 1)
        {lifebarTrans.Find("energy").transform.Find("Fill Area/Fill").GetComponent<Image>().color = Color.blue;}
        else
        { lifebarTrans.Find("energy").transform.Find("Fill Area/Fill").GetComponent<Image>().color = Color.yellow; }
    }

    

    public virtual void SkillUpdate()
    {
        if (mana <= 0)
        {
            lifebarTrans.Find("energy").gameObject.SetActive(false);
            return;
        }

        if (IsActiveReady() && energy >= maxEnergy && mana>=10)
        {
            LaunchActiveSkill();
            curActiveCD = activeSkillCD;
            energy = 0;
            mana -= 10;
            StartCoroutine(ShowActiveSkillText());
        }

        if (IsPassiveReady() && mana >= 5)
        {
            LaunchPassiveSkill();
            curPassiveCD = passiveSkillCD;
            mana -= 5;
            StartCoroutine(ShowPassiveSkillText());
        }
            
    }

    public virtual bool IsActiveReady()
    {
        if (curActiveCD > 0)
        {curActiveCD = curActiveCD - Time.deltaTime;}

        if (curActiveCD <= 0)
            return true;
        return false;
        
    }

    public virtual void LaunchActiveSkill()
    { 
        
    }

    public IEnumerator ShowActiveSkillText()
    {
        if (!IsDefender())
        {
            lifebarTrans.Find("Text").gameObject.SetActive(true);
            lifebarTrans.Find("Text").GetComponent<Text>().text = soldierName2ActiveText[gameObject.name];
            //Vector3 lastScale = lifebarTrans.FindChild("Text").GetComponent<RectTransform>().localScale;
            //lifebarTrans.FindChild("Text").GetComponent<RectTransform>().localScale = new Vector3(lastScale.x + 0.1f, lastScale.y + 0.1f, lastScale.z + 0.1f);
        }
        yield return new WaitForSeconds(2);
        lifebarTrans.Find("Text").gameObject.SetActive(false);
    }

    public IEnumerator ShowPassiveSkillText()
    {
        if (!IsDefender())
        {
            lifebarTrans.Find("Text").gameObject.SetActive(true);
           
            lifebarTrans.Find("Text").GetComponent<Text>().text = soldierName2PassiveText[gameObject.name];
            //Vector3 lastScale = lifebarTrans.FindChild("Text").GetComponent<RectTransform>().localScale;
            //lifebarTrans.FindChild("Text").GetComponent<RectTransform>().localScale = new Vector3(lastScale.x + 0.1f, lastScale.y + 0.1f, lastScale.z + 0.1f);
        }
        yield return new WaitForSeconds(2);
        lifebarTrans.Find("Text").gameObject.SetActive(false);
    }

    public virtual bool IsPassiveReady()
    {
        if (curPassiveCD > 0)
        { curPassiveCD = curPassiveCD - Time.deltaTime; }

        if (curPassiveCD <= 0)
            return true;
        return false;

    }

    public virtual void LaunchPassiveSkill()
    {

    }

    public void AddBuff(Buff buff)
    {

        if (buff2time.ContainsKey(buff) || buff2timeBuffer.ContainsKey(buff))
            return;
        //print("AddBuff" + buff + Time.time);
        buff2timeBuffer.Add(buff, Time.time);
        if (buff == Buff.Roar)
        {
            damageScale = damageScale * 1.2f;
        }

        if (buff == Buff.WindArrow)
        {
            attckIntervalScale = 1/1.5f;
            bulletName = "stunbullet";
        }

        if (buff == Buff.RevengeCurse)
        {
            damageScale = damageScale * 0.7f;
        }

        if (buff == Buff.ShieldWall)
        {
            ReviveBoold((int)(maxBlood * 0.3f));
            maxBlood = (int)(maxBlood * 1.3f);
        }


    }

    public void BuffUpdate()
    {
        List<Buff> delList = new List<Buff>();
        foreach (KeyValuePair<Buff, float> item in buff2time)
        {
            float cdtime = 0;
            switch (item.Key)
            {
                case Buff.HolyLight:
                    cdtime = 5;
                    break;
                case Buff.HolyShield:
                    cdtime = 5;
                    break;
                case Buff.Requiem:
                    cdtime = 10;
                    break;
                case Buff.Roar:
                    cdtime = 5;
                    break;
                case Buff.LifeBloom:
                    ReviveBoold((int)((float)maxBlood * 0.02f * Time.deltaTime));
                    cdtime = 5;
                    break;
                case Buff.WindArrow:
                    cdtime = 10;
                    break;
                case Buff.Stun:
                    cdtime = 2;
                    break;
                case Buff.Shadow:
                    cdtime = 5;
                    break;
                case Buff.RevengeCurse:
                    cdtime = 8;
                    break;
                case Buff.Exile:
                    cdtime = 5;
                    break;
                case Buff.Blizard:
                    blood = blood - (int)((float)maxBlood * 0.02f * Time.deltaTime);
                    //ReceiveDamage((int)((float)maxBlood * 0.02f * Time.deltaTime));
                    cdtime = 5;
                    break;
                case Buff.Taunt:
                    cdtime = 5;
                    break;
                case Buff.TauntDefense:
                    cdtime = 5;
                    break;
                case Buff.ShieldWall:
                    cdtime = 15;
                    break;
                case Buff.Terrify:
                    cdtime = 5;
                    break;

            }
            if ((Time.time - item.Value) > cdtime)
            {
                delList.Add(item.Key);
            }
        }

        //遍历buff2time之后，再把buff2timeBuffer里的东西放到buff2time
        foreach (KeyValuePair<Buff, float> item in buff2timeBuffer)
        {
            buff2time.Add(item.Key, item.Value);
        }
        buff2timeBuffer.Clear();

        foreach (Buff buffid in delList)
        {
            DelBuff(buffid);
        }
    }

    public void DelBuff(Buff buff)
    {

        if (!buff2time.ContainsKey(buff)) return;

        //print("DelBuff-----------" + buff.ToString() + Time.time);
        buff2time.Remove(buff);

        if (buff == Buff.Roar)
        {
            damageScale = damageScale / 1.2f;
        }
        if (buff == Buff.Roar)
        {
            attckIntervalScale = 1.0f;
        }
        if (buff == Buff.WindArrow)
        {
            attckIntervalScale = 1.0f;
            bulletName = normalBullet;
        }
        if (buff == Buff.RevengeCurse)
        {
            damageScale = damageScale / 0.7f;
        }

        if (buff == Buff.ShieldWall)
        {
            maxBlood = (int)(maxBlood / 1.3f);
        }

    }

    public bool HasBuff(Buff buff)
    {
        if (buff2time.ContainsKey(buff))
            return true;
        return false;
    }

    public bool IsDefender()
    {
        if (gameObject.name.Contains("defender"))
            return true;
        return false;
    }

    public ClickBtn GetControlBtn()
    {
        Transform btnTrans = btnPanel.transform.Find(gameObject.name + "btn");
        return btnTrans.GetComponent<ClickBtn>();
    }
}


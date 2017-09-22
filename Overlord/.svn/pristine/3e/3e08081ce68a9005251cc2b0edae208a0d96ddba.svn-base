using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionBehaviour : MonoBehaviour {
    public Side side = Side.Neutral;
    public Status curStatus = Status.Peace;
    public int posNum = 0;
    public RoundManager belongCity = null;
    public GameObject btnPanel = null;
    
    private ChampionProp prop = null;
    private ChampionSkill skillLogic = null;
    private NeutralAI neutralAI = null;

    private float rotateSpeed = 720;
    private float rotateCount = 0;
    private float runY = 1;
    private float runYAdd = 6f;
    private int onceDamage = 0;//单次伤害，可能变化
    private Transform battlePosTarTrans = null;
    private Transform tarCity = null;
	private bool IsDead  = false;
    private float ActionSpeed = 1.0f;//攻击动画行走速度，保证在OneAttackTime时间内完成攻击动画


    
    private GameObject lifebar = null;
    public Transform lifebarTrans = null;
    private UnityEngine.UI.Slider lifebarSlider = null;
    private UnityEngine.UI.Slider energybarSlider = null;
    private UnityEngine.UI.Slider manabarSlider = null;

    private float lastOnWayAttackTime = 0;
    private float onWayAttackInterval = 1f;

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
	/// <summary>
	/// 近战移动控制
	/// </summary>
	private bool ctl = true;
    /// <summary>
    /// 我在阵中的位置（忽略Y轴）
    /// </summary>
    Vector3 myPos = Vector3.zero;
    /// <summary>
    /// 目标（敌人）的位置（忽略Y轴）
    /// </summary>
    Vector3 tarPos = Vector3.zero;

    void Awake()
    {
        if(prop == null)
            prop = GetComponent<ChampionProp>();
        if(skillLogic == null)
            skillLogic = GetComponent<ChampionSkill>();
        if(neutralAI == null)
            neutralAI = GetComponent<NeutralAI>();
    }

	void Start () {
       
		
	}


    public void InitSlider()
    {
        if (lifebarTrans != null) return;

        ResManager.LoadPrefab("LifeBar".ToLower() + LuaFramework.AppConst.ExtName, "LifeBar", (System.Action<UnityEngine.Object[]>)(objs =>
        {
            GameObject uiroot3d = GameObject.Find("3DCanvas");
            GameObject lifebarPreb = objs[0] as GameObject;
            lifebar = (GameObject)Instantiate(lifebarPreb);
            if (UnityEngine.Object.Equals((UnityEngine.Object)lifebar, (UnityEngine.Object)null))
                return;
            lifebarTrans = lifebar.transform;
            lifebarTrans.SetParent(uiroot3d.transform);
            lifebarTrans.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            lifebarSlider = lifebarTrans.GetComponent<UnityEngine.UI.Slider>();
            energybarSlider = lifebarTrans.Find("energy").GetComponent<UnityEngine.UI.Slider>();
            manabarSlider = lifebarTrans.Find("mana").GetComponent<UnityEngine.UI.Slider>();
            UpdateLifebar();
        })
            );


    }
	// Update is called once per frame
	void Update () {
        switch (curStatus)
        {
            case Status.OnWay:
                OnWayUpdate();
                break;
            case Status.WaitOutCity:
                break;
            case Status.Peace:
                break;
            case Status.ReachingBattlePos:
                GotoBattlePos();
                break;
            case Status.ReachedBattlePos:
                break;
            case Status.RoundIdle:
                break;
		case Status.RoundAttacking:
			if (prop.attackDistance == AttackDistance.Short) {
				MoveAndAttack ();
			}
				break;
        }

        UpdateLifebar();
		
	}
	
	void MoveAndAttack()
	{

		if (Vector3.Distance (transform.position, tarPos) > 1 && ctl) {
			
			transform.LookAt (tarPos);
            transform.Translate(Vector3.forward * Time.deltaTime * ActionSpeed);

		} 
		if (Vector3.Distance (transform.position, tarPos) <= 1) {
			//Debug.Log("我在攻击！");
			ctl = false;
		}
		if(Vector3.Distance (transform.position, myPos) > 1 && !ctl)  
		{
			transform.LookAt (myPos);
            transform.Translate(Vector3.forward * Time.deltaTime * ActionSpeed);
			if (Vector3.Distance (transform.position, myPos) <= 1) {
				transform.LookAt (tarPos);
			}
		}
	}

	//ChampionBehaviour tar;
	public void Attack(ChampionBehaviour tar = null)
    {
        if (GetComponent<ChampionSkill>().CanAttack() && IsAlive())
        {           
            curStatus = Status.RoundAttacking;
            tar = belongCity.ChooseAim(this);
            if (prop.attackDistance == AttackDistance.Short)
            {
                ctl = true;
                myPos = new Vector3(battlePosTarTrans.position.x, transform.position.y, battlePosTarTrans.position.z);
                    //tar = belongCity.ChooseAim (this);
                tarPos = new Vector3(tar.transform.position.x, transform.position.y, tar.transform.position.z);
                ReachedVit(tar);
                ActionSpeed = (Vector3.Distance(myPos, tarPos) * 2) / belongCity.OneAttackTime;
            }
            else
            {
                CreateBullet(tar);
            }
            if (prop.energy >= prop.maxEnergy && prop.mana >= 10)
            {
                skillLogic.LaunchActiveSkill();
                prop.energy = 0;
                prop.mana -= 10;
            }
        }
    }

    public void CreateBullet(ChampionBehaviour tar)
    {
        string bulletName = skillLogic.GetBulletName();
        GameObject createBullet = null;
        ResManager.LoadPrefab("bullet".ToLower() + LuaFramework.AppConst.ExtName, bulletName, (System.Action<UnityEngine.Object[]>)(objs =>
        {
            GameObject bulletPreb = objs[0] as GameObject;
            createBullet = (GameObject)Instantiate(bulletPreb);
            if (UnityEngine.Object.Equals((UnityEngine.Object)createBullet, (UnityEngine.Object)null))
            {
                Debug.LogWarning("UnityEngine.Object.Equals((UnityEngine.Object)createBullet, (UnityEngine.Object)null)");
                return;
            }
            createBullet.GetComponent<Bullet>().SetInfo(gameObject, tar.gameObject);
            createBullet.GetComponent<Bullet>().SetDamage(skillLogic.CalRealBulletHurt(prop.damage));
        })
             );
    }

	/// <summary>
	/// 敌人减血
	/// </summary>
	/// <param name="tar">敌人</param>
	public void ReachedVit(ChampionBehaviour tar)
	{
        if (!IsDead)
            tar.ReceiveDamage(prop.damage, this);
	}

    public Side GetSide()
    {
        return side;
    }

    public void SetStatus(Status status)
    {
        curStatus = status;
    }

    public Status GetStatus()
    {
        return curStatus;
    }

    public bool IsAlive()
    {
        return (!IsDead);
    }


    public bool IsDefender()
    {
        if (gameObject.name.Contains("defender"))
            return true;
        return false;
    }

    public void SetBelongCity(RoundManager city)
    {
        belongCity = city;
    }

    public void SetPosNum(int num)
    {
        posNum = num;
        string childname = "";
        if (side == Side.Red)
        {
            childname = "redpos";
        }
        else if (side == Side.Blue)
        {
            childname = "bluepos";
        }
        else
        {
            childname = "neutralpos";
        }
        battlePosTarTrans = belongCity.transform.Find(childname).Find(num.ToString());
    }

    public int GetPosNum()
    {
        return posNum;
    }

    void GotoBattlePos()
    {
        if (Vector3.Distance(transform.position, battlePosTarTrans.position + new Vector3(0, 2, 0)) < GlobalDefineNum.ReachDistance)
        {
            SetStatus(Status.ReachedBattlePos);
            belongCity.ChampionReachedPos(this);
            
        }
        else
        {
            transform.LookAt(battlePosTarTrans.position + new Vector3(0,2,0));
            transform.Translate(Vector3.forward * Time.deltaTime * prop.Speed);
        }
    }
    /// <summary>
    /// 获取英雄最大血量
    /// </summary>
    /// <returns></returns>
    public float GetMaxBlood()
    {
        return prop.maxBlood;
    }
    /// <summary>
    /// 获取英雄当前血量
    /// </summary>
    /// <returns></returns>
    public float GetBlood()
    {
        return prop.blood;
    }
    /// <summary>
    /// 增加英雄当前血量
    /// </summary>
    /// <param name="blood">增加值</param>
    public void AddBlood(float blood)
    {
        
            if (prop.blood + blood < prop.maxBlood)
                prop.blood += blood;
            else
                prop.blood = prop.maxBlood;
        
    }

    public ClickBtn GetControlBtn()
    {
        Transform btnTrans = btnPanel.transform.Find(gameObject.name + "btn");
        return btnTrans.GetComponent<ClickBtn>();
    }

    public void SetTarCity(Transform city)
    {
        tarCity = city;
    }

	public void Die()
	{
		belongCity.ChampionDie (this);
		prop.isDead = true;
		IsDead = true;
		lifebar.SetActive(false);
		gameObject.SetActive(false);

	}

    public void UpdateLifebar()
    {

        if (lifebarTrans == null || lifebarSlider == null || lifebar == null)
            return;

        //lifebar.SetActive(!isDead);
        // 更新生命条
        lifebarSlider.value = (float)prop.blood / (float)prop.maxBlood;
        // 更新位置
        Vector3 lifebarpos = this.transform.position;
        lifebarpos.y += 2.0f;
        lifebarpos.z += 1.5f;
        lifebarTrans.position = lifebarpos;
        // 更新角度
        lifebarTrans.eulerAngles = Camera.main.transform.eulerAngles;

        energybarSlider.value = (float)prop.energy / (float)prop.maxEnergy;
        manabarSlider.value = (float)prop.mana / (float)prop.maxMana;

        if ((float)prop.energy / (float)prop.maxEnergy >= 1)
        { lifebarTrans.Find("energy").transform.Find("Fill Area/Fill").GetComponent<Image>().color = Color.blue; }
        else
        { lifebarTrans.Find("energy").transform.Find("Fill Area/Fill").GetComponent<Image>().color = Color.yellow; }
    }

    void OnWayUpdate()
    {
        ChampionBehaviour tar = GetOnWayEnemy();
        //城外即时战斗
        if (tar != null && !belongCity.IsInCity(transform.position))
        {
            if (Time.time - lastOnWayAttackTime > onWayAttackInterval)
            {
                CreateBullet(tar);
                lastOnWayAttackTime = Time.time;
            }
        }
        else
        {
            GoToTarCity();
        }

    }

    void GoToTarCity()
    {
        if (tarCity == null)
            return;
        Transform tarTans = tarCity;
        transform.LookAt(tarTans);
        transform.Translate(Vector3.forward * Time.deltaTime * prop.Speed);

        if (tarCity.GetComponent<RoundManager>().IsInCity(transform.position))
        {
            tarCity.GetComponent<RoundManager>().ChampionReachedCity(this);
            belongCity.fightManager.DelOnWayChampionList(this);
            
        }

    }
    public IEnumerator ShowActiveSkillText(string skill)
    {
        GameObject txt = lifebarTrans.Find("Text").gameObject;
        txt.SetActive(true);
        txt.GetComponent<Text>().text = skill;

        yield return new WaitForSeconds(2);
        txt.SetActive(false);
    }
    public virtual void ReceiveDamage(float damage, ChampionBehaviour att)
    {
        if (prop.isDead) return;

        
        float realdamage = skillLogic.CalRealDamageReceived(damage);
        if (neutralAI != null) neutralAI.DamageCount(damage, att);

        prop.blood -= realdamage;
        prop.energy = prop.energy + 10;
        if (prop.blood <= 0)
        {
            Die();
        }
        skillLogic.DamageCount += 1;
        skillLogic.SetCurNumber();
    }

    public void AddBuff(Buff buff)
    {
        skillLogic.AddBuff(buff);
    }

    public void SetCurRoundNum(int num)
    {
        skillLogic.SetCurRoundNum(num);
    }

    public void BattleEnd()
    {
        skillLogic.ClearAllBuff();
        SetStatus(Status.Peace);
        if (neutralAI != null) neutralAI.ClearDamageCount();
    }

    public Side NeutralChooseAttackSide()
    {
        return neutralAI.ChooseAttackSide();   
    }

    public ChampionBehaviour GetOnWayEnemy()
    {
        List<ChampionBehaviour> onWayChampions = new List<ChampionBehaviour>(belongCity.fightManager.GetOnWayChampionList());
        foreach (var chmp in onWayChampions)
        {
            if (!chmp.IsAlive()) continue;
            if (chmp.GetSide() == this.GetSide()) continue;
            if (Vector3.Distance(this.transform.position, chmp.transform.position) < prop.attackRange)
            {
                return chmp;
            }
        }
        return null;
    }

}

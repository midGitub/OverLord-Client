using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionSkill : MonoBehaviour {

    public ChampionBehaviour champion = null;
   
    /// <summary>
    /// buff添加时的回合数
    /// </summary>
    private Dictionary<Buff, int> buff2round = new Dictionary<Buff, int>();
    /// <summary>
    /// 回合数
    /// </summary>
    private int curRound = 0; 
    /// <summary>
    /// buff持续回合数
    /// </summary>
    private Dictionary<Buff, int> buff2ContinueRound = new Dictionary<Buff, int>(); 
    /// <summary>
    /// buff添加时的受击的次数
    /// </summary>
    private Dictionary<Buff, int> buff2Number = new Dictionary<Buff, int>();
    /// <summary>
    /// buff持续的次数
    /// </summary>
    private Dictionary<Buff, int> buff2ContinueNumber = new Dictionary<Buff, int>();
    /// <summary>
    /// 受到伤害的次数
    /// </summary>
    public int DamageCount = 0;

    void Awake()
    {
        if(champion == null)
            champion = GetComponent<ChampionBehaviour>();
    }
    void Start()
    {

        buff2ContinueRound.Add(Buff.ShieldWall, 1);
        //buff2ContinueRound.Add(Buff.HolyShield, 999);
        buff2ContinueRound.Add(Buff.Roar,1);
        buff2ContinueRound.Add(Buff.Requiem, 1);
        buff2ContinueRound.Add(Buff.Blizard, 1);
        buff2ContinueRound.Add(Buff.Terrify, 1);
        buff2ContinueRound.Add(Buff.WindArrow, 1);

        //buff2ContinueNumber.Add(Buff.ShieldWall, 999);
        buff2ContinueNumber.Add(Buff.HolyShield, 3);
        //buff2ContinueNumber.Add(Buff.Roar, 999);
        //buff2ContinueNumber.Add(Buff.Requiem, 999);
        //buff2ContinueNumber.Add(Buff.Blizard, 999);
        //buff2ContinueNumber.Add(Buff.Terrify, 999);
        //buff2ContinueNumber.Add(Buff.WindArrow, 999);
    }

    /// <summary>
    /// 各个英雄去改写自己的bullet以创建不同的子弹
    /// </summary>
    /// <returns> 子弹名</returns>
    public virtual string GetBulletName()
    {
        return "bullet";
    }

    /// <summary>
    /// 根据当前状态计算攻击加成
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public float CalRealBulletHurt(float damage)
    {
        if (HasBuffInbuff2round(Buff.Roar) && HasBuffInbuffNumber(Buff.Roar))
        {
            damage = damage * 1.2f;
        }
        if (HasBuffInbuff2round(Buff.Blizard) && HasBuffInbuffNumber(Buff.Blizard))
        {
            damage = damage * 0.7f;
        }
        return damage;

    }
    

    //根据当前状态计算真正受到的伤害
    public float CalRealDamageReceived(float damage)
    {
        if (HasBuffInbuff2round(Buff.Requiem) && HasBuffInbuffNumber(Buff.Requiem))
        {
            damage = damage * 0.8f;
        }
        if (HasBuffInbuff2round(Buff.ShieldWall) && HasBuffInbuffNumber(Buff.ShieldWall))
        {
            damage = damage * 0.5f;
        }
        if (HasBuffInbuff2round(Buff.HolyShield) && HasBuffInbuffNumber(Buff.HolyShield))
        {
            damage = 0f;
        }
        return damage;
        
    }

    

    //各个英雄继承重写
    public virtual void LaunchActiveSkill()
    { 
        
    }

    public void AddBuff(Buff buff)
    {
        if (buff2round.ContainsKey(buff) || buff2Number.ContainsKey(buff))
            return;
        buff2round.Add(buff, curRound);
        buff2Number.Add(buff, DamageCount);
    
    }

    public void DelBuff(Buff buff)
    {
        if (!buff2round.ContainsKey(buff) || !buff2Number.ContainsKey(buff))
        {
            Debug.LogError("试图删除没有的buff：" + buff);
            return;
        }
        buff2round.Remove(buff);
        buff2Number.Remove(buff);
    }

   

    /// <summary>
    /// buff2round是否有参数这个buff
    /// </summary>
    /// <param name="buff"></param>
    /// <returns></returns>
    public bool HasBuffInbuff2round(Buff buff)
    {
        if (buff2round.ContainsKey(buff)) return true;
        return false;
    }
    /// <summary>
    /// buffNumber是否有参数这个buff
    /// </summary>
    /// <param name="buff"></param>
    /// <returns></returns>
    public bool HasBuffInbuffNumber(Buff buff)
    {
        if (buff2Number.ContainsKey(buff)) return true;
        return false;
    }
    /// <summary>
    /// 获取buff开始时的回合数
    /// </summary>
    /// <param name="buff">buff类型</param>
    /// <returns>回合数</returns>
    public int GetBuffBeginRound(Buff buff)
    {
        if (buff2round.ContainsKey(buff))   return buff2round[buff];
        return -1;
    }

    /// <summary>
    /// 获取buff开始时的被击次数
    /// </summary>
    /// <param name="buff"></param>
    /// <returns></returns>
    public int GetBuffBeginNumber(Buff buff)
    {
        if (buff2Number.ContainsKey(buff)) return buff2Number[buff];
        return -1;
    }

    /// <summary>
    /// 获得buff持续的回合数
    /// </summary>
    /// <param name="buff"></param>
    /// <returns></returns>
    public int GetBuffContinueRound(Buff buff)
    {
        if (buff2ContinueRound.ContainsKey(buff)) return buff2ContinueRound[buff];
        return 0;
    }
    /// <summary>
    /// 获得buff持续的次数
    /// </summary>
    /// <param name="buff"></param>
    /// <returns></returns>
    public int GetBuffContinueNumber(Buff buff)
    {
        if (buff2ContinueNumber.ContainsKey(buff)) return buff2ContinueNumber[buff];
        return 0;
    }

    public void SetCurRoundNum(int num)
    {
        curRound = num;
        List<Buff> checkBuffList = new List<Buff>();
        foreach (Buff buff in buff2ContinueRound.Keys)
        {
            checkBuffList.Add(buff);
        }

        foreach (Buff onebuff in checkBuffList)
        {
            CheckBuff(onebuff);
        }
    }
    public void SetCurNumber()
    {
        List<Buff> checkBuffList = new List<Buff>();
        foreach (Buff buff in buff2ContinueNumber.Keys)
        {
            checkBuffList.Add(buff);
        }

        foreach (Buff onebuff in checkBuffList)
        {
            CheckBuff(onebuff);
        }
    }
    public void CheckBuff(Buff buff)
    {
        
        if (HasBuffInbuff2round(buff) && (curRound - GetBuffBeginRound(buff)) >= GetBuffContinueRound(buff))
        {
            DelBuff(buff);
        }
     
        if(HasBuffInbuffNumber(buff) && (DamageCount - GetBuffBeginNumber(buff)) >= GetBuffContinueNumber(buff))
        {
            DelBuff(buff);
        }
    }
    /// <summary>
    /// 是否能攻击
    /// </summary>
    /// <returns></returns>
    public bool CanAttack()
    {
        if (buff2round.ContainsKey(Buff.Terrify) || buff2round.ContainsKey(Buff.WindArrow))
        { 
            return false;
        }
        return true;
    }

    public void ClearAllBuff()
    {
        buff2round.Clear();
    }

}

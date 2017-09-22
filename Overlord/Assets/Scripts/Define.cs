public enum Status
{
    None,
    OnWay,  //路上
    WaitOutCity,    //城池排队等待队列
    Peace,          //和平状态
    ReachingBattlePos, //前往战斗站位
    ReachedBattlePos,  //到达战斗站位
    RoundIdle,      //回合待机中
    RoundAttacking      //回合攻击中
}


public enum CityStatus
{
    None,
    Peace,          //和平状态
    PrepareBattle,     //预备战斗状态，持续三秒进入战斗状态
    InRound,          //回合战斗状态
    RoundInterval,          //回合间隙
}

public enum Side
{
    Red,
    Blue,
    Neutral
}

/// <summary>
/// 攻击距离
/// </summary>
public enum AttackDistance
{
    None,
    Long,
    Short,
    Medium
}

public enum Buff
{
    /// <summary>
    /// 圣光术
    /// </summary>
    HolyLight,  
    /// <summary>
    /// 神圣护盾
    /// </summary>
    HolyShield, 
    /// <summary>
    /// 镇魂曲
    /// </summary>
    Requiem,
    /// <summary>
    /// 招魂曲
    /// </summary>
    SummonSoul, 
    /// <summary>
    /// 咆哮
    /// </summary>
    Roar,       
    /// <summary>
    /// 生命绽放
    /// </summary>
    LifeBloom,   
    /// <summary>
    /// 疾风箭
    /// </summary>
    WindArrow,   
    /// <summary>
    /// 眩晕
    /// </summary>
    Stun,   
    /// <summary>
    /// 欢迎
    /// </summary>
    Shadow,
    /// <summary>
    /// 复仇诅咒
    /// </summary>
    RevengeCurse,
    /// <summary>
    /// 放逐 
    /// </summary>
    Exile,       
    /// <summary>
    /// 暴风雪
    /// </summary>
    Blizard, 
    /// <summary>
    /// 嘲讽
    /// </summary>
    Taunt, 
    /// <summary>
    /// 嘲讽减伤
    /// </summary>
    TauntDefense,
    /// <summary>
    /// 盾墙
    /// </summary>
    ShieldWall,  
    /// <summary>
    /// 恐惧
    /// </summary>
    Terrify,  
}

/// <summary>
/// 子弹类型
/// </summary>
public enum BulletType
{
    none,
    /// <summary>
    /// 眩晕弹
    /// </summary>
    StunBullet,

}


//队列优先级
public enum Priority
{
	none,
	FrontMiddleBack,    //前中后
	MiddleFrontBack,    //中前后
	MiddleBackFront,    //中后前
	BackMiddleFront,    //后中前
}

//职业
public enum Occupation
{
	none,
	warriors ,//战士
	knight,//骑士
	poet,//诗人
	druid,//德鲁伊
	hunter,//猎人
	warlock,//术士
	seer,//先知
	mage,//法师
	pastor,//牧师
}

class GlobalDefineNum
{
    public const float ReachDistance = 0.7f;
}

public enum AUTH
{
    ERR = 6000,
    ALGORITHM_GENERATE_ERR = 6001,
    ALGORITHM_AUTH_ERR = 6002,
    TOKEN_EXPIRE = 6003,
    OPERATE_ERR = 6004,
    REGISTER_EXIST = 6005,
    LOGIN_UIDPWD_ERR = 6006,
    REGIST_ERR = 6007
}
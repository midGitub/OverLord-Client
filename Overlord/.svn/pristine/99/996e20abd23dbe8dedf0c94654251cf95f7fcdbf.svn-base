--PlayerBaseClass
--kyx
--2016/08/30


PlayerBaseClass = 
{
  playerId = 0,    --服务器用playerid
  userId = nil,    --玩家帐号
  serverId = nil,  --服务器id
  name = nil,      --昵称
  sex = 0,         --性别
  gold = 0,        --金币
  level = 0,       --玩家等级
  Exp = 0,         --玩家经验
  diamond = 0,     --钻石

};

PlayerBaseClass.__index = PlayerBaseClass;

--------------------------------------------------------
--构造函数
--------------------------------------------------------
function PlayerBaseClass:New(pid, uid) 

    local self = {}; 
    setmetatable(self, PlayerBaseClass);

    self.playerId = pid;
  
    self.userId = uid;

    self.serverId = 0;

    self.name = nil;
    self.sex = 0;
    self.gold = 0;
    self.level = 0;
    self.Exp = 0;    
    self.diamond = 0;

    return self; 
end

----------------------------------------
--public funs
----------------------------------------
function PlayerBaseClass:Init(_name, _gold, _level, _exp, dia)

    self.name = _name;

    self.gold = _gold;  

    self.level = _level;
    self.Exp = _exp;
    self.diamond = _dia;

end


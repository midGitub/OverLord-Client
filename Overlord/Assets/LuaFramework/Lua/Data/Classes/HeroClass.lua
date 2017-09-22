HeroClass = 
{
  heroId = 0,
  heroProfession = nil,
  heroLevel = 0,
  heroExp = 0, 

};

HeroClass.__index = HeroClass;

local that = {};
--------------------------------------------------------
--构造函数
--------------------------------------------------------
function HeroClass:New(_heroId, _heroExp, _heroLevel,_heroProfession ) 
    local self = {}; 
    setmetatable(self, HeroClass);
    self.heroId = _heroId; 
    self.heroProfession = _heroProfession;
    self.heroLevel = _heroLevel;
    self.heroExp = _heroExp;
 
    return self; 
end
---------------------------------------------------------------------
--public方法
---------------------------------------------------------------------


------------------------------------------------------------------------
--privite方法
------------------------------------------------------------------------
--获取英雄的基本信息数据
--function that.GetTankBaseInfo(tankId)
 -- local base = {};
--  base = TableReader["TankMetaData"][tankId];
----  return base;
--end
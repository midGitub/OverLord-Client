--heroCtrl
--yangguang
--2017/09/06

require 'FairyGUI'
require "Logic/GameGlobal"
require "View/heroPanel"

heroCtrl = {};
local this = heroCtrl;
local that = {};
------------------------------------------
--static funtions demanded by uipageclass
------------------------------------------
 --Constructor-- 
function this.New() 
  logWarn("HeroCtrl.New--->>");
	heroCtrl = GameGlobal.CreatePageInstance(this, UIPage.Hero);	
    this = heroCtrl;
	return this;
end

function this.Awake(view)
    this.InitPanel(view)
end 

function this.InitPanel(obj)
    this.mainView=obj
    this.Controller = obj:GetController("c1")
	this.backBtn = obj:GetChild("n7")
    this.heroList = obj:GetChild("heroList")

    this.npcBtn = this.heroList:GetChild("b0")
    this.npcBtn1 = this.heroList:GetChild("b1")
    this.npcBtn2 = this.heroList:GetChild("b2")
    this.npcBtn3 = this.heroList:GetChild("b3")
    this.npcBtn4 = this.heroList:GetChild("b4")
    this.npcBtn5 = this.heroList:GetChild("b5")
    this.npcBtn6 = this.heroList:GetChild("b6")
    this.npcBtn7 = this.heroList:GetChild("b7")
    this.npcBtn8 = this.heroList:GetChild("b8")
    this.npcBtn9 = this.heroList:GetChild("b9")
    this.npcBtn10 = this.heroList:GetChild("b10")
    this.npcBtn11 = this.heroList:GetChild("b11")

    local infolist = obj:GetChild("heroInfoList")

    local bloodbtn = infolist:GetChild("blood")
    this.bloodText = bloodbtn:GetChild("title")
    
    local manabtn = infolist:GetChild("mana")
    this.manaText = manabtn:GetChild("title")

    local energybtn = infolist:GetChild("energy")
    this.energyText = energybtn:GetChild("title")

    local speedbtn = infolist:GetChild("speed")
    this.speedText = speedbtn:GetChild("title")

    local damagebtn = infolist:GetChild("damage")
    this.damageText = damagebtn:GetChild("title")

    this.behaviour:AddClick(this.backBtn,this.BackGame)

    this.behaviour: ListItemAddClick(this.heroList,this.ItemOnclick)

  --  UpdateBeat:Add(ShowSkill, self)
    InitHeroList()
end  

function InitHeroList()
    SetInfoText(this.bloodText,"  气血：0")
    SetInfoText(this.manaText,"  魔法：0")
    SetInfoText(this.speedText,"  速度：0")
    SetInfoText(this.damageText,"  伤害：0")
    SetInfoText(this.energyText,"  能量：0")
end

--Active this page
function this.Active()
    -- this.gameObject:SetActive(true);
    this.isActived = true;
    this.isPassiveOpen = false;
end

--Only Deactive UI wont clear Data.
function this.Hide()
     --this.gameObject:SetActive(false);
     this.isActived = false;  
    this.m_data = nil; --set this page's data null when hide.
    this.isPassiveClose = false;
end

--Show UI Refresh Eachtime.
function this.Refresh() 
end

function SetInfoText(obj,txt)
    obj.text = txt
end

--点击事件
function this.BackGame()
    if this.Controller.selectedIndex==0
    then 
    mainCtrl.mainView.visible = true
    this.mainView.visible = false
    else if this.Controller.selectedIndex==1
    then this.Controller.selectedIndex=0
    end
    end
end

function  this.ItemOnclick(item)
    this.Controller.selectedIndex=1
    logWarn(item.name)
    print("&&&&&&&&&&-------------展示英雄-------------&&&&&&&&&&")
    UseHero1.PlayHero()
end

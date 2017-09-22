
require "System/UIPageClass"
require "System/Utility"
require "Data/Classes/HeroClass"
--GameGlobal 全局保存数据

GameGlobal = {};
local this = GameGlobal;

GameGlobal.GameStarted = false;
GameGlobal.LoadedLevel = 0; 			--刚进入的场景

GameGlobal.token = {}; 					--token
GameGlobal.socketIP = '';      --长连接ip
GameGlobal.socketPort = 0;     --长连接端口

this.TipTimer=true
this.TipAnmList={}
this.TipAnmList.up={}
this.TipAnmList.lock={}

GameGlobal.clickonce = true
--用户
this.playerBase = nil;  --玩家基本信息

--Hero列表
this.Herolist = nil;
this.Hero = nil;
this.tankWar = 0;
this.ShipEquip={}
this.OnTankId=0

this.Champion = nil;

--背包
this.Item={}
this.BagEquips={}

--战斗
this.warTanks = nil;
--this.myId = 0;

--错误弹板
function this.PopError(str)
	TipCtrl.ShowTip(str)
end

this.ShowError={
	
}

--UIroot tranforms
this.UIRoot = 
{
	Fixed = nil,
	Normal = nil,
	Popup = nil,
	CrossScene = nil,
}

--set a uipage's transform
function this.SetUITransform(trans, UIType)
	if UIType == 0 then
		trans:SetParent(this.UIRoot.Normal);
		GameG.SetLayer(trans, "2DUI");
	elseif UIType == 1 then
		trans:SetParent(this.UIRoot.Fixed);
		GameG.SetLayer(trans, "2DUI");
	elseif UIType == 2 then
		trans:SetParent(this.UIRoot.Popup);
		GameG.SetLayer(trans, "2DUI");
	elseif UIType == 3 then
		return;
	elseif UIType == 7 then
		trans:SetParent(this.UIRoot.CrossScene);
		GameG.SetLayer(trans, "2DE");
	else
		logWarn("CtrlManager:: the para UIType error");
		return;
	end
    trans.localEulerAngles = Vector3.New(0, 0, 0);
    trans.localScale = Vector3.New(1, 1, 1);
    trans.localPosition = Vector3.New(0, 0, 0);
end

--create uipage instance
function this.CreatePageInstance(table2, paras)
   logWarn("GameGlobal.CreatePageInstance--->>");
	local table1 = UIPageClass.NewInstance():Init(paras.id, paras.CtrlName, paras.UIType, paras.UIMode, paras.UICollider, paras.BundleName, paras.isUsePool, paras.isE);
	table.merge(table1, table2);
	return table1;
end
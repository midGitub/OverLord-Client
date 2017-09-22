--mainCtrl
--yangguang
--2017/08/30

require 'FairyGUI'
require "Logic/GameGlobal"
require "View/mainPanel"

require "Controller/loadingCtrl"
require "Controller/heroCtrl"

mainCtrl = {};
local this = mainCtrl;
local that = {};

heroFirst = true
------------------------------------------
--static funtions demanded by uipageclass
------------------------------------------
 --Constructor-- 
function this.New() 
  logWarn("MainCtrl.New--->>");
	mainCtrl = GameGlobal.CreatePageInstance(this, UIPage.Main);	
    this = mainCtrl;
	return this;
end

function this.Awake(view)
    this.InitPanel(view)
end 

function this.InitPanel(obj)
    this.mainView=obj
	this.startBtn = obj:GetChild("n2")
    this.windowBtn = obj:GetChild("n3")
    this.heroBtn = obj:GetChild("n11")

    this.behaviour:AddClick(this.startBtn,this.StartGame)
    this.behaviour:AddClick(this.windowBtn,this.OpenWindow)
    this.behaviour:AddClick(this.heroBtn,this.OpenHero)
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

--点击事件
function this.StartGame()
    local senddata = {n = {1,1}};
	MsgFunc.SocketRequest(senddata,30)
    
end

function  this.OpenWindow()  
    UseWindow1.PlayWindow()
end

function this.OpenHero()
    this.mainView.visible = false
    --加载英雄选择界面
    if heroFirst==true
    then
    heroFirst=false
    UIPageClass.ShowPage(UIPage.Hero.CtrlName, CtrlManager.GetCtrl(UIPage.Hero.CtrlName))
    else heroCtrl.mainView.visible = true
    end
end
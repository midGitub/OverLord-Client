--loadingCtrl
--yangguang
--2017/08/30

require 'FairyGUI'
require "Logic/GameGlobal"
require "View/loadingPanel"
require "System/UIPageClass"

loadingCtrl = {};
local this = loadingCtrl;
local that = {};

sceneManager=UnityEngine.SceneManagement.SceneManager
scene=UnityEngine.SceneManagement.Scene
------------------------------------------
--static funtions demanded by uipageclass
------------------------------------------
 --Constructor-- 
function this.New()
  logWarn("LoadingCtrl.New--->>");
	loadingCtrl = GameGlobal.CreatePageInstance(this, UIPage.Loading);
    this = loadingCtrl;
	return this;
end

function this.Awake(view)
    this.mainView = view
    this.InitPanel(view);

    --载入login
    if GameTimes.first==true
    then
    UIPageClass.ShowPage(UIPage.Login.CtrlName, CtrlManager.GetCtrl(UIPage.Login.CtrlName))
    UIPageClass.ShowPage(UIPage.Register.CtrlName,CtrlManager.GetCtrl(UIPage.Register.CtrlName))
    end
end 

function this.InitPanel(obj)
	this.ProgressBar = obj:GetChild("n1")
	this.ProgressText = obj:GetChild("n2")

    this.ProgressBar.value=0
    this.ProgressText.text="正在载入游戏……"
    UpdateBeat:Add(LoadingUpdate, self)
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

function this.LoadingScene()
    --加载fight场景
    sceneManager.LoadScene("roundfight")
    GameTimes.first=false
    UIPageClass.ShowPage(UIPage.Loading.CtrlName, CtrlManager.GetCtrl(UIPage.Loading.CtrlName))
end

--每帧执行
function LoadingUpdate()
    if this.ProgressBar.value<this.ProgressBar.max
    then this.ProgressBar.value=this.ProgressBar.value+1

    else 
    this.mainView:Dispose()
    --this.mainView.visible = false
    --else if (sceneManager.GetActiveScene().name == "fgui")
    --then this.mainView.visible = false
    --end
    end
end
--loginCtrl
--nzp
--2017/08/30

require 'FairyGUI'
require "Logic/GameGlobal"
require "Logic/MsgFunc"
require "View/loginPanel"
require "Common/protocal"
require "Common/define"
loginCtrl = {};
local this = loginCtrl;
local that = {};

local json = require "cjson" --加入json解析
------------------------------------------
--static funtions demanded by uipageclass
------------------------------------------
 --Constructor-- 
function this.New() 
  logWarn("LoginCtrl.New--->>");
	loginCtrl = GameGlobal.CreatePageInstance(this, UIPage.Login);
  
  this = loginCtrl;

	return this;
end

function this.Awake(view)
	this.view = view
  this.InitPanel(view)
	
end 

function this.InitPanel(obj)
  
	this.RegBtn = obj:GetChild("regBtn")
  this.behaviour:AddClick(this.RegBtn,this.RegOnClick);
  
	this.LogBtn = obj:GetChild("loginBtn")
  this.behaviour:AddClick(this.LogBtn, this.LoginOnClick);
  
  this.testBtn = obj:GetChild("testBtn")
  this.behaviour:AddClick(this.testBtn, this.testOnclick);
  
	this.labelName = obj:GetChild("idText")
	this.labelPass = obj:GetChild("pwText") 
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
function this.Refresh() end

function this.LoginOnClick()
  
	local user = this.labelName.text;
	local pwd = this.labelPass.text;
	logWarn('LoginCtrl::Click LogIn'..' UserName：'..user..' Pwd：'..pwd); 
	if	user=='' then
		
		return
	end
	
	if	pwd=='' then
		
		return
	end
	MsgFunc.senddata[10] = {user,pwd}
	NetManager:ConnectHttp(NetMsg[10], 0, MsgFunc.senddata[10][1], MsgFunc.senddata[10][2], 1);
  
  logWarn("LoginOnClick----------->")
end

function  this.RegOnClick()
  registerCtrl.view.visible = true
  loginCtrl.view.visible = false
  
end

function this.testOnclick()
  local sceneManager=UnityEngine.SceneManagement.SceneManager
  sceneManager.LoadScene("roundfight")
end

--registerCtrl
--nzp
--2017/08/30

require 'FairyGUI'
require "Logic/GameGlobal"
require "View/registerPanel"

registerCtrl = {};
local this = registerCtrl;
local that = {};
------------------------------------------
--static funtions demanded by uipageclass
------------------------------------------
 --Constructor-- 
function this.New() 
  logWarn("registerCtrl.New--->>");
	registerCtrl = GameGlobal.CreatePageInstance(this, UIPage.Register);
	
  this = registerCtrl;

	return this;
end

function this.Awake(view)
	this.view = view
  this.InitPanel(view)
	this.view.visible = false
end 

function this.InitPanel(obj)
  
	this.RegBtn = obj:GetChild("regBtn")
  this.behaviour:AddClick(this.RegBtn,this.RegOnClick);
  
  this.returnBtn = obj:GetChild("returnBtn")
  this.behaviour:AddClick(this.returnBtn,this.returnOnClick);
  
	this.labelName = obj:GetChild("idText")
	this.labelPass = obj:GetChild("pwText") 
  this.labelPass2 = obj:GetChild("pwConfirmText") 
end  

function this.returnOnClick()
  logWarn("111111111111111111111")
  loginCtrl.view.visible = true
  this.view.visible = false
end

function this.RegOnClick()
	local user = this.labelName.text;
	local pwd
	if	this.labelPass.text == this.labelPass2.text then
		pwd = this.labelPass.text;
	else
		
		return
	end
	if	pwd=='' then
    
		return
	end
	print('LoginCtrl::Click Register'..' UserName：'..user..' Pwd：'..pwd); 
	MsgFunc.senddata[10] ={user,pwd};
	NetManager:ConnectHttp(NetMsg[10], 0, MsgFunc.senddata[10][1], MsgFunc.senddata[10][2], 2);
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

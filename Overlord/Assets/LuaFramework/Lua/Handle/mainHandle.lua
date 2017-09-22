--mainHandle
--nzp
--2017/09/21

local list = nil;
local exp = 0;
local HeroExp = 0;
local json = require "cjson" --加入json解析

------------------------------------------------
--30进入房间返回消息处理
-----------------------------------------------
--{"id":30,"n":[200,1],"data":[{"n":[1,2500,2500,27000,100,1000001]},{"n":[2,0,2500,27000,100,1000001]},{"n":[3,-2500,2500,27000,100,1000001]},{"n":[4,2500,-2500,9000,100,1000001]},{"n":[5,0,-2500,9000,100,1000001]},{"n":[6,-2500,-2500,9000,100,1000001]}]}

MsgFunc.ErrHandle[30] = function (errCode)
	--GameGlobal.ShowError[errCode]()
end

MsgFunc.SaveData[30] = function(obj)

end 

MsgFunc.HandleData[30] = function()
  --  loadingCtrl.LoadingScene()
   -- mainCtrl.mainView.visible = false   

end

------------------------------------------------
--31离开房间返回消息处理
-----------------------------------------------
MsgFunc.ErrHandle[31] = function (errCode)
	--GameGlobal.ShowError[errCode]()
end

MsgFunc.SaveData[31] = function(obj)

end 

MsgFunc.HandleData[31] = function()
	--UIPageClass.ShowPage(UIPage.Settlement.CtrlName, CtrlManager.GetCtrl(UIPage.Settlement.CtrlName),{BattlePanel.js.text,BattlePanel.sw.text,0,BattlePanel.timeshow.text,0,tostring(tonumber(BattlePanel.js.text) * 10),0});						--[3011]
    --SceneManager.LoadLevel(1)--进入主城  CROSS_PLATFORM_INPUT;MOBILE_INPUT

end

----------------------------------------------


------------------------------------------------
MsgFunc.ErrHandle[1012] = function (errCode)
	--GameGlobal.ShowError[errCode]()
end

MsgFunc.SaveData[1012] = function(obj)

end

MsgFunc.HandleData[1012] = function()

end
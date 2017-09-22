--LoginHandle
--nzp
--2017/09/01
local json = require "cjson"
local WWW = UnityEngine.WWW;
local WWWForm = UnityEngine.WWWForm;

---------------------------------------------------------------
--10登录注册返回消息处理
---------------------------------------------------------------
local uid;
local url;
local token;
--{"id":10,"n":[200,28,4010],"s":["e2334cc5c44188a64178ea169e9aceca168f1a5b417af608f010327d9addc4a3","192.168.1.131"]}
MsgFunc.ErrHandle[10] = function (errCode)
	GameGlobal.ShowError[errCode]()
  logWarn(errCode)
end

MsgFunc.SaveData[10] = function(obj)
	token = obj['s'][1];
	uid = obj['n'][2];
	url = 'http://'.. obj['s'][2] .. ':' .. obj['n'][3] .. '/';	
end


MsgFunc.HandleData[10] = function()
	
	local uurl = url .. NetMsg[11];
	logWarn(uurl);
	local form = WWWForm();
  	form:AddField('id', 1);
  	form:AddField('uid', uid);    
  	local www = WWW(uurl,form);
  	coroutine.www(www);
      
    if www.isDone then
		logWarn('HandleData[10]   Lua Network:'..www.text);
	
		local obj0 = json.decode(www.text);
		obj0 = obj0['body']; 
		log(json.encode(obj0))
		Network.OnSocket("11", json.encode(obj0));
  
	end	
	
	UIPageClass.ShowPage(UIPage.Main.CtrlName, CtrlManager.GetCtrl(UIPage.Main.CtrlName));
	loginCtrl.view.visible = false
	registerCtrl.view.visible = false

end


------------------------------------------------------------------------
----11进入长连接服务器返回消息处理
------------------------------------------------------------------------
--{"id":11,"s":["192.168.1.131"],"n":[200,4011]}
MsgFunc.ErrHandle[11] = function (errCode)
	GameGlobal.ShowError[errCode]()
	 --错误消息处理
end

MsgFunc.SaveData[11] = function(obj)
	GameGlobal.socketIP = obj['s'][1];      --长连接ip
	GameGlobal.socketPort = obj['n'][2];     --长连接端口

end

MsgFunc.HandleData[11] = function()
	NetManager:ConnectSocket(GameGlobal.socketIP, GameGlobal.socketPort, token, NetMsg[12]);
end

-------------------------------------------------------
--12验证登陆返回消息处理
-------------------------------------------------------
--{"id":12,"n":[200,28]}
MsgFunc.ErrHandle[12] = function (errCode)
	GameGlobal.ShowError[errCode]()
end

MsgFunc.SaveData[12] = function(obj)
	
	GameGlobal.playerBase = PlayerBaseClass:New(obj['n'][2], MsgFunc.senddata[10][1])

	GameGlobal['msg12']=json.encode(obj)
end

MsgFunc.HandleData[12] = function()
	
	NetManager:Listen('onTick');
	NetManager:Listen('onOperate');
	NetManager:Listen('onFire');
	NetManager:Listen('onSomeoneEnter');
	NetManager:Listen('onSomeoneExit');
	NetManager:Listen('onDamage');
	NetManager:Listen('onEnterRoom');
	NetManager:Listen('onSomeoneResult');
	NetManager:Listen('onCanStart');
	NetManager:Listen('onChangeAttackArea');
	NetManager:Listen('onChangeSpeed');
	
	if GameGlobal.playerBase.playerId == 0 then
		
		local senddata = {name = 'Charles', sex = 0};
		MsgFunc.SocketRequest(senddata,13)
	else
		local senddata = {id = 13, n = {200, GameGlobal.playerBase.playerId}};
		senddata = json.encode(senddata);
		Network.OnSocket("13", senddata);      
	end	

end

----------------------------------------------------
--13注册玩家返回消息处理
----------------------------------------------------
--{"n":[200,28],"id":13}
MsgFunc.ErrHandle[13] = function (errCode)
	GameGlobal.ShowError[errCode]()
end

MsgFunc.SaveData[13] = function(obj)
    GameGlobal.playerBase.playerId = obj['n'][2];
end

MsgFunc.HandleData[13] = function()
	local senddata = {};
	MsgFunc.SocketRequest(senddata,20)
	
end

------------------------------------------------------
--20进入主城，信息初始化 返回消息处理
-------------------------------------------------------
--{"id":20,"n":[200]}
MsgFunc.ErrHandle[20] = function (errCode)
	GameGlobal.ShowError[errCode]()
end

MsgFunc.SaveData[20] = function(obj)

end

MsgFunc.HandleData[20] = function()
	local senddata = {};
	
	MsgFunc.SocketRequest(senddata, 21)

	-- 进入游戏购买两艘船(暂定为起始3艘船解决方案，后期需要服务器修改登录消息)
	-- MsgFunc.senddata[51] = {['n']={1000000002}}--tankId
	-- MsgFunc.SocketRequest(MsgFunc.senddata[51], 51)
	-- MsgFunc.senddata[51] = {['n']={1000000003}}--tankId
	-- MsgFunc.SocketRequest(MsgFunc.senddata[51], 51)
end

-------------------------------------------------
--21获取玩家所有数据处理
-------------------------------------------------
--[[
{"n":[200,10000,1000000,1,0],
"s":["Charles"],
"navigators":
[
	{"n":[10001,0,1,1]},
	{"n":[10002,0,1,2]},
	{"n":[10003,0,1,3]},
	{"n":[10004,0,1,4]},
	{"n":[10005,0,1,5]},
	{"n":[10006,0,1,6]},
	{"n":[10007,0,1,7]},
	{"n":[10008,0,1,8]},
	{"n":[10009,0,1,9]}
],
"id":21}
	]]--
MsgFunc.ErrHandle[21]=function(errCode)
  --常规错误码：1000(参数不全);1001(内部代码错误);4001(玩家不存在)处理
   GameGlobal.ShowError[errCode]()
end

MsgFunc.SaveData[21]=function(obj)
	print("===============getalldata savedata========================")
	--玩家
	GameGlobal.playerBase:Init(obj["s"], obj["n"][2], obj["n"][4], obj["n"][5], obj["n"][3]);
	--英雄
	GameGlobal.Herolist= {}
	local tankcount = #obj["navigators"]
	for i = 1, tankcount do
		
		local _hero = obj["navigators"][i]
		
		local champion = HeroClass:New(_hero["n"][1],_hero["n"][2],_hero["n"][3],_hero["n"][4])
		
		table.insert(GameGlobal.Herolist,champion)
	end
--[[	--坦克
	GameGlobal.tankList = {};
	GameGlobal.Tanks = {};
	GameGlobal.tankWar = nil;
	local tankdata = obj["tanks"]['na'];
	local tankcount = #tankdata;
	
	local _tanklistCount = #GameGlobal.tankList;
	for i = 1, tankcount do 
		local _tank = tankdata[i];
		local tid = _tank["n"][1];
		if nil == GameGlobal.Tanks[tid] then 
			_tanklistCount = _tanklistCount + 1;
			GameGlobal.tankList[_tanklistCount] = tid;
			--GameGlobal.Tanks[tid] = TankClass:New(tid, _tank["n"][2], _tank["n"][3]);
			GameGlobal.Tanks[tid] = TankClass:New(tid, _tank["n"][2], _tank["n"][3], _tank["n"][4]);
			tid=tid+_tank["n"][2]-1
			print('登录返回消息船只升级信息ID'..tid)
			GameGlobal.ShipEquip[tid]={}

			--修改服务器后
			local Info=TableReader.AllTankData[tid].HavePosition
			local splitlist = Split(Info, "#")            --字符串分割
			for i=1,#splitlist do
				local Splitlist=Split(splitlist[i], ":")
				local posid = tonumber(Splitlist[1])
				local maxequip = tonumber(Splitlist[2])
				local gold = tonumber(Splitlist[3])
				local itemid = tonumber(Splitlist[4])
				GameGlobal.ShipEquip[tid][posid]={}
				GameGlobal.ShipEquip[tid][posid].itemid=itemid
				GameGlobal.ShipEquip[tid][posid].maxequip=maxequip
				GameGlobal.ShipEquip[tid][posid].gold=gold
				for j=1,#_tank['na'] do 
					if _tank['na'][j][1] == posid then
						GameGlobal.ShipEquip[tid][posid].ishave=_tank['na'][j][2]
						GameGlobal.ShipEquip[tid][posid].equipd=_tank['na'][j][3]
						if(_tank['na'][j][3]==maxequip) then 
							GameGlobal.ShipEquip[tid][posid].ismax=1
						else
							GameGlobal.ShipEquip[tid][posid].ismax=0
						end
						break
					end
				end
			end
		end
	end]]--
--	GameGlobal.OnTankId=obj["tanks"]['n'][1]
--	GameGlobal.tankWar = GameGlobal.Tanks[GameGlobal.tankList[1]] ;
--	local bagdata = obj['itemBag']['na']
--	local bagcount = #bagdata
--	print('bagcount='..bagcount)
--	for	i=1, bagcount do
--		print('bagdata[i][1]='..bagdata[i][1])
--		print('bagdata[i][2]='..bagdata[i][2])
--		GameGlobal.Item[bagdata[i][1]]={}
--		GameGlobal.Item[bagdata[i][1]].number = bagdata[i][2]
--	end
	--测试道具数据(后期移到登录信息处理事件)
	-- for i in pairs(TableReader.Item) do
		-- GameGlobal.Item[i]={}
		-- GameGlobal.Item[i].number=200
	-- end
	--print(GameGlobal.tankWar.tankId .. " ==== " .. GameGlobal.tankWar.tankLevel .. " === " .. GameGlobal.tankWar.tankBase.sPreName)]]--
end

MsgFunc.HandleData[21]=function()
	local senddata = {};
	MsgFunc.SocketRequest(senddata, 24);
	SceneManager.LoadLevel(1)--进入主城
	print("读取信息完成，跳转主城场景zzzzzzzzzzzzzzz")
end

function Split(szFullString, szSeparator)    
	local nFindStartIndex = 1    
	local nSplitIndex = 1    
	local nSplitArray = {}    
	while true do    
	   local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)    
	   if not nFindLastIndex then    
		nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))    
		break    
	   end    
	   nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1)    
	   nFindStartIndex = nFindLastIndex + string.len(szSeparator)    
	   nSplitIndex = nSplitIndex + 1    
	end    
	return nSplitArray    
end  
---------------------------------------------
--接受初始推送消息
---------------------------------------------
MsgFunc.ErrHandle[24] = function (errCode)
	GameGlobal.ShowError[errCode]()
end

MsgFunc.SaveData[24] = function(obj)
	print("-=--=---------------------------")
end

MsgFunc.HandleData[24] = function()
	
end



-------------------------------------------------------
--断线重连消息23
-------------------------------------------------------
MsgFunc.ErrHandle[23]=function (errCode)
	if 4003 == errCode then
		NetManager:ConnectHttp(NetMsg[10], 0, MsgFunc.senddata[10][1], MsgFunc.senddata[10][2], 1);
	end
end

MsgFunc.SaveData[23]=function(obj)
	MsgFunc.SaveData[21](obj)
end

MsgFunc.HandleData[23]=function()
	NetManager:Listen('onTick');
	NetManager:Listen('onOperate');
	NetManager:Listen('onFire');
	NetManager:Listen('onSomeoneEnter');
	NetManager:Listen('onSomeoneExit');
	NetManager:Listen('onDamage');
	NetManager:Listen('onEnterRoom');
	NetManager:Listen('onSomeoneResult');
	NetManager:Listen('onCanStart');
	NetManager:Listen('onChangeAttackArea');
	NetManager:Listen('onChangeSpeed');
	--NetManager:Listen('onChangeAttackArea');
	--NetManager:Listen()
	
	local senddata = {};
	MsgFunc.SocketRequest(senddata, 24);
	for k,v in pairs(UIPage) do 
		local ctrl = CtrlManager.GetCtrl(v.CtrlName);	
		if ctrl.isActived then
			print("=------===============================Refresh  " .. ctrl.name)
			ctrl.Refresh();
		end
		
	end
end



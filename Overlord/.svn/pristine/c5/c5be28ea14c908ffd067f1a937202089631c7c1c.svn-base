--Network 网络消息处理

local Event = require 'events'
local json = require "cjson" --加入json解析
require "Logic/MsgFunc"

Network = {};
local this = Network;
local that = {};

function Network.Start() 
    logWarn("Network.Start!!----->");
    this.LoadEvent();
    
end

--增加网络监听
function Network.LoadEvent()	
	 for	k,v in pairs(NetMsg) do
    
	 	Event.AddListener(tostring(k),this.MsgHandle)
    
  end
end

--卸载网络监听--
function Network.Unload() 
  for	k,v in pairs(NetMsg) do
	 	Event.RemoveListener(tostring(k)) 	
	 end
  logWarn('Unload Network...');
end

--Socket消息--
function Network.OnSocket(key, data)
  
  Event.Brocast(tostring(key), data);
end

function Network.MsgHandle(msg)

	logWarn('LUA Network Received.MsgHandle:'..msg)

	obj = json.decode(msg)

	local id = obj['id']
	if id >= 1000 or obj['n'][1] == 200 then --正常消息或无code

		MsgFunc.SaveData[id](obj);
		MsgFunc.HandleData[id]();  
    logWarn("Network.MsgHandle---正常---------->"..id)
	else
		MsgFunc.ErrHandle[id](obj['n'][2]);
	end
	--TipCtrl.HideLoading()
end

local netState = 0;
--网络状态改变回调 2016/03/02 kyx 增加
function Network.NetworkStateChangeCallBack(state)
	netState = state;
  log('Lua Network' .. 'NetworkStateChangeCallBack' ..state);
	
end

function this.OnUpdate()
	if netState == 3 or netState == 4 or netState == 5 then --3掉线--4超时--5错误
		print("----3---------------334433------------------")

		TipCtrl.ShowReLog(that.Reconnect)
		netState = 0; 	
	end	
end

function that.Reconnect()
	-- TipCtrl.ShowLoading()
  NetManager:ReconnectSocket(GameGlobal.socketIP, GameGlobal.socketPort, GameGlobal.playerBase.playerId, NetMsg[23])
end
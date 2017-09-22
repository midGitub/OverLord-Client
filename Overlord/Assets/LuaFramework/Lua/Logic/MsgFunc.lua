--MsgFunc

require "Common/define"  
require "Common/functions"
require "Logic/GameGlobal"
require "Common/protocal"
require "Logic/PublicFunc"
require "Data/TableReader"
require	"Logic/Bit"

local json = require "cjson" --加入json解析


MsgFunc = {}

function MsgFunc.SocketRequest(senddata,id)
	--TipCtrl.ShowLoading()
	senddata = json.encode(senddata)
	log('Lua SendData:'..senddata)
    NetManager:SocketRequest(senddata, NetMsg[id])
end

MsgFunc.SaveData = {};
MsgFunc.ErrHandle = {};
MsgFunc.HandleData = {};
MsgFunc.senddata = {}; --发送的消息的table

require "Handle/loginHandle" 
require "Handle/mainHandle"

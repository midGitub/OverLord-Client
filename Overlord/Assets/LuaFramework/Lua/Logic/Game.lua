--require "Logic/SceneManager"


local lpeg = require "lpeg"

local json = require "cjson"
local util = require "3rd/cjson/util"

local sproto = require "3rd/sproto/sproto"
local core = require "sproto.core"
local print_r = require "3rd/sproto/print_r"

require "Logic/LuaClass"
require "Logic/CtrlManager"
require "Common/functions"
require "Controller/PromptCtrl"
require "Logic/SceneManager"


local lpeg = require "lpeg"

local json = require "cjson"
local util = require "3rd/cjson/util"

local sproto = require "3rd/sproto/sproto"
local core = require "sproto.core"
local print_r = require "3rd/sproto/print_r"

require "Logic/LuaClass"
require "Logic/CtrlManager"
require "Common/functions"
require "Controller/PromptCtrl"

--管理器--
Game = {};
local this = Game;

--初始化完成，发送链接服务器信息--
function Game.OnInitOK()
	--LuaFramework.AppConst.SocketPort = 3011;
	--LuaFramework.AppConst.SocketAddress = "192.168.1.187";--海盗法典的服务器
	LuaFramework.AppConst.SocketPort = 3013;
	LuaFramework.AppConst.SocketAddress = "192.168.1.235";--海盗法典的服务器
--注册LuaView--
  
    CtrlManager.Init();
    SceneManager.GameStartUp();
	this.LoadUpdate();
	
	--TableReader.SaveToLuaTable()
    TableReader.InitTables();
    --name = TableReader["championBase"][1001]["nSkill"];
    --print("haojingbin"..name);
end


local shipTip=false
function Game.OnUpdate()
  --print("lua:GameManager.OnUpdate");
  --TODO 此处加入需要update的面板的方法
end


--销毁--
function Game.OnDestroy()
	--logWarn('OnDestroy--->>>');
end

--加载Update
function Game.LoadUpdate()
    UpdateBeat:Add(this.OnUpdate, this);
end
--卸载Update
function Game.UnloadUpdate()
    UpdateBeat:Remove(this.OnUpdate, this);
end

function Game.OnLevelWasLoaded(index)
	--SceneManager.OnSceneLoaded(index);
end
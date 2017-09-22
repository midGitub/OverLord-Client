
CtrlNames = {
	Prompt = "PromptCtrl",
	Message = "MessageCtrl"
}

PanelNames = {
	"PromptPanel",	
	"MessagePanel",
}


--[[ PS:
UIType  
	0, -- Normal
 	1, -- Fixed
	2, -- PopUp
	3, -- None     Standalone Window
    
UIMode 
	0, -- DoNothing,
	1, -- HideOther,     Close Other windows
 	2, -- NeedBack,      Click Back Btn to Close Cur Window, But Not Close Other Window. Added to backSequence
 	3, -- NoNeedBack,    Close TopBar, Close other Windows, Not Added backSequence

UICollider 
	0, -- None,      No Background without collider
	1, -- Normal,    Trans background with collider
	2, -- WithBg,    opaque background with collider ]]

UIPage =
{
	Loading = {id = 0,  CtrlName = "LoadingCtrl", PanelName = "LoadingPanel", UIType = 0, UIMode = 3, UICollider = 2, BundleName = "loading", isUsePool = false, isE = false},
	Login = {id = 1,  CtrlName = "loginCtrl", PanelName = "loginPanel", UIType = 0, UIMode = 3, UICollider = 2, BundleName = "login", isUsePool = false, isE = false},
  	Register = {id = 2,  CtrlName = "registerCtrl", PanelName = "registerPanel", UIType = 0, UIMode = 3, UICollider = 2, BundleName = "register", isUsePool = false, isE = false},
  	Main = {id = 3,  CtrlName = "mainCtrl", PanelName = "mainPanel", UIType = 0, UIMode = 3, UICollider = 2, BundleName = "main", isUsePool = false, isE = false},
	Hero = {id = 4,  CtrlName = "heroCtrl", PanelName = "heroPanel", UIType = 0, UIMode = 3, UICollider = 2, BundleName = "hero", isUsePool = false, isE = false},
}

TableName =
{
	"championBase",
	"skillBase",
}

--协议类型--
ProtocalType = {
	BINARY = 0,
	PB_LUA = 1,
	PBC = 2,
	SPROTO = 3,
}
--当前使用的协议类型--
TestProtoType = ProtocalType.BINARY;

Util = LuaFramework.Util;
AppConst = LuaFramework.AppConst;
LuaHelper = LuaFramework.LuaHelper;
ByteBuffer = LuaFramework.ByteBuffer;

resMgr = LuaHelper.GetResManager();
panelMgr = LuaHelper.GetPanelManager();
soundMgr = LuaHelper.GetSoundManager();
NetManager = LuaHelper.GetNetManager();

WWW = UnityEngine.WWW;
GameObject = UnityEngine.GameObject;
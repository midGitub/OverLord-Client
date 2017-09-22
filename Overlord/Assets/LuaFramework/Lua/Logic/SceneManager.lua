--SceneManager
--kyx
--2016/08/03
require "Logic/CtrlManager"
require "System/UIPageClass"
require "Common/define"
--require "Logic/BuildingInfo"

SceneManager = {};
local this = SceneManager;
local that = {};
local LoadLevelNumber = 0;

local AwakeCtrl = {
	[0] = function()
			UIPageClass.ShowPage(UIPage.Login.CtrlName, CtrlManager.GetCtrl(UIPage.Login.CtrlName)); 
    	end,
	[1] = function()
			UIPageClass.ShowPage(UIPage.Register.CtrlName, CtrlManager.GetCtrl(UIPage.Register.CtrlName)); 
    	end,
	[2] = function()
			UIPageClass.ShowPage(UIPage.Loading.CtrlName, CtrlManager.GetCtrl(UIPage.Loading.CtrlName)); 
    	end,
  }

  local OnLevelLoadOverShowChoose = 
  {
	  	[0] = function()
				UIPageClass.ShowPage(UIPage.Login.CtrlName, CtrlManager.GetCtrl(UIPage.Login.CtrlName));   			
        end,
      [1] = function()
				UIPageClass.ShowPage(UIPage.Register.CtrlName, CtrlManager.GetCtrl(UIPage.Register.CtrlName));   			
    		  end,
		
  }
---------------------------------------------
--Static Functions
---------------------------------------------
function this.GameStartUp()
    
--    that.SetUIRoot();
	
		AwakeCtrl[2]();	
	 
end



function this.OnLevelLoadOverShow()
	print("     LoadLevelNumber  is             "..tostring(LoadLevelNumber));
	OnLevelLoadOverShowChoose[LoadLevelNumber]();
end

function this.OnSceneLoaded(index)
    GameGlobal.LoadedLevel = index; 
	--UIPageClass.ClearAllPages();
	that.SetUIRoot();
	print("============================================切换场景===========================================")
	LoadingCtrl.AddMax(20);
	LoadingCtrl.SetText("加载资源")
 
	AwakeCtrl[GameGlobal.LoadedLevel](); 
end

--切换场景 
function this.LoadLevel(index)
	LoadLevelNumber = index;
	AnimationCtrl.ClearAnimations();
	UIPageClass.ClearAllPages();		
 	UIPageClass.ShowPage(UIPage.Loading.CtrlName, CtrlManager.GetCtrl(UIPage.Loading.CtrlName), {30, 2, "初始化资源", index}); 
	Application.LoadLevel (index);
	ResManager:UnloadAllAssetBundle();
end
------------------------------------------------
--Private Functions
------------------------------------------------
function that.InitViewPanels()
	for k,v in pairs(UIPage) do 
		require ("View/" .. v.PanelName);
	end
end

--find uiroot as parent of uipages
function that.SetUIRoot()
	local root2d = GameObject.Find("2DUIRoot").transform;
	local rootTrans = root2d:FindChild('Resolution');
	GameGlobal.UIRoot.Fixed = root2d:FindChild("Resolution/Fixed");
	GameGlobal.UIRoot.Normal = root2d:FindChild("Resolution/Normal");
	GameGlobal.UIRoot.Popup = root2d:FindChild("Resolution/Popup");
	if nil == GameGlobal.UIRoot.CrossScene then
		GameGlobal.UIRoot.CrossScene = GameObject.Find("2DERoot/Resolution").transform;
	end

	local resolution = UnityEngine.Screen.width/UnityEngine.Screen.height ----屏幕宽高比
	rootTrans.localScale = Vector3.New(1,16/9/resolution,1); 
	GameGlobal.UIRoot.CrossScene.localScale = Vector3.New(1,16/9/resolution,1); 	
end

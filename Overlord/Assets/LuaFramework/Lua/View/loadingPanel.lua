--[[
loadingPanel 
Author:	yangguang
Created Date:2017/08/30
--]]

loadingPanel = {}  
local this = loadingPanel

--Monobehavour����
--======================================================================
function loadingPanel.Awake(obj)  
    this.transform = obj.transform
    -- this.InitPanel(obj) 
    logWarn("Awake lua-=======-->>"..obj.name);
end

function loadingPanel.InitPanel(obj)
    --this.a = obj:GetChild('1')
end  

function loadingPanel.OnEnable()  
    loadingCtrl.OnEnable();
end

function loadingPanel.OnDisable() 
    loadingCtrl.OnDisable();
end
--[[
mainPanel 
Author:	yangguang
Created Date:2017/08/30
--]]

mainPanel = {}  
local this = mainPanel

--Monobehavour����
--======================================================================
function mainPanel.Awake(obj)  
    this.transform = obj.transform
    -- this.InitPanel(obj) 
    logWarn("Awake lua-=======-->>"..obj.name);
end

function mainPanel.InitPanel(obj)
    --this.a = obj:GetChild('1')
end  

function mainPanel.OnEnable()  
    mainCtrl.OnEnable();
end

function mainPanel.OnDisable() 
    mainCtrl.OnDisable();
end
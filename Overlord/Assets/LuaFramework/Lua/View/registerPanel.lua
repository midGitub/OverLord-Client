--[[
registerPanel 
Author:	nzp
Created Date:2017/08/30
--]]

registerPanel = {}  
local this = registerPanel

--Monobehavour����
--======================================================================
function registerPanel.Awake(obj)  
    this.transform = obj.transform 
   -- this.InitPanel(obj) 
    logWarn("Awake lua-=======-->>"..obj.name);

end

function registerPanel.InitPanel(obj)
		
end  


function registerPanel.OnEnable()  
   regiserCtrl.OnEnable();
end

function registerPanel.OnDisable() 
   regiserCtrl.OnDisable();
end
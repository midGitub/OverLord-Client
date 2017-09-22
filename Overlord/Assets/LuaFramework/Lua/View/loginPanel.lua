--[[
loginPanel 
Author:	nzp
Created Date:2017/08/30
--]]

loginPanel = {}  
local this = loginPanel

--Monobehavour����
--======================================================================
function loginPanel.Awake(obj)  
    this.transform = obj.transform 
   -- this.InitPanel(obj) 
    logWarn("Awake lua-=======-->>"..obj.name);

end

function loginPanel.InitPanel(obj)
		
  
end  


function loginPanel.OnEnable()  
   loginCtrl.OnEnable();
end

function loginPanel.OnDisable() 
   loginCtrl.OnDisable();
end
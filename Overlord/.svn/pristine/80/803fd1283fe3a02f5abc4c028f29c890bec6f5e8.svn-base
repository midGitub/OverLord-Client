

--Create an class.
function Class(className, superName, superModulePath) 
  
  if superName ~= nil then
    require(superModulePath)
  end
  
  if _G[className] ~= nil then
    if _G[className].className == nil or _G[className].isLuaClass == nil then
      Debugger.LogError("Class name has been in the global,Please change the class name")
    end
    return
  end
  
  _G[className] = {}
  _G[className].className = className
  _G[className].isLuaClass = true
 -- _G[className].InitLuaClass = function(luaTable, csInstance, transform)
 --   luaTable.unityInstance = csInstance
 --   luaTable.transform = transform
 -- end
 --   
    
  if _G[superName] ~= nil then      
    setmetatable(_G[className], _G[superName])
    _G[superName].__index = _G[superName]    
  end  
  
   _G[className].NewInstance = function()
    local o = o or {}
    setmetatable(o, _G[className])
    _G[className].__index = _G[className]
    return o
  end

end
--MessageCenter class, singlton
Class("MessageCenter")

function MessageCenter:GetInstance()
  if self.instance == nil then
    self.instance = self:NewInstance()
    self.areaGroups = {}
  end   
  return self.instance
end

function MessageCenter:JoinArea(areaCode, luaTable)
  for k,v in pairs(self.areaGroups) do
    if k == areaCode and table.contains(self.areaGroups[k], luaTable) == false then 
     
      table.insert(self.areaGroups[k], luaTable) 
      return
    end 
  end 
  self.areaGroups[areaCode] = {}
  table.insert(self.areaGroups[areaCode], luaTable) 
end

function MessageCenter:LeaveArea(areaCode, luaTable)
  for k,v in pairs(self.areaGroups) do
    if k == areaCode then 
      table.RemoveTableItem(self.areaGroups[k], luaTable) 
      return
    end 
  end 
end

function MessageCenter:SendMessage(areaCode, head, content, callbackFunc, sender, ...)
  for k1,v1 in pairs(self.areaGroups) do
    if k1 == areaCode then
      --print(#self.areaGroups[k1])
      for k2,v2 in pairs(self.areaGroups[k1]) do
        if v2.OnMessage ~= nil then
          v2:OnMessage(head, content, callbackFunc, sender, ...)
        end
      end
      return
    end
  end
end

function MessageCenter:ClearArea(areaCode)
  if self.areaGroups[areaCode] ~= nil then
    self.areaGroups[areaCode] = nil
  end
end

function MessageCenter:ClearAllArea()
  self.areaGroups = {}
end
---------------------------------------
--region new Added by kyx 2016/07/19
---------------------------------------
function MessageCenter:GetAreaCount(areaCode)
  if self.areaGroups[areaCode] ~= nil then
    return table.getCount(self.areaGroups[areaCode]);
  else
    return 0;
  end
end
--endregion
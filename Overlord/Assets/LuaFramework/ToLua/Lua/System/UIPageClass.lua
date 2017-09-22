--kyx 2016/07/16
--a Page Mean one UI 'window'
--3 steps:
--instance ui > refresh ui by data > show
-----------------------------------------
--define
-----------------------------------------
require "System/MessageCenter"

UIType = 
{
  [0] = 0, -- Normal
  [1] = 1, -- Fixed
  [2] = 2, -- PopUp
  [3] = 3, -- None     Standalone Window
};
    
UIMode =
{
  [0] = 0, -- DoNothing,
  [1] = 1, -- HideOther,     Close Other windows
  [2] = 2, -- NeedBack,      Click Back Btn to Close Cur Window, But Not Close Other Window. Added to backSequence
  [3] = 3, -- NoNeedBack,    Close TopBar, Close other Windows, Not Added backSequence
};

UICollider =
{
  [0] = 0, -- None,      No Background without collider
  [1] = 1, -- Normal,    Trans background with collider
  [2] = 2, -- WithBg,    opaque background with collider 
};
local that = {}; --local functions
--ui page manager
UIPageManager = 
{
  m_allPages = {}; --all pages with the union type
  m_currentPageNodes = {}; --control 1>2>3>4>5 each page close will back show the previus page.
}; 
-----------------------------------------
--UIPageClass
-----------------------------------------
Class("UIPageClass");
local this = UIPageClass;
--------------------------------------------
--region member functions
--------------------------------------------
--constructor
function this:Init(id, name, uitype, uimode, uicollider, uipath, isusepool, ise) 
    logWarn("UIPageClass.Init--->>");
    self.name = name;
    self.id = id;
    self.uiType = uitype;
    self.uiMode = uimode;
    self.uiCollider = uicollider;
    self.uiPath = uipath;
    self.isUsePool = isusepool;
    self.isE = ise;
   
    self.areaCode = 777;
    MessageCenter:GetInstance():JoinArea(self.areaCode, self)
    return self;
end

function this:OnMessage(head, content, callbackFunc, sender, ...)
  print("################"..self.name.."###################");
end

--when destroy
function this:Destory()
  if self.isUsePool then
    return;
  end
  --print('          Destory name  '..self.uiType)
  --if self.name == 'TipCtrl' then
  if self.uiType == 7 then
    return
  end
  if self:CheckIfIsActive() == true then
    this.ClosePage(self);
  end
  
  MessageCenter:GetInstance():LeaveArea(self.areaCode, self);
  UIPageManager.m_allPages[self.name] = nil;
  self.gameObject = nil;
  self.transform = nil;
  self.behaviour = nil;
end

--when despawn
function this:Despawn()
  if self.isUsePool == false then
    return;
  end
--  self.behaviour:ClearClick();
logWarn(self.gameObject.name.."MemPoolManager:Despawn   this   ");
  MemPoolManager:Despawn(self.transform, self.isE);
  
  if self:CheckIfIsActive() == true then
    this.ClosePage(self);
  end
  MessageCenter:GetInstance():LeaveArea(self.areaCode, self);
  UIPageManager.m_allPages[self.name] = nil;
  self.gameObject = nil;
  self.transform = nil;
  self.behaviour = nil;
  
end 

--check if this page is active
function this:CheckIfIsActive()
  local ret = self.gameObject ~= nil and self.gameObject.activeSelf;
  return ret or self.isActived;
end
--endregion
-------------------------------------------------------------------------
--region static functions:
-------------------------------------------------------------------------
--open a page
function this.ShowPage(pageName, pageInstance, pageData, callback)   
  logWarn("pageName   is  "..pageName)   
  if pageName == nil or pageName == "" or pageInstance == nil then
    logWarn("[UI] show page error with :" .. pageName .. " maybe null instance.");
    return;
  end

  local page = nil;
  if UIPageManager.m_allPages[pageName] ~= nil then 
   
    page = UIPageManager.m_allPages[pageName];
  else
    
    UIPageManager.m_allPages[pageName] = pageInstance;
    
    page = pageInstance;
  end
 
    --if active before,wont active again.
    --if (page.isActive() == false)
    --before show should set this data if need. maybe.!!
  page.m_data = pageData;
 
  that.Show(page, callback);
end

--close current page in the "top" node.
function this.CloseTopPage()
  --Debug.Log("Back&Close PageNodes Count:" + m_currentPageNodes.Count);
  local nodecount = #UIPageManager.m_currentPageNodes;
  if nodecount > 1 then
    local closePage = UIPageManager.m_currentPageNodes[nodecount];
    table.remove(UIPageManager.m_currentPageNodes, nodecount);
    --show older page.
    --TODO:Sub pages.belong to root node.
    nodecount = #UIPageManager.m_currentPageNodes;   
    local page = UIPageManager.m_currentPageNodes[nodecount];             
    this.ShowPage(page.name, page, nil, function () closePage.Hide(); end);           
  end   
end

--Close the target page
function this.ClosePage(target)
  if target == nil then  
    return; 
  end
  local nodecount = #UIPageManager.m_currentPageNodes; 
  if target:CheckIfIsActive() == false then    
    local i;
    for i = 1, nodecount, 1 do                    
      if UIPageManager.m_currentPageNodes[i] == target then                        
        table.remove(UIPageManager.m_currentPageNodes, i);
        break;
      end
    end
    return;
  end

  if nodecount >= 1 and UIPageManager.m_currentPageNodes[nodecount] == target then
    table.remove(UIPageManager.m_currentPageNodes, nodecount);
    --show older page.
    --TODO:Sub pages.belong to root node.
    nodecount = #UIPageManager.m_currentPageNodes; 
    if nodecount > 0 then                
      local page = UIPageManager.m_currentPageNodes[nodecount];    

 --     print("++++++++++++++++++++++++++++  target.Hide" .. target.name)              
      this.ShowPage(page.name, page, nil, function () target.Hide(); end);
      return;
    end
  elseif that.CheckIfNeedBack(target) == true then
    local i;       
    for i = 1, nodecount, 1 do
      if UIPageManager.m_currentPageNodes[i] == target then
        table.remove(UIPageManager.m_currentPageNodes, i);
        if(target.name~=TipCtrl) then
          target.Hide();
        end
    
        break;
      end
    end
  end

  if(target.name~=TipCtrl) then
          target.Hide();
  end
end


function this.ClearNodes()
  local nodecount = #UIPageManager.m_currentPageNodes;
  if nodecount > 0 then
    local i;
    for i = nodecount, 1, -1 do
      UIPageManager.m_currentPageNodes[i] = nil;
    end
  end
end

function this.ClearAllPages()
  logWarn("start    ClearAllPages  ")
  for k,v in pairs(UIPageManager.m_allPages) do
      v:Destory();
      v:Despawn();
  end
end
--endregion
-----------------------------------------
-----that
-----------------------------------------
--Async Show UI Logic  
function that.Show(page, callback)  
  print("show------------------" .. page.name);
  
  if page.gameObject == nil and page.uiPath ~= nil and page.uiPath ~= "" then
    print("create=====================" .. page.name);
	  if page.isUsePool then
		  PanelManager:SpawnPanel(page.uiPath, page.isE, function (obj)  
			that.AnchorUIGameObject(page, obj);
			page.Awake();
			page.Active();
			page.Refresh();
			that.PopNode(page);
			if nil ~= callback then
			  callback();
			end
		end);
	else
		--[[PanelManager:CreatePanel(page.uiPath, function (obj)
			that.AnchorUIGameObject(page, obj);
			page.Awake();
			page.Active();
			page.Refresh();
			that.PopNode(page);
			if nil ~= callback then
			  callback();
			end
		end);--]]
   
		--PanelManager:CreateFGUIPanel(page.uiPath, function (obj)
    resMgr:LoadFGUI(page.uiPath,function (obj)
			that.AnchorUIGameObject(page, obj);
      
			page.Awake(obj);
			page.Active();
			page.Refresh();
			that.PopNode(page);
			if nil ~= callback then
			  callback();
			end
		end);
     
	end
  else
    page.Active();
    page.Refresh();
    that.PopNode(page);
    if nil ~= callback then
			  callback();
		end
  end
end
--set parent position scale rotation etc.
function that.AnchorUIGameObject(page,obj)
  panel = GameObject.Find(obj.gameObjectName)
  page.behaviour = panel.transform:GetComponent("LuaBehaviour")  
  
  logWarn("---------->   AnchorUIGameObject")
--  page.gameObject = obj;
 -- page.transform = obj.transform;
--  page.behaviour = obj.transform:GetComponent('LuaBehaviour'); 
  --TODO
  --Set Parent
  --Set transform(position, scale, rotation etc）
end
--make the target node to the top.
function that.PopNode(page)
  if nil == page then
    LogError("[UI] page popup is null.");
  else
    if that.CheckIfNeedBack(page) == true then
    print("PopNode::UIPageManager.m_currentPageNodes:" .. #UIPageManager.m_currentPageNodes);
      local i;
      for i = 1, #UIPageManager.m_currentPageNodes, 1 do
            
        if UIPageManager.m_currentPageNodes[i] == page then        
           table.remove(UIPageManager.m_currentPageNodes, i); 
          print("!!!!!!!!!!break for!!!!!!!!!!!!!!!");
           break;
        end
      end
      UIPageManager.m_currentPageNodes[#UIPageManager.m_currentPageNodes + 1] = page;    

      --after pop should hide the old node if need.
      that.HideOldNodes();
    end
  end   
end

--after pop should hide the old node if need.
function that.HideOldNodes()
  local pagecount = #UIPageManager.m_currentPageNodes;
  if pagecount > 0 then
    local topPage = UIPageManager.m_currentPageNodes[pagecount];
    if topPage.uiMode == UIMode[1] then
      --form bottm to top.
      local i;
      for i = pagecount -1, 1, -1 do
        if UIPageManager.m_currentPageNodes[i]:CheckIfIsActive() == true then
          if UIPageManager.m_currentPageNodes[i].name == 'TipCtrl' then          
              return
          end
          if UIPageManager.m_currentPageNodes[i].name == 'LoadingCtrl' then          
              return
          end
          UIPageManager.m_currentPageNodes[i].Hide();
        end                        
      end
    end
  end
end
--CheckIfNeedBack
function that.CheckIfNeedBack(page)
  if page.uiType == UIType[1] or page.uiType == UIType[2] or page.uiType == UIType[3] then
    return false;
  elseif page.uiMode == UIMode[0] or page.uiMode == UIMode[2] then
    return false;
  else
    return true;
  end  
end


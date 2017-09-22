--TableReader
--kyx
--2016/3/21

require "Data/SaveTableToFile"
local json = require "cjson"
local util = require "3rd/cjson.util"

TableReader = {};
local this = TableReader;
local LocalFuns = {};

function this.InitTables()
    local count = #TableName;
    for i = 1, count do
        this[TableName[i]] = require ("Data/LuaTable/" .. TableName[i]);
    end
end

function this.SaveToLuaTable()
	print(Util.DataPath)
    local count = #TableName;
    local tbl = nil;
    for i = 1, count do
        tbl = LocalFuns.ReadTable(Util.DataPath .. "SkillBase" .. ".json");
        table.EaseSave(tbl, "c:/" .. "SkillBase" .. ".lua")
    end
end

--读表通用函数
function LocalFuns.ReadTable(filepath)   
    local _dataTable = {};
    local _text = util.file_load(filepath);
    local _data = json.decode(_text);
    
    local _theType = _data[1];    
    local _field = _data[3];
    
    table.remove(_data,1);
    table.remove(_data,1);
    table.remove(_data,1);    
	--print('______________'..#_data)
    for i = 1, #_data do
        local _tValue = {};
        for j = 1, #_theType do
           if _theType[j] == "NUM" or _theType[j] == "FLOAT"  then             
               _tValue[_field[j]] = tonumber(_data[i][j]);
           elseif _theType[j] == "TEXT" then             
                _tValue[_field[j]] = _data[i][j];
           else
                print("table err");
           end
        end
        _dataTable[_tValue[_field[1]]] = _tValue;
    end      
    return _dataTable;
end
--------------------------------------------------------------------------------
--以下为读json表
--------------------------------------------------------------------------------
--[=[
local buildingAreaPath = Util.DataPath.."lua/Data/Tables/buildingArea.json";  
local buildingAreaTable = {};
function this.GetbuildingAreaTable()
  
    if #buildingAreaTable < 1 then
        print("GetbuildingAreaTable new-----------------")
        buildingAreaTable = LocalFuns.ReadTable(buildingAreaPath);
    end
    return buildingAreaTable;
    
end

--大地图相关-->
--区域--
local areaPath = Util.DataPath.."lua/Data/Tables/area.json";  
local areaTable = {};

function this.GetareaTable()
    if #areaTable < 1 then
        areaTable = LocalFuns.ReadTable(areaPath);
    end
    return areaTable;
end

--副本--
local mapPath = Util.DataPath.."lua/Data/Tables/map.json";  
local mapTable = {};

function this.GetmapTable()
    if #mapTable < 1 then
        mapTable = LocalFuns.ReadTable(mapPath);
    end
    return mapTable;
end

-- local map_levelPath = Util.DataPath.."lua/Data/Tables/map_level.json";  
-- local map_levelTable = {};

-- function TableReader.Getmap_levelTable()
    -- if #map_levelTable < 1 then
      -- print("Getmap_levelTable new-----------------");
        -- map_levelTable = LocalFuns.ReadTable(map_levelPath);
    -- end
    -- return map_levelTable;
-- end


--副本关卡--
local mapLevelPath = Util.DataPath.."lua/Data/Tables/map_level.json";  
local mapLevelTable = {};

function this.GetmapLevelTable()
    if #mapLevelTable < 1 then
        mapLevelTable = LocalFuns.ReadTable(mapLevelPath);
    end
	
    return mapLevelTable;
end

--活动--
local activityPath = Util.DataPath.."lua/Data/Tables/activity.json";  
local activityTable = {};

function this.GetactivityTable()
    if #activityTable < 1 then
      print("GetactivityTable new-----------------");
        activityTable = LocalFuns.ReadTable(activityPath);
    end
    return activityTable;
end

--岛屿--
local islandPath = Util.DataPath.."lua/Data/Tables/island.json";  
local islandTable = {};

function this.GetislandTable()
    if #islandTable < 1 then
      print("GetislandTable new-----------------");
        islandTable = LocalFuns.ReadTable(islandPath);
    end
    return islandTable;
	
end

--资源岛--
local resourcePath = Util.DataPath.."lua/Data/Tables/resource.json";  
local resourceTable = {};

function this.GetresourceTable()
    if #resourceTable < 1 then
      print("Getresource_settingTable new-----------------");
        resourceTable = LocalFuns.ReadTable(resourcePath);
    end
    return resourceTable;
end

--资源岛防御--
local resource_settingPath = Util.DataPath.."lua/Data/Tables/resource_setting.json";  
local resource_settingTable = {};

function this.Getresource_settingTable()
    if #resource_settingTable < 1 then
      print("Getresource_settingTable new-----------------");
        resource_settingTable = LocalFuns.ReadTable(resource_settingPath);
    end
    return resource_settingTable;
	
end
--<

--战舰相关---->

local shipBasePath = Util.DataPath.."lua/Data/Tables/shipBase.json";  
local shipBaseTable = {};

function this.GetshipBaseTable()
    if #shipBaseTable < 1 then
    -- print("GetshipBaseTable new-----------------");
        shipBaseTable = LocalFuns.ReadTable(shipBasePath);
    end
    return shipBaseTable;
	
end

local shipStarPath = Util.DataPath.."lua/Data/Tables/shipStar.json";  
local shipStarTable = {};

function this.GetshipStarTable()
    if #shipStarTable < 1 then
    -- print("GetshipStarTable new-----------------");
        shipStarTable = LocalFuns.ReadTable(shipStarPath);
    end
    return shipStarTable;
	
end

local shipPartsPath = Util.DataPath.."lua/Data/Tables/shipParts.json";  
local shipPartsTable = {};

function this.GetshipPartsTable()
    if #shipPartsTable < 1 then
     -- print("GetshipPartsTable new-----------------");
        shipPartsTable = LocalFuns.ReadTable(shipPartsPath);
    end
    return shipPartsTable;
	
end

local shipSlotsLevelPath = Util.DataPath.."lua/Data/Tables/shipSlotsLevel.json";  
local shipSlotsLevelTable = {};

function this.GetshipSlotsLevelTable()
    if #shipSlotsLevelTable < 1 then
     -- print("GetshipSlotsLevelTable new-----------------");
        shipSlotsLevelTable = LocalFuns.ReadTable(shipSlotsLevelPath);
    end
    return shipSlotsLevelTable;
	
end

local shipTypePath = Util.DataPath.."lua/Data/Tables/shipType.json";  
local shipTypeTable = {};

function this.GetshipTypeTable()
    if #shipTypeTable < 1 then
    --  print("GetshipTypeTable new-----------------");
        shipTypeTable = LocalFuns.ReadTable(shipTypePath);
    end
    return shipTypeTable;
	
end
----<

--英雄相关---->
local heroBasePath = Util.DataPath.."lua/Data/Tables/heroBase.json";  
local heroBaseTable = {};

function this.GetheroBaseTable()
    if #heroBaseTable < 1 then
    --  print("GetheroBaseTable new-----------------");
        heroBaseTable = LocalFuns.ReadTable(heroBasePath);
    end
    return heroBaseTable;
end

local heroStarPath = Util.DataPath.."lua/Data/Tables/heroStar.json";  
local heroStarTable = {};

function this.GetheroStarTable()
    if #heroStarTable < 1 then
     -- print("GetheroStarTable new-----------------");
        heroStarTable = LocalFuns.ReadTable(heroStarPath);
    end
    return heroStarTable;
end

local equipmentBasePath = Util.DataPath.."lua/Data/Tables/equipmentBase.json";  
local equipmentBaseTable = {};

function this.GetequipmentBaseTable()
    if #equipmentBaseTable < 1 then
    --  print("GetequipmentBaseTable new-----------------");
        equipmentBaseTable = LocalFuns.ReadTable(equipmentBasePath);
    end
    return equipmentBaseTable;
end

local equipmentBlueprintPath = Util.DataPath.."lua/Data/Tables/equipmentBlueprint.json";  
local equipmentBlueprintTable = {};

function this.GetequipmentBlueprintTable()
    if #equipmentBlueprintTable < 1 then
     print("GetequipmentBlueprintTable new-----------------");
       equipmentBlueprintTable = LocalFuns.ReadTable(equipmentBlueprintPath);
    end
    return equipmentBlueprintTable;
end
----<


--获取建筑详情主表buildingGuide
local buildingGuidePath = Util.DataPath.."lua/Data/Tables/buildingGuide.json"
local buildingGuideTable = {};
function this.GetBuildingGuideTable()  
    if #buildingGuideTable < 1 then
        print("new-----------------");
        buildingGuideTable = LocalFuns.ReadTable(buildingGuidePath);
    end
    return buildingGuideTable;
    
end


function this.GetAllBuildTable(buildtype)
	
	if buildtype == 1 then 
		return this.GetguildHallTable();
	elseif buildtype == 2 then
		return this.GettavernTable();
	elseif buildtype == 3 then
		return this.GetgoldMineTable();
	elseif buildtype == 4 then
		return this.GetloggingCampTable();
	elseif buildtype == 5 then
		return this.GetironMineTable();
	elseif buildtype == 6 then
		return this.GetplantationTable();
	elseif buildtype == 7 then
		return this.GetbreweryTable();
	elseif buildtype == 8 then
		return this.GetshipYardTable();
	elseif buildtype == 9 then
		return this.GetsailMakerTable();
	elseif buildtype == 10 then
		return this.GetarsenalTable();
	elseif buildtype == 11 then
		return this.GetartisanCornerTable();
	elseif buildtype == 12 then
		return this.GetlightTowerTable();
	elseif buildtype == 13 then
		return this.GetresearchCenterTable();
	elseif buildtype == 14 then
		return this.GetcollegeTable();
	elseif buildtype == 15 then
		return this.GetwareHouseTable();
	elseif buildtype == 16 then
		return this.GetdockTable();
	elseif buildtype == 17 then
		return this.GetmarketTable();
	elseif buildtype == 18 then
		return this.GetplanetariumTable();
	elseif buildtype == 19 then
		return this.GetfactoryCarpenterTable();
	end


end



--获取市政厅信息表
local guildHallPath = Util.DataPath.."lua/Data/Tables/guildHall.json"
local guildHallTable = {};
function this.GetguildHallTable()
    if #guildHallTable < 1 then
        print("new-----------------guildHall");
        guildHallTable = LocalFuns.ReadTable(guildHallPath);
    end
    return guildHallTable;   
end

--获取酒馆信息表
local tavernPath = Util.DataPath.."lua/Data/Tables/tavern.json"
local tavernTable = {};
function this.GettavernTable()
    if #tavernTable < 1 then
        print("new-----------------tavern");
        tavernTable = LocalFuns.ReadTable(tavernPath);
    end
    return tavernTable;   
end

--获取金矿信息表
local goldMinePath = Util.DataPath.."lua/Data/Tables/goldMine.json"
local goldMineTable = {};
function this.GetgoldMineTable()
    if #goldMineTable < 1 then
        print("new-----------------goldMine");
        goldMineTable = LocalFuns.ReadTable(goldMinePath);
    end
    return goldMineTable;   
end

--获取伐木场信息表
local loggingCampPath = Util.DataPath.."lua/Data/Tables/loggingCamp.json"
local loggingCampTable = {};
function this.GetloggingCampTable()
    if #loggingCampTable < 1 then
        print("new-----------------loggingCamp");
        loggingCampTable = LocalFuns.ReadTable(loggingCampPath);
    end
    return loggingCampTable;   
end

--获取铁矿信息表
local ironMinePath = Util.DataPath.."lua/Data/Tables/ironMine.json"
local ironMineTable = {};
function this.GetironMineTable()
    if #ironMineTable < 1 then
        print("new-----------------ironMine");
        ironMineTable = LocalFuns.ReadTable(ironMinePath);
    end
    return ironMineTable;   
end

--获取种植园信息表
local plantationPath = Util.DataPath.."lua/Data/Tables/plantation.json"
local plantationTable = {};
function this.GetplantationTable()
    if #plantationTable < 1 then
        print("new-----------------plantation");
        plantationTable = LocalFuns.ReadTable(plantationPath);
    end
    return plantationTable;   
end

--获取酿酒厂信息表
local breweryPath = Util.DataPath.."lua/Data/Tables/brewery.json"
local breweryTable = {};
function this.GetbreweryTable()
    if #breweryTable < 1 then
        print("new-----------------brewery");
        breweryTable = LocalFuns.ReadTable(breweryPath);
    end
    return breweryTable;   
end

--造船厂
local shipYardPath = Util.DataPath.."lua/Data/Tables/factoryShip.json"
local shipYardTable = {};
function this.GetshipYardTable()
    if #shipYardTable < 1 then
        print("new-----------------factoryShip");
        shipYardTable = LocalFuns.ReadTable(shipYardPath);
    end
    return shipYardTable;   
end

--制帆厂
local sailMakerPath = Util.DataPath.."lua/Data/Tables/factorySail.json"
local sailMakerTable = {};
function this.GetsailMakerTable()
    if #sailMakerTable < 1 then
        print("new-----------------sailMaker");
        sailMakerTable = LocalFuns.ReadTable(sailMakerPath);
    end
    return sailMakerTable;   
end

--兵工厂
local arsenalPath = Util.DataPath.."lua/Data/Tables/factoryWeapon.json"
local arsenalTable = {};
function this.GetarsenalTable()
    if #arsenalTable < 1 then
        print("new-----------------factoryWeapon");
        arsenalTable = LocalFuns.ReadTable(arsenalPath);
    end
    return arsenalTable;   
end

--工匠坊
local artisanCornerPath = Util.DataPath.."lua/Data/Tables/factoryCraftsman.json"
local artisanCornerTable = {};
function this.GetartisanCornerTable()
    if #artisanCornerTable < 1 then
        print("new-----------------factoryCraftsman");
        artisanCornerTable = LocalFuns.ReadTable(artisanCornerPath);
    end
    return artisanCornerTable;   
end

--木工坊
local factoryCarpenterPath = Util.DataPath.."lua/Data/Tables/factoryCarpenter.json"
local factoryCarpenterTable = {};
function this.GetfactoryCarpenterTable()
    if #factoryCarpenterTable < 1 then
        print("new-----------------factoryCraftsman");
        factoryCarpenterTable = LocalFuns.ReadTable(factoryCarpenterPath);
    end
    return factoryCarpenterTable;   
end

--灯塔
local lightTowerPath = Util.DataPath.."lua/Data/Tables/lightTower.json"
local lightTowerTable = {};
function this.GetlightTowerTable()
    if #lightTowerTable < 1 then
        print("new-----------------lightTower");
        lightTowerTable = LocalFuns.ReadTable(lightTowerPath);
    end
    return lightTowerTable;   
end

--研究中心
local researchCenterPath = Util.DataPath.."lua/Data/Tables/researchCenter.json"
local researchCenterTable = {};
function this.GetresearchCenterTable()
    if #researchCenterTable < 1 then
        print("new-----------------researchCenter");
        researchCenterTable = LocalFuns.ReadTable(researchCenterPath);
    end
    return researchCenterTable;   
end

--学院
local collegePath = Util.DataPath.."lua/Data/Tables/college.json"
local collegeTable = {};
function this.GetcollegeTable()
    if #collegeTable < 1 then
        print("new-----------------college");
        collegeTable = LocalFuns.ReadTable(collegePath);
    end
    return collegeTable;   
end

--仓库
local wareHousePath = Util.DataPath.."lua/Data/Tables/wareHouse.json"
local wareHouseTable = {};
function this.GetwareHouseTable()
    if #wareHouseTable < 1 then
        print("new-----------------wareHouse");
        wareHouseTable = LocalFuns.ReadTable(wareHousePath);
    end
    return wareHouseTable;   
end

--船坞
local dockPath = Util.DataPath.."lua/Data/Tables/dock.json"
local dockTable = {};
function this.GetdockTable()
    if #dockTable < 1 then
        print("new-----------------dock");
        dockTable = LocalFuns.ReadTable(dockPath);
    end
    return dockTable;   
end

--市场
local marketPath = Util.DataPath.."lua/Data/Tables/market.json"
local marketTable = {};
function this.GetmarketTable()
    if #marketTable < 1 then
        print("new-----------------market");
        marketTable = LocalFuns.ReadTable(marketPath);
    end
    return marketTable;   
end

--天文馆
local planetariumPath = Util.DataPath.."lua/Data/Tables/planetarium.json"
local planetariumTable = {};
function this.GetplanetariumTable()
    if #planetariumTable < 1 then
        print("new-----------------planetarium");
        planetariumTable = LocalFuns.ReadTable(planetariumPath);
    end
    return planetariumTable;   
end

--龙骨信息表
local keelPath = Util.DataPath.."lua/Data/Tables/keel.json";  
local keelTable = {};
function this.GetkeelTable()
    if #keelTable < 1 then
        print("new-----------------keel");
        keelTable = LocalFuns.ReadTable(keelPath);
    end
    return keelTable;   
end
--船帆信息表
local sailPath = Util.DataPath.."lua/Data/Tables/sail.json"; 
local sailTable = {};
function this.GetsailTable()
    if #sailTable < 1 then
        print("new-----------------sail");
        sailTable = LocalFuns.ReadTable(sailPath);
    end
    return sailTable;   
end
--舰炮信息表
local navalGunPath = Util.DataPath.."lua/Data/Tables/navalGun.json"; 
local navalGunTable = {};
function this.GetnavalGunTable()
    if #navalGunTable < 1 then
        print("new-----------------navalGun");
        navalGunTable = LocalFuns.ReadTable(navalGunPath);
    end
    return navalGunTable;   
end
--船首像信息表
local shipStatuaryPath = Util.DataPath.."lua/Data/Tables/shipStatuary.json"; 
local shipStatuaryTable = {};
function this.GetshipStatuaryTable()
    if #planetariumTable < 1 then
        print("new-----------------shipStatuary");
        shipStatuaryTable = LocalFuns.ReadTable(shipStatuaryPath);
    end
    return shipStatuaryTable;   
end
--科技信息表
local technologyPath = Util.DataPath.."lua/Data/Tables/technology.json"; 
local technologyTable = {};
function this.GettechnologyTable()
    if #technologyTable < 1 then
        print("new-----------------technology");
        technologyTable = LocalFuns.ReadTable(technologyPath);
    end
    return technologyTable;   
end


--钻石表信息
local diamondPath = Util.DataPath.."lua/Data/Tables/diamond.json"; 
local diamondTable = {};
function this.GetdiamondTable()
    if #diamondTable < 1 then
        print("new-----------------diamond!");
        diamondTable = LocalFuns.ReadTable(diamondPath);
		
    end
    return diamondTable;   
end

--造船组件表
local shipPartPath = Util.DataPath.."lua/Data/Tables/shipParts.json"; 
local shipPartTable = {};
function this.GetshipPartTable()
    if #shipPartTable < 1 then
        print("new-----------------shipPart!");
        shipPartTable = LocalFuns.ReadTable(shipPartPath);
		
    end
    return shipPartTable;   
end



--]=]
--cjson callback--

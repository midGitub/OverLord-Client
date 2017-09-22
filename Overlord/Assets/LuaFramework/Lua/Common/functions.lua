
--输出日志--
function log(str)
    Util.Log(str);
end

--错误日志--
function logError(str) 
	Util.LogError(str);
end

--警告日志--
function logWarn(str) 
	Util.LogWarning(str);
end

--查找对象--
function find(str)
	return GameObject.Find(str);
end

function destroy(obj)
	GameObject.Destroy(obj);
end

function newObject(prefab)
	return GameObject.Instantiate(prefab);
end

--创建面板--
function createPanel(name)
	PanelManager:CreatePanel(name);
end

function child(str)
	return transform:FindChild(str);
end

function subGet(childNode, typeName)		
	return child(childNode):GetComponent(typeName);
end

function findPanel(str) 
	local obj = find(str);
	if obj == nil then
		error(str.." is null");
		return nil;
	end
	return obj:GetComponent("BaseLua");
end


local visited_table_list = {}
function GetTableInfoRecursive(tbl, layer)
	if layer == 0 then
		visited_table_list = {}
	end
	visited_table_list[tbl] = true
	local result = getRepeatStr("\t", layer) .. "{\n"
	for key, value in pairs(tbl) do
		result = result .. getRepeatStr("\t", layer+1)
		if type(value) == "string" then
			result = result .. string.format("%s = \"%s\"\n", tostring(key), tostring(value))
		else
			result = result .. string.format("%s = %s\n", tostring(key), tostring(value))
		end

		if type(value) == "table" and not visited_table_list[value] then
			result = result .. GetTableInfoRecursive(value, layer+1)
		end
	end
	result = result .. getRepeatStr("\t", layer) .. "}\n"

	return result
end

function PrintTable(tbl, not_print)
	if type(tbl) ~= "table" then
		print(string.format("[PrintTable] arg1 %s is not a table", tostring(tbl)))
		return
	end

	local str = GetTableInfoRecursive(tbl, 0)
	if not not_print then
		print("\n" .. str)
	end
	return str
end

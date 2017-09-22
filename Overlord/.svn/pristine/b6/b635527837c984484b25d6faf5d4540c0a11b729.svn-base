--Utility 
--by kyx 
--2016/-7/20
--Clone a table
function table.clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end

--if a table contains some element
function table.contains(thetable, element)
  if thetable == nil then
        return false
  end

  for _, value in pairs(thetable) do
    if value == element then
      return true
    end
  end
  return false
end

--remove an element from a table
function table.removeElement(thetable, element)
    if table.contains(thetable, element) == true then
        local index = 0;
        local count = #thetable;
        local i =0;
        for i = 1, count, 1 do
            if thetable[i] == element then
                table.remove(thetable, i);
                break;
            end
        end
    else
        return false;
    end
end

--merge a table (tSrc) to a table(tDest)
function table.merge( tDest, tSrc )
    for k, v in pairs( tSrc ) do
        tDest[k] = v
    end
end

--get the elements count of a table.
function table.getCount(thetable)
	local count = 0
	
	for k, v in pairs(thetable) do
		count = count + 1	
	end
	
	return count
end

function table.RemoveTableItem(list, item, removeAll)
    local rmCount = 0

    for i = 1, #list do
        if list[i - rmCount] == item then
            table.remove(list, i - rmCount)

            if removeAll then
                rmCount = rmCount + 1
            else
                break
            end
        end
    end
end
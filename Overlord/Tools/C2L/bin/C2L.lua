require "lfs"

local num=0 

local function Split(szFullString, szSeparator)   
	local nFindStartIndex = 1 
	local nSplitIndex = 1 
	local nSplitArray = {}   
	while true do   
		local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)   
		if not nFindLastIndex then   
			nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))  
		break 
		end   
		nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1)  
		nFindStartIndex = nFindLastIndex + string.len(szSeparator)   
		nSplitIndex = nSplitIndex + 1 
	end   
	return nSplitArray   
end 

local function readFile(csvName)
	local path = './csv/' .. csvName .. '.csv'
    local f = assert(io.open(path, 'r'))
    local string = f:read("*all")
    f:close()
    return string
end

local function writeFile(path, str)
    local f = assert(io.open(path, 'w'))
    f:write(str)
    f:close()
end

local function convert(name)
	local csvStr=readFile(name)
	local tableStr=''
	local strS='	'
	local clines=Split(csvStr,'\n')
	local dataTypes=Split(clines[1],',')
	local dataNames=Split(clines[3],',')
	for n=4,(#clines-1) do
		local line=Split(clines[n],',')
		local lineStr=''
		for m=1,#line do
			local s=""
			if	dataTypes[m]=='TEXT'	then
				s='"'..line[m]..'"'
			else
				s=line[m]
			end
			lineStr=lineStr..strS..strS..'["'..dataNames[m]..'"]='..s..',\n'
		end
		tableStr=tableStr..strS..'['..line[1]..']={\n'.. lineStr..strS..'},\n'
	end
	
    local path = './lua/' .. name .. '.lua'
    local luaStr = 'return {\n'..tableStr..'}'
    writeFile(path, luaStr)
end

local function start(path)
    for fileName in lfs.dir(path) do
        if fileName ~= "." and fileName ~= ".." then
            if string.find(fileName, ".csv") ~= nil then
                local filePath = path .. '/' .. fileName
                fileName = string.sub(fileName, 1, string.find(fileName, ".csv") - 1)
                convert(fileName)
                num = num + 1
            end
         end
    end
end

print(">>>>>> 开始转换 <<<<<<\n")

start("./csv")

print("\n>>>>>> 转换 " .. num .." 个文件成功 <<<<<<\n")
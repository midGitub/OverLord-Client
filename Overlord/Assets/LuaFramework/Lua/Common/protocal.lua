--Buildin Table
Protocal = {
	Connect		= '101';	--连接服务器
	Exception   = '102';	--异常掉线
	Disconnect  = '103';	--正常断线   
	Message		= '104';	--接收消息
}

NetMsg = {
    --用户---->
    [10]='main.userHandler.validateUser',					--登录注册
    [11]='gate.gateHandler.queryEntry',						--请求进入服务器
    [12]='connector.entryHandler.entry',					--进入验证玩家
    [13]='connector.roleHandler.createPlayer',				--创建玩家
    [20]='area.playerHandler.enterArea',					--进入主城
    [21]='area.playerHandler.getAllData',					--获取玩家数据
    [22]='area.playerHandler.changeNickname',               --修改玩家昵称
    [23]='connector.entryHandler.reconnect',                    --断线重连
    [24]='area.playerHandler.getPushMessage',
    --大地图
    [30]='area.roomHandler.enterBattleRoom',				--进入房间
    [31]='area.roomHandler.exitBattleRoom',					--离开房间
    [32]="area.roomHandler.move",                           --移动
    [33]="area.roomHandler.moveOperate",                    --移动命令
    [34]="area.roomHandler.attackOperate",                  --攻击命令
    [35]='area.roomHandler.startBattleRoom',	            --开始战斗
	[36]='area.roomHandler.damageOperate',					--伤害
    [37]='area.roomHandler.cancelPracticeMatching',			--退出匹配
	[38]='area.roomHandler.changeAttackArea',
	[39]="area.roomHandler.changeSpeed",                    --速度命令
	
	--工厂
	[50]='area.tanksHandler.changeTankEquipment',	            --更换坦克装备
	[51]='area.tanksHandler.buyTanks',							--够买坦克
	[52]='area.tanksHandler.readyBattleTank',					--坦克上阵
	[53]='area.tanksHandler.buypos',							--购买装备位置
	[54]='area.tanksHandler.equipitem',							--装备碎片
	[55]='area.tanksHandler.shipUpDate',						--船只升级
	
	--道具
	[60]='area.itembagHandler.itemCompose',						--道具合成


	[1000] = "onSomeoneEnter",								--别人进入
	[1005] = "onSomeoneExit",								--别人退出
	
	[1010] = "onSomeoneResult",								--战斗结算消息
	[1011] = "onEnterRoom",									--进入房间推送消息
	[1012] = "onCanStart",									--开始
	
	
    [1006] = "onTick",                                      --位置信息刷新推送
    [1007] = "onOperate",                                   --操作消息推送
	[1008] = "onFire",										--攻击
	[1009] = "onDamage",									--受击
	[1013] = "onChangeAttackArea",							--改变攻击方向
	[1014] = "onChangeSpeed",							--改变攻击方向

--[[
    [2001]='onBuildingUpgradeComplete',                     --推送升级消息
    [2002]='onProduceComponentComplete',                    --推送生产部件完成消息
    [2005]='onResourceOutput',                              --推送资源产出消息

    [3000]='onFreeChoose',                                  --免费招募时间到
    --]]
}

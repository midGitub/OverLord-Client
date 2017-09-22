using UnityEngine;
using System.Collections;
using LuaFramework;

public class StartUpCommand : ControllerCommand
{

    public override void Execute(IMessage message)
    {
        if (!Util.CheckEnvironment()) return;

        GameObject gameMgr = GameObject.Find("GlobalGenerator");
        if (gameMgr != null)
        {
            AppView appView = gameMgr.AddComponent<AppView>();
        }
        //-----------------关联命令-----------------------
        AppFacade.Instance.RegisterCommand(NotiConst.DISPATCH_MESSAGE, typeof(SocketCommand));

        //-----------------初始化管理器-----------------------
        AppFacade.Instance.AddManager<LuaFramework.LuaManager>(ManagerName.Lua);
        AppFacade.Instance.AddManager<LuaFramework.PanelManager>(ManagerName.Panel);
        AppFacade.Instance.AddManager<LuaFramework.SoundManager>(ManagerName.Sound);
        AppFacade.Instance.AddManager<LuaFramework.TimerManager>(ManagerName.Timer);
        AppFacade.Instance.AddManager<LuaFramework.NetworkManager>(ManagerName.Network);
        AppFacade.Instance.AddManager<LuaFramework.ResourceManager>(ManagerName.Resource);
        AppFacade.Instance.AddManager<LuaFramework.ThreadManager>(ManagerName.Thread);
        AppFacade.Instance.AddManager<LuaFramework.ObjectPoolManager>(ManagerName.ObjectPool);
        AppFacade.Instance.AddManager<LuaFramework.GameManager>(ManagerName.Game);
    }
}

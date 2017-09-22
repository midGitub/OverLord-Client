using UnityEngine;
using System.Collections;
using LuaFramework;
using UnityEngine.UI;
using System.Collections.Generic;
public class ShowLogTool : MonoBehaviour
{
    public bool showFlag = true;
    public int logCount = 10;
    private string m_ShowLog = string.Empty;
    private Queue<string> logQueue = new Queue<string>();
    //private string unityLog = "";

    void Start()
    {
        Application.logMessageReceived += WriteUnityLog;
    }
    public void WriteMyLog(string log)
    {
        WriteInLogQueue(log);
    }


    void WriteUnityLog(string log, string stackTrace, LogType type)
    {
        WriteInLogQueue(log);
    }

    void WriteInLogQueue(string log)
    {
        logQueue.Enqueue(log);
        while (logQueue.Count > logCount)
        {
            logQueue.Dequeue();
        }
        m_ShowLog = string.Empty;
        foreach (string onelog in logQueue)
        {
            m_ShowLog = m_ShowLog + "\r\n" + onelog;
        }
    }

    void OnGUI()
    {
        if (showFlag)
        {
            GUI.color = Color.blue;
            GUI.Label(new Rect(0, Screen.height / 2, 1000, 1000), m_ShowLog);
        }
    }


}

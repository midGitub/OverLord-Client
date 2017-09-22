using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class UseHero1 {
    public static GComponent _mainView;
    
    public static void PlayHero()
    {
        GameObject g = GameObject.Find("heroPanel");
        _mainView = g.GetComponent<UIPanel>().ui;

        Object prefab = Resources.Load("Prefabs/Role/npc");
        GameObject go = (GameObject)Object.Instantiate(prefab);
        go.transform.localPosition = new Vector3(61, -89, 1000); //set z to far from UICamera is important!
        go.transform.localScale = new Vector3(180, 180, 180);
        go.transform.localEulerAngles = new Vector3(0, 100, 0);
        go.AddComponent<HeroCheck>();
        _mainView.GetChild("holder").asGraph.SetNativeObject(new GoWrapper(go));
    }
}
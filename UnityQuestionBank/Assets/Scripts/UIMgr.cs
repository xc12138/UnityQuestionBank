using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// UI管理器
/// </summary>
public class UIMgr
{
    public void Init()
    {
        m_canvasTrans = GameObject.Find("Canvas").transform;
    }

    public GameObject ShowUI(string resPath)
    {
        var prefab = ResMgr.instance.GetRes(resPath);
        if (prefab == null) return null;
        var uiObj = Object.Instantiate(prefab);
        uiObj.transform.SetParent(m_canvasTrans, false);
        return uiObj;
    }

    private Transform m_canvasTrans;

    private static UIMgr s_instance;
    public static UIMgr instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new UIMgr();
            return s_instance;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源管理器
/// </summary>
public class ResMgr
{
    public GameObject GetRes(string resPath)
    {
        if (m_prefabs.ContainsKey(resPath))
        {
            return m_prefabs[resPath];
        }
        var go = Resources.Load<GameObject>(resPath);
        m_prefabs[resPath] = go;
        return go;
    }

    private Dictionary<string, GameObject> m_prefabs = new Dictionary<string, GameObject>();

    private static ResMgr s_instance;
    public static ResMgr instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new ResMgr();
            return s_instance;
        }
    }
}

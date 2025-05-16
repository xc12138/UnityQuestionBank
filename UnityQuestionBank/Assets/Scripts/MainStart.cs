using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIMgr.instance.Init();

        MainPanel.Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

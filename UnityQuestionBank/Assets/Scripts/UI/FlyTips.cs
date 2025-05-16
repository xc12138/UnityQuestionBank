using System;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 提示语
/// </summary>
public class FlyTips : MonoBehaviour
{
    public AnimationTrigger aniTrigger;
    public Text tipsText;

    public static void Show(string txt)
    {
        var uiObj = UIMgr.instance.ShowUI("UIPrefabs/FlyTips");
        var tips = uiObj.GetComponent<FlyTips>();
        tips.OnShow(txt);
    }

    private void OnShow(string txt)
    {
        tipsText.text = txt;
        aniTrigger.aniEvent = (msg) =>
        {
            if ("finish" == msg)
            {
                Destroy(gameObject);
            }
        };
    }
}

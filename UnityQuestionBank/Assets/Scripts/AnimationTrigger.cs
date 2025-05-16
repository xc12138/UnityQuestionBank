using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public Action<string> aniEvent;

    public void TriggerEvent(string msg)
    {
        aniEvent?.Invoke(msg);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyDebug : MonoBehaviour
{
    public static MyDebug init;

    public Text text;
    public void Log(string mes)
    {
        text.text += " --- 9 --- " + mes;
    }
    private void Awake()
    {
        if(init == null)
        {
            init = this;
        }
    }
}

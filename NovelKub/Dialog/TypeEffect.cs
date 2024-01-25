using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public bool isAnim;
    public int CharPerSeconds;
    public string targetMsg;


    Text msgText;
    int index;
    float interval;


    private void Awake()
    {
        //msgText - GetComponent<Text>();
    }

    public void SetMsg(string msg)
    {
        if (isAnim)
        {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }

        else
        {
            targetMsg = msg;
            EffectStart();
        }
    }

    void EffectStart()
    {
        msgText.text = "";
        index = 0;

        interval = 1.0f / CharPerSeconds;
        Debug.Log(interval);

        isAnim = true;

        Invoke("Effecting", interval);
    }

    void Effecting()
    {
        if(msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[index];
        index++;

        Invoke("Effecting", interval);
    }


    void EffectEnd()
    {
        isAnim = false;
    }
}


// 게임 매니저같은곳에서 pbulic TypeEffect dialogText;
//dialog.SetMsg() 해서 이걸로 출력
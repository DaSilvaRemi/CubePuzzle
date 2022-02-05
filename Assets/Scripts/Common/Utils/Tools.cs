using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.Tweening;

public delegate float EasingFuncDelegate(float t);

public class Tools
{
    public enum GameState
    {
        MENU,
        PLAY,
        PAUSE,
        WIN,
        GAMEOVER,
        ENDLVL
    }

    public enum GameScene
    {
        MENUSCENE = 0, 
        FIRSTLEVELSCENE, 
        SECONDLVLSCENE, 
        THIRDLEVELSCENE, 
        FOURTHLEVELSCENE, 
        HELPSCENE, 
        CREDITSCENE
    }

    public static void Log(Component component, string msg)
    {
        Debug.Log(Time.frameCount + " " + component.GetType().Name + " " + msg);
    }

    public static string FormatFloatNumberToString(float number)
    {
        return number.ToString("N01");
    }

    public static IEnumerator MyTranslateCoroutine(Transform transform, Vector3 startPos, Vector3 endPos, float duration,
       EasingFuncDelegate easingFuncDelegate, Action startAction = null, Action endAction = null)
    {
        float elapsedTime = 0;

        if (startAction != null)
        {
            startAction();
        }

        while (elapsedTime < duration)
        {
            float k = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, endPos, easingFuncDelegate(k));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        if (endAction != null)
        {
            endAction();
        }
    }
}
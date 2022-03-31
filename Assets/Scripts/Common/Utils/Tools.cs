using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.Tweening;
using Random = UnityEngine.Random;

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
        CREDITSCENE,
        VICTORYSCENE
    }

    public enum MoveDirection 
    { 
        LEFT, 
        RIGHT, 
        UP, 
        DOWN, 
        FORWARD, 
        BACK, 
        NONE 
    };

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

        startAction?.Invoke();

        while (elapsedTime < duration)
        {
            float k = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, endPos, easingFuncDelegate(k));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        endAction?.Invoke();
    }

    public static IEnumerator MyActionCoroutine(float duration, Action startAction = null, Action duringAction = null, Action endAction = null)
    {
        float elapsedTime = 0;

        startAction?.Invoke();

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            duringAction?.Invoke();
            yield return null;
        }

        endAction?.Invoke();
    }

    public static void SetRandomColor(GameObject gameObject)
    {
        MeshRenderer[] mr = gameObject.GetComponentsInChildren<MeshRenderer>();

        if (mr != null && mr.Length > 0)
        {
            for (int i = 0; i < mr.Length; i++)
            {
                Tools.SetColor(mr[i], Random.ColorHSV());
            }
        }
    }

    public static void SetColor(MeshRenderer meshRenderer, Color color)
    {
        if (meshRenderer != null && color != null)
        {
            meshRenderer.material.color = color;
        }
    }
}